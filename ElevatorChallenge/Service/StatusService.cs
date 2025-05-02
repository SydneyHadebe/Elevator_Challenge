using ElevatorChallenge.Enums;
using ElevatorChallenge.Logging;
using ElevatorChallenge.Models;

namespace ElevatorChallenge.Service
{
    public class StatusService : IStatusService
    {
        private readonly ILogger _logger;

        public StatusService(ILogger logger) => _logger = logger;

        public void SetStatus(Elevator elevator, ElevatorStatus status)
        {
            elevator.Status = status;
            _logger.LogInfo($"Elevator {elevator.Id} status set to {status}.");
        }

        public string GetFormattedStatus(Elevator elevator)
        {
            string overload = elevator.PassengerCount > elevator.MaxCapacity ? " [OVERLOADED]" : "";

            string direction = elevator.Direction switch
            {
                Direction.Up => "Moving Up",
                Direction.Down => "Moving Down",
                Direction.Stopped => "Stopped",
                _ => "Unknown"
            };

            return $"Elevator {elevator.Id}: Floor {elevator.CurrentFloor}, " +
                   $"Status {elevator.Status}, Direction {direction}, " +
                   $"Passengers {elevator.PassengerCount}/{elevator.MaxCapacity}{overload}";
        }
    }

}
