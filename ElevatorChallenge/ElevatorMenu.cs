using ElevatorChallenge.Handlers;
using ElevatorChallenge.Interface;
using ElevatorChallenge.Logging;
using ElevatorChallenge.Service;

namespace ElevatorChallenge
{
    public class ElevatorMenu
    {
        private readonly IElevatorService _service;
        private readonly ILogger _logger;

        private readonly Dictionary<string, string> _adminCredentials = new()
        {
            { "admin1", "adminpass" }
        };

        public ElevatorMenu(IElevatorService service, ILogger logger)
        {
            _service = service;
            _logger = logger;
        }

        public void Run()
        {
            while (true)
            {
                switch (SelectRole())
                {
                    case "User":
                        IMenuHandler userHandler = new UserMenuHandler(_service, _logger);
                        userHandler.ShowMenu();
                        break;

                    case "Admin":
                        IMenuHandler adminHandler = new AdminMenuHandler(_service, _logger);
                        adminHandler.ShowMenu();
                        break;

                    case "Exit":
                        return;

                    default:
                        Console.WriteLine("Invalid role selected.");
                        break;
                }
            }
        }

        private string SelectRole()
        {
            Console.WriteLine("Select role:");
            Console.WriteLine("1. User");
            Console.WriteLine("2. Admin");
            Console.WriteLine("3. Exit");
            string input = Console.ReadLine();

            return input switch
            {
                "1" => "User",
                "2" => AuthenticateAdmin() ? "Admin" : "Invalid",
                "3" => "Exit",
                _ => "Invalid"
            };
        }

        private bool AuthenticateAdmin()
        {
            Console.Write("Admin Username: ");
            string user = Console.ReadLine();

            Console.Write("Admin Password: ");
            string pass = GetPasswordInput(); // Use the method to get password input with masking

            if (_adminCredentials.TryGetValue(user, out var correctPass) && correctPass == pass)
            {
                Console.WriteLine($"Welcome, Admin {user}!");
                return true;
            }

            Console.WriteLine("Invalid admin credentials.");
            return false;
        }

        // Method to handle password input with masking
        private string GetPasswordInput()
        {
            string password = string.Empty;
            ConsoleKeyInfo key;

            // Loop to capture each character entered
            while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter) // Wait until Enter is pressed
            {
                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    // If backspace is pressed, remove the last character from the password
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b"); // Erase the last character on the console
                }
                else if (key.Key != ConsoleKey.Backspace)
                {
                    password += key.KeyChar;
                    Console.Write("*"); // Print * to mask the character
                }
            }
            Console.WriteLine(); // Move to the next line after password entry
            return password;
        }
    }
}
