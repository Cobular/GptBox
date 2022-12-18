// ReSharper disable InconsistentNaming
namespace JackboxGPT3.Services
{
    public interface IConfigurationProvider
    {
        public string EcastHost { get; }
        public string LogLevel { get; }
        
        public string OpenAIEngine { get; }
        public string OpenAIKey { get; }
    }
}
