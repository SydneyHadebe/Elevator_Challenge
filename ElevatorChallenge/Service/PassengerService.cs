using ElevatorChallenge.Enums;
using ElevatorChallenge.Logging;
using ElevatorChallenge.Models;

namespace ElevatorChallenge.Service
{
    public class PassengerService : IPassengerService
    {
        private readonly ILogger _logger;
        private readonly IDoorService _doorService;

        public PassengerService(ILogger logger, IDoorService doorService)
        {
            _logger = logger;
            _doorService = doorService;
        }

        public void AddPassenger(Elevator elevator)
        {
            if (elevator.Status == ElevatorStatus.UnderMaintenance) return;

            if (elevator.PassengerCount < elevator.MaxCapacity)
            {
                elevator.PassengerCount++;
                _logger.LogInfo($"Passenger added. Total: {elevator.PassengerCount}");
            }
            else
            {
                _logger.LogWarning($"Elevator {elevator.Id} is overloaded.");
                elevator.DoorsOpen = true;
            }
        }

        public void RemovePassenger(Elevator elevator)
        {
            if (elevator.PassengerCount > 0)
            {
                elevator.PassengerCount--;
                _logger.LogInfo($"Passenger removed. Total: {elevator.PassengerCount}");

                if (elevator.PassengerCount <= elevator.MaxCapacity)
                    _doorService.CloseDoors(elevator);
            }
        }
    }

}
