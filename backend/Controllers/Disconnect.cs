using JackboxGPT3.Engines;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GptBoxApi.Controllers;

[ApiController]
[Route("disconnect")]
public class DisconnectController : BaseGptController
{
    public DisconnectController(ILogger<DisconnectController> logger, IGptBoxDependency jackbox) : base(logger, jackbox)
    {
    }    
    
    [HttpPost(Name = "Disconnect")]
    public IActionResult Post([FromQuery(Name = "room_code")] string room_code)
    {
        var game_code_clean = room_code.ToUpper().Trim();


        if (_running_games.ContainsKey(game_code_clean) == false)
        {
            _logger.LogInformation($"Game engine does not exist for code {game_code_clean}!");
            return BadRequest("Not in that game");
        }

        var this_engine = _running_games[game_code_clean];

        _logger.LogInformation($"Disconnecting from game {game_code_clean}!. There are currently {_running_games.Count} games running.");
        this_engine.Disconnect();

        return Ok("Disconnected");
    }
}
