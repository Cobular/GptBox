namespace JackboxGPT3
{
  public interface IJackboxGPT3Dependency
  {
    void WriteMessage(string message);
  }

  public class JackboxGPT3Dependency : IJackboxGPT3Dependency
  {
    public JackboxGPT3Dependency()
    {
      Console.WriteLine("MyDependency.ctor");
    }
    public void WriteMessage(string message)
    {
      Console.WriteLine($"MyDependency.WriteMessage Message: {message}");
    }
  }
}