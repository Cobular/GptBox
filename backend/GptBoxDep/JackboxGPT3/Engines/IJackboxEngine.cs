namespace JackboxGPT3.Engines
{
  public interface IJackboxEngine
  {
    public abstract GameStatus GetGameStatus();
   public abstract void Disconnect();
    public event EventHandler OnDisconnect;
  }
}
