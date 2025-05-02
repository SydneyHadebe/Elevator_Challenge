using ElevatorChallenge.Enums;
using ElevatorChallenge.Models;

namespace ElevatorChallenge.Service
{
    public interface IElevatorService
    {
        void MoveElevator(int elevatorId, int targetFloor);
        void AddPassenger(int elevatorId);
        void RemovePassenger(int elevatorId);
        void OpenDoors(int elevatorId);
        void CloseDoors(int elevatorId);
        void SetElevatorStatus(int elevatorId, ElevatorStatus status);
        void ScheduleMaintenance(int elevatorId);
        int GetPassengerCount(int elevatorId);
        void ReportAllElevatorStatuses();
        string GetElevatorFormattedStatus(Elevator elevator);
        Elevator GetElevator(int elevatorId);
    }
}
