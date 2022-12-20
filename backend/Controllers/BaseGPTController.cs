using JackboxGPT3.Engines;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GptBoxApi.Controllers;

public class BaseGptController : ControllerBase
{
    protected readonly Dictionary<string, IJackboxEngine> _running_games;

    protected readonly ILogger _logger;
    protected readonly IGptBoxDependency _jackbox;

    public BaseGptController(ILogger logger, IGptBoxDependency jackbox)
    {
        _logger = logger;
        _jackbox = jackbox;
        _running_games = _jackbox.RunningGames;
    }
}
