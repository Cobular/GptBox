using JackboxGPT3.Games.Common;
using JackboxGPT3.Services;

namespace JackboxGPT3.Engines
{
    public abstract class BaseJackboxEngine<TClient> : IJackboxEngine
        where TClient : IJackboxClient
    {
        protected abstract string Tag { get; }

        protected readonly ICompletionService CompletionService;
        protected readonly TClient JackboxClient;
        
        private readonly ILogger _logger;

    public event EventHandler OnDisconnect;

    public void Disconnect() {
      OnDisconnect?.Invoke(this, EventArgs.Empty);
    }

    public abstract GameStatus GetGameStatus();

        protected BaseJackboxEngine(ICompletionService completionService, ILogger logger, TClient client)
        {
            CompletionService = completionService;
            JackboxClient = client;
            JackboxClient.OnDisconnect += (sender, args) => OnDisconnect?.Invoke(this, EventArgs.Empty);
            _logger = logger;
        }
        
        // ReSharper disable UnusedMember.Global
        protected void LogWarning(string text)
        {
            _logger.LogWarning($"[{Tag}] {text}");
        }

        protected void LogError(string text)
        {
            _logger.LogError($"[{Tag}] {text}");
        }

        protected void LogDebug(string text)
        {
            _logger.LogDebug($"[{Tag}] {text}");
        }

        protected void LogVerbose(string text)
        {
            _logger.LogTrace($"[{Tag}] {text}");
        }

        protected void LogInfo(string text)
        {
            _logger.LogInformation($"[{Tag}] {text}");
        }
        // ReSharper restore UnusedMember.Global
    }
}
