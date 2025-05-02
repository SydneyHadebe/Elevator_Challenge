using ElevatorChallenge.Enums;
using ElevatorChallenge.Models;

namespace ElevatorChallenge.Service
{
    public interface IStatusService
    {
        void SetStatus(Elevator elevator, ElevatorStatus status);
        string GetFormattedStatus(Elevator elevator);
    }


}
