using ElevatorChallenge.Models;

namespace ElevatorChallenge.Service
{
    public interface IMoveElevatorService
    {
        void MoveElevator(Elevator elevator, int targetFloor);
    }


}
