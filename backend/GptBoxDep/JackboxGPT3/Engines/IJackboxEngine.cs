namespace JackboxGPT3.Engines
{
  public interface IJackboxEngine
  {
    public abstract GameStatus GetGameStatus();

    public event EventHandler OnDisconnect;
  }
}
