using System.Net;
using Autofac;
using JackboxGPT3.Engines;
using JackboxGPT3.Games.Common.Models;
using Newtonsoft.Json;

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
  void WriteMessage(string message);
  Task<GameStatus> ConnectToGame(string gameCode);
  // GameStatus DisconnectFromGame();
  // GameStatus GetGameStatus();
}

// The implementation of the dependency
public class GptBoxDependency : IGptBoxDependency
{
  private readonly ILogger<GptBoxDependency> _logger;
  private readonly IHttpClientFactory _httpClientFactory;

  private readonly GptBoxOptions config;
  private readonly IContainer jackboxGpt3Container;

  public GptBoxDependency(ILogger<GptBoxDependency> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
  {
    _logger = logger;
    _httpClientFactory = httpClientFactory;

    var _config = configuration.GetRequiredSection(GptBoxOptions.GptBox).Get<GptBoxOptions>();
    if (_config == null)
    {
      throw new Exception("GptBoxOptions is null");
    }
    config = _config;

    jackboxGpt3Container = JackboxGPT3.Startup.InternalSetup(config, logger);

    _logger.LogInformation("GptBox Dependency Initialized");
  }

  public async Task<GameStatus> ConnectToGame(string gameCode)
  {
    var _httpClient = _httpClientFactory.CreateClient();

    var roomCode = config.RoomCode;
    var ecastHost = config.EcastHost;

    _logger.LogDebug($"Ecast host: {ecastHost}");
    _logger.LogInformation($"Trying to join room with code: {roomCode}");

    var response = await _httpClient.GetAsync($"https://{ecastHost}/api/v2/rooms/{roomCode}");

    try
    {
      response.EnsureSuccessStatusCode();
    }
    catch (HttpRequestException ex)
    {
      if (ex.StatusCode != HttpStatusCode.NotFound)
        throw;

      _logger.LogError("Room not found.");
      return GameStatus.RoomNotFound;
    }

    var roomResponse = JsonConvert.DeserializeObject<GetRoomResponse>(await response.Content.ReadAsStringAsync());
    var tag = roomResponse.Room.AppTag;

    if (!jackboxGpt3Container.IsRegisteredWithKey<IJackboxEngine>(tag))
    {
      _logger.LogError($"Unsupported game: {tag}");
      return GameStatus.UnsupportedGame;
    }

    _logger.LogInformation($"Room found! Starting up {tag} engine...");
    jackboxGpt3Container.ResolveNamed<IJackboxEngine>(tag);

    return GameStatus.Connected;
  }

  public void WriteMessage(string message)
  {
    _logger.LogInformation($"JackboxGPT3Dependency.WriteMessage Message: {message}");
  }
}