using JackboxGPT3.Engines;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GptBoxApi.Controllers;

[ApiController]
[Route("[controller]")]
public class GptBoxController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly Dictionary<string, IJackboxEngine> _running_games;

    private readonly ILogger<GptBoxController> _logger;
    private readonly IGptBoxDependency _jackbox;

    public GptBoxController(ILogger<GptBoxController> logger, IGptBoxDependency jackbox)
    {
        _logger = logger;
        _logger.LogInformation("GptBoxController created!");
        _jackbox = jackbox;
        _running_games = _jackbox.RunningGames;
    }

    [HttpPost(Name = "JoinGame")]
    public async Task<IActionResult> Post([FromQuery(Name = "gameCode")] string game_code)
    {
        var game_code_clean = game_code.ToUpper().Trim();

        if (_running_games.ContainsKey(game_code_clean))
        {
            _logger.LogInformation($"Game engine already exists for code {game_code_clean}!");
            return Ok("We're already in that game!");
        }

        _logger.LogInformation($"Joining game {game_code_clean}!. There are currently {_running_games.Count} games running.");

        var res = await _jackbox.ConnectToGame(game_code_clean);

        if (res.Item2 != null)
        {
            _running_games.Add(game_code_clean, res.Item2);
            res.Item2.OnDisconnect += (sender, args) => {
                _logger.LogInformation($"Removed game {game_code_clean} from running games.");
                _running_games.Remove(game_code_clean);
            };

            return Ok("Game engine created!");
        }

        switch (res.Item1)
        {
            case GameStatus.Connected:
                return Ok("Connected");
            case GameStatus.RoomNotFound:
                return NotFound("Room not found");
            case GameStatus.UnsupportedGame:
                return BadRequest("Unsupported game");
            case GameStatus.Disconnection:
                return BadRequest("Disconnection");
            default:
                return BadRequest("Unknown error");
        }
    }
}
