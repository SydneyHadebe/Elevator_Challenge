using ElevatorChallenge.Logging;
using ElevatorChallenge.Models;

namespace ElevatorChallenge.Service
{
    public class DoorService : IDoorService
    {
        private readonly ILogger _logger;

        public DoorService(ILogger logger) => _logger = logger;

        public void OpenDoors(Elevator elevator)
        {
            elevator.DoorsOpen = true;
            _logger.LogInfo($"Opening doors for Elevator {elevator.Id}.");
        }

        public void CloseDoors(Elevator elevator)
        {
            if (elevator.PassengerCount > elevator.MaxCapacity)
            {
                _logger.LogWarning($"Cannot close doors. Elevator {elevator.Id} is overloaded.");
                elevator.DoorsOpen = true;
                return;
            }

            elevator.DoorsOpen = false;
            _logger.LogInfo($"Closing doors for Elevator {elevator.Id}.");
        }
    }

}
