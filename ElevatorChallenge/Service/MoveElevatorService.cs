using ElevatorChallenge.Enums;
using ElevatorChallenge.Logging;
using ElevatorChallenge.Models;

namespace ElevatorChallenge.Service
{
    public class MoveElevatorService : IMoveElevatorService
    {
        private readonly ILogger _logger;

        public MoveElevatorService(ILogger logger) => _logger = logger;

        public void MoveElevator(Elevator elevator, int targetFloor)
        {
            if (elevator.Status == ElevatorStatus.UnderMaintenance)
            {
                _logger.LogWarning($"Elevator {elevator.Id} is under maintenance and cannot be moved.");
                return;
            }

            elevator.TargetFloor = targetFloor;

            if (elevator.PassengerCount > elevator.MaxCapacity)
            {
                _logger.LogWarning($"Elevator {elevator.Id} is over capacity and cannot move.");
                return;
            }

            if (targetFloor < 0)
            {
                _logger.LogError($"Invalid target floor: {targetFloor}. Movement aborted.");
                return;
            }

            if (elevator.CurrentFloor == targetFloor)
            {
                _logger.LogInfo($"Elevator {elevator.Id} is already at floor {targetFloor}.");
                return;
            }

            elevator.Direction = elevator.CurrentFloor < targetFloor ? Direction.Up : Direction.Down;
            elevator.Status = ElevatorStatus.InTransit;

            _logger.LogInfo($"Moving Elevator {elevator.Id} from floor {elevator.CurrentFloor} to {targetFloor} – Direction: {elevator.Direction}");

            elevator.CurrentFloor = targetFloor;

            elevator.Direction = Direction.Stopped;
            elevator.Status = ElevatorStatus.Available;

            _logger.LogInfo($"Elevator {elevator.Id} arrived at floor {targetFloor}. Status: Available.");
        }
    }

}
