using ElevatorChallenge.Logging;
using ElevatorChallenge.Service;
using System;

namespace ElevatorChallenge.Handlers
{
    public class UserMenuHandler : MenuHandlerBase
    {
        public UserMenuHandler(IElevatorService service, ILogger logger)
            : base(service, logger) { }

        public override void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("User Menu:");
                Console.WriteLine("1. Move Elevator");
                Console.WriteLine("2. Add Passenger");
                Console.WriteLine("3. Remove Passenger");
                Console.WriteLine("4. Get Elevator Status");
                Console.WriteLine("5. Get Passenger Count");
                Console.WriteLine("6. Return to Main Menu");

                string choice = Console.ReadLine();
                if (choice == "6") return;

                try
                {
                    HandleChoice(choice, isAdmin: false);  // Users will not have admin privileges
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
