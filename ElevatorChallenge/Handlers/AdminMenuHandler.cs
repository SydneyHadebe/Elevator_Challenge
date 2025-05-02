using ElevatorChallenge.Logging;
using ElevatorChallenge.Service;

namespace ElevatorChallenge.Handlers
{
    public class AdminMenuHandler : MenuHandlerBase
    {
        public AdminMenuHandler(IElevatorService service, ILogger logger)
            : base(service, logger) { }

        public override void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Admin Menu:");
                Console.WriteLine("1. Move Elevator");
                Console.WriteLine("2. Add Passenger");
                Console.WriteLine("3. Remove Passenger");
                Console.WriteLine("4. Open Doors");
                Console.WriteLine("5. Close Doors");
                Console.WriteLine("6. Set Elevator Status");
                Console.WriteLine("7. Schedule Maintenance");
                Console.WriteLine("8. Get Elevator Status");
                Console.WriteLine("9. Get Passenger Count");
                Console.WriteLine("10. Return to Main Menu");

                string choice = Console.ReadLine();
                if (choice == "10") return;

                try
                {
                    HandleChoice(choice, isAdmin: true);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                }

                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }
    }
}
