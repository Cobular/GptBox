using JackboxGPT3.Engines;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GptBoxApi.Controllers;

[ApiController]
[Route("status")]
public class StatusController : BaseGptController
{
    public StatusController(ILogger<StatusController> logger, IGptBoxDependency jackbox) : base(logger, jackbox) {}    
    
    [HttpGet(Name = "Get Game Status")]
    public IActionResult Get([FromQuery(Name = "room_code")] string room_code)
    {
        var game_code_clean = room_code.ToUpper().Trim();
        return Ok(_running_games.ContainsKey(game_code_clean) ? "Connected" : "Not Connected");
    }
}
