using JackboxGPT3.Engines;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GptBoxApi.Controllers;

[ApiController]
[Route("type")]
public class TypeController : BaseGptController
{
    public TypeController(ILogger<TypeController> logger, IGptBoxDependency jackbox) : base(logger, jackbox)
    {
    }    
    
    [HttpGet(Name = "Get Game Type")]
    public async Task<IActionResult> Get([FromQuery(Name = "room_code")] string room_code)
    {
        var game_code_clean = room_code.ToUpper().Trim();

        var game_type = await _jackbox.GetGameType(game_code_clean);

        if (game_type == null)
        {
            return NotFound("Room not found");
        }

        return Ok(game_type);
    }
}
