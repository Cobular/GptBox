public class GptBoxOptions : JackboxGPT3.Services.DefaultConfigurationProvider
{
  public const string GptBox = "GptBox";

  public override string OpenAIEngine { get; set; } = String.Empty;
  public override string LogLevel { get; set; } = String.Empty;

  public override string OpenAIKey { get; } = String.Empty;
}