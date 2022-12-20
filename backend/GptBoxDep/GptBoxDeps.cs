using System.Net;
using Autofac;
using Autofac.Core;
using JackboxGPT3.Engines;
using JackboxGPT3.Games.Common.Models;
using JackboxGPT3.Games.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[JsonConverter(typeof(StringEnumConverter))]
public enum GameStatus
{
  Connected,
  RoomNotFound,
  UnsupportedGame,
  Disconnection
}

// The public interface of this dependency
public interface IGptBoxDependency
{
  /// Connects to a game, creating and returning a new game engine if successful
  Task<Tuple<GameStatus, IJackboxEngine?>> ConnectToGame(string gameCode);
  Task<string?> GetGameType(string room_code);

  public Dictionary<string, IJackboxEngine> RunningGames { get; }

  // GameStatus DisconnectFromGame();
  // GameStatus GetGameStatus();
}

// The implementation of the dependency
public class GptBoxDependency : IGptBoxDependency
{
  private readonly ILogger<GptBoxDependency> _logger;
  private readonly HttpClient _httpClient;

  private readonly GptBoxOptions config;
  private readonly IContainer jackboxGpt3Container;

  private readonly Dictionary<string, IJackboxEngine> _RunningGames = new();

  public GptBoxDependency(ILogger<GptBoxDependency> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
  {
    _logger = logger;
    _httpClient = httpClientFactory.CreateClient();

    var _config = configuration.GetRequiredSection(GptBoxOptions.GptBox).Get<GptBoxOptions>();
    if (_config == null)
    {
      throw new Exception("GptBoxOptions is null");
    }
    config = _config;

    jackboxGpt3Container = JackboxGPT3.Startup.InternalSetup(config, logger);

    _logger.LogInformation("GptBox Dependency Initialized");
  }

  public Dictionary<string, IJackboxEngine> RunningGames => _RunningGames;

  public async Task<string?> GetGameType(string room_code) { 
    _logger.LogDebug($"Ecast host: {config.EcastHost}");
    _logger.LogInformation($"Trying to join room with code: {room_code}");

    var response = await _httpClient.GetAsync($"https://{config.EcastHost}/api/v2/rooms/{room_code}");

    try
    {
      response.EnsureSuccessStatusCode();
    }
    catch (HttpRequestException ex)
    {
      if (ex.StatusCode != HttpStatusCode.NotFound)
        return null;

      _logger.LogError("Room not found.");
      return null;
    }

    return JsonConvert.DeserializeObject<GetRoomResponse>(await response.Content.ReadAsStringAsync()).Room.AppTag;
  }

  public async Task<Tuple<GameStatus, IJackboxEngine?>> ConnectToGame(string room_code)
  {
    var tag = await GetGameType(room_code);

    if (tag == null)
      return Tuple.Create<GameStatus, IJackboxEngine?>(GameStatus.RoomNotFound, null);

    if (!jackboxGpt3Container.IsRegisteredWithKey<IJackboxEngine>(tag))
    {
      _logger.LogError($"Unsupported game: {tag}");
      return Tuple.Create<GameStatus, IJackboxEngine?>(GameStatus.UnsupportedGame, null);
    }

    _logger.LogInformation($"Room found! Starting up {tag} engine...");

    Parameter[] constructor_params = new Parameter[2] {
      new NamedParameter("player_name", "GPT3"),
      new NamedParameter("room_code", room_code)
    };

    IJackboxEngine? engine = jackboxGpt3Container.ResolveNamed<IJackboxEngine>(tag, constructor_params);

    return Tuple.Create(GameStatus.Connected, engine);
  }
}