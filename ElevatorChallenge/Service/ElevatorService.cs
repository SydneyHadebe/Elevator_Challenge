using ElevatorChallenge.Enums;
using ElevatorChallenge.Logging;
using ElevatorChallenge.Models;
using System;
using System.Collections.Generic;

namespace ElevatorChallenge.Service
{
    public class ElevatorService : IElevatorService
    {
        private readonly Dictionary<int, Elevator> _elevators;
        private readonly ILogger _logger;
        private readonly IMoveElevatorService _moveElevatorService;
        private readonly IPassengerService _passengerService;
        private readonly IDoorService _doorService;
        private readonly IStatusService _statusService;

        public ElevatorService(
            IEnumerable<Elevator> elevators,
            ILogger logger,
            IMoveElevatorService moveElevatorService,
            IPassengerService passengerService,
            IDoorService doorService,
            IStatusService statusService)
        {
            if (elevators == null) throw new ArgumentNullException(nameof(elevators));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            _elevators = elevators.ToDictionary(e => e.Id);
            _logger = logger;
            _moveElevatorService = moveElevatorService;
            _passengerService = passengerService;
            _doorService = doorService;
            _statusService = statusService;

            foreach (var elevator in _elevators.Values)
            {
                if (elevator.PassengerCount > elevator.MaxCapacity)
                {
                    _logger.LogWarning($"Elevator {elevator.Id} is over capacity and cannot move.");
                    elevator.PassengerCount = elevator.MaxCapacity;
                    elevator.DoorsOpen = true;
                }
            }
        }

        public Elevator GetElevator(int elevatorId) =>
            _elevators.TryGetValue(elevatorId, out var elevator)
                ? elevator
                : LogAndReturnNull($"Elevator {elevatorId} not found.");

        private Elevator LogAndReturnNull(string message)
        {
            _logger.LogError(message);
            return null;
        }

        public void MoveElevator(int elevatorId, int targetFloor)
        {
            var elevator = GetElevator(elevatorId);
            if (elevator != null)
                _moveElevatorService.MoveElevator(elevator, targetFloor);
        }

        public void AddPassenger(int elevatorId)
        {
            var elevator = GetElevator(elevatorId);
            if (elevator != null)
                _passengerService.AddPassenger(elevator);
        }

        public void RemovePassenger(int elevatorId)
        {
            var elevator = GetElevator(elevatorId);
            if (elevator != null)
                _passengerService.RemovePassenger(elevator);
        }

        public void OpenDoors(int elevatorId)
        {
            var elevator = GetElevator(elevatorId);
            if (elevator != null)
                _doorService.OpenDoors(elevator);
        }

        public void CloseDoors(int elevatorId)
        {
            var elevator = GetElevator(elevatorId);
            if (elevator != null)
                _doorService.CloseDoors(elevator);
        }

        public void SetElevatorStatus(int elevatorId, ElevatorStatus status)
        {
            var elevator = GetElevator(elevatorId);
            if (elevator != null)
                _statusService.SetStatus(elevator, status);
        }

        public void ScheduleMaintenance(int elevatorId) =>
            SetElevatorStatus(elevatorId, ElevatorStatus.UnderMaintenance);

        public int GetPassengerCount(int elevatorId)
        {
            var elevator = GetElevator(elevatorId);
            return elevator?.PassengerCount ?? -1;
        }

        public void ReportAllElevatorStatuses()
        {
            foreach (var elevator in _elevators.Values)
            {
                _logger.LogInfo(_statusService.GetFormattedStatus(elevator));
            }
        }

        public string GetElevatorFormattedStatus(Elevator elevator) =>
            _statusService.GetFormattedStatus(elevator);
    }

}
