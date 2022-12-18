public class GptBoxOptions : JackboxGPT3.Services.DefaultConfigurationProvider
{
  public const string GptBox = "GptBox";

  public override string PlayerName { get; set; } = String.Empty;
  public override string OpenAIEngine { get; set; } = String.Empty;
  public override string RoomCode { get; set; } = String.Empty;
  public override string LogLevel { get; set; } = String.Empty;
}