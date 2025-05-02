using ElevatorChallenge.Models;

namespace ElevatorChallenge.Service
{
    public interface IDoorService
    {
        void OpenDoors(Elevator elevator);
        void CloseDoors(Elevator elevator);
    }


}
