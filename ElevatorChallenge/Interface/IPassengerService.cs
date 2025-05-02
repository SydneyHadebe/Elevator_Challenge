using ElevatorChallenge.Models;

namespace ElevatorChallenge.Service
{
    public interface IPassengerService
    {
        void AddPassenger(Elevator elevator);
        void RemovePassenger(Elevator elevator);
    }


}
