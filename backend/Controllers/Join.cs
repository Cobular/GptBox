using JackboxGPT3.Engines;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GptBoxApi.Controllers;

[ApiController]
[Route("join")]
public class JoinController : BaseGptController
{
    public JoinController(ILogger<JoinController> logger, IGptBoxDependency jackbox) : base(logger, jackbox)
    {
    }

    [HttpPost(Name = "JoinGame")]
    public async Task<IActionResult> Post([FromQuery(Name = "room_code")] string room_code)
    {
        var game_code_clean = room_code.ToUpper().Trim();

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
