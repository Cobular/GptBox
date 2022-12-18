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

    private readonly ILogger<GptBoxController> _logger;
    private readonly IGptBoxDependency _jackbox;

    public GptBoxController(ILogger<GptBoxController> logger, IGptBoxDependency jackbox)
    {
        _logger = logger;
        _jackbox = jackbox;
    }

    [HttpPost(Name = "JoinGame")]
    public async Task<IActionResult> Post([FromQuery(Name = "gameCode")] string game_code)
    {
        var res = await _jackbox.ConnectToGame(game_code.ToUpper());

        switch (res)
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
