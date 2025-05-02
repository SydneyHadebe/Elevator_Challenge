using ElevatorChallenge.Logging;

namespace ElevatorChallenge.Tests
{
    public class TestLogger : ILogger
    {
        public List<string> Messages { get; } = new();

        public void LogInfo(string message) => Messages.Add(message);
        public void LogWarning(string message) => Messages.Add(message);
        public void LogError(string message) => Messages.Add(message);
    }

}
