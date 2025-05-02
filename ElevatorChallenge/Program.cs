using ElevatorChallenge.Enums;
using ElevatorChallenge.Logging;
using ElevatorChallenge.Models;
using ElevatorChallenge.Service;

namespace ElevatorChallenge
{
    public class Program
    {
        public static void Main()
        {
            // Create elevators
            var elevators = new List<Elevator>
            {
                new Elevator(1, 10, ElevatorCategory.Passenger),
                new Elevator(2, 10, ElevatorCategory.Freight),
                new Elevator(3, 15, ElevatorCategory.HighSpeed),
                new Elevator(4, 8, ElevatorCategory.Glass)
            };

            // Core logger
            ILogger logger = new ConsoleLogger();

            // Create services with DI-style wiring
            IDoorService doorService = new DoorService(logger);
            IMoveElevatorService moveElevatorService = new MoveElevatorService(logger);
            IStatusService statusService = new StatusService(logger);
            IPassengerService passengerService = new PassengerService(logger, doorService);

            // Compose the main elevator service
            IElevatorService elevatorService = new ElevatorService(
                elevators,
                logger,
                moveElevatorService,
                passengerService,
                doorService,
                statusService
            );

            // Launch UI/menu
            var menu = new ElevatorMenu(elevatorService, logger);
            menu.Run();
        }
    }
}
