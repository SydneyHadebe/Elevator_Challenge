using ElevatorChallenge.Enums;
using ElevatorChallenge.Interface;
using ElevatorChallenge.Logging;
using ElevatorChallenge.Service;
using System;

namespace ElevatorChallenge.Handlers
{
    public abstract class MenuHandlerBase : IMenuHandler
    {
        protected readonly IElevatorService _service;
        protected readonly ILogger _logger;

        protected MenuHandlerBase(IElevatorService service, ILogger logger)
        {
            _service = service;
            _logger = logger;
        }

        public abstract void ShowMenu();

        protected virtual void HandleChoice(string choice, bool isAdmin)
        {
            try
            {
                switch (choice)
                {
                    case "1":
                        _service.MoveElevator(ReadElevatorId(), ReadInt("Enter target floor: "));
                        break;
                    case "2":
                        _service.AddPassenger(ReadElevatorId());
                        break;
                    case "3":
                        _service.RemovePassenger(ReadElevatorId());
                        break;
                    case "4":
                        if (isAdmin)
                        {
                            // Only admin can open doors
                            _service.OpenDoors(ReadElevatorId());
                        }
                        else
                        {
                            // Users can get elevator status
                            GetElevatorStatus();
                        }
                        break;
                    case "5":
                        if (isAdmin)
                        {
                            // Only admin can close doors
                            _service.CloseDoors(ReadElevatorId());
                        }
                        else
                        {
                            // Users can check passenger count
                            int id = ReadElevatorId();
                            int count = _service.GetPassengerCount(id);
                            _logger.LogInfo($"Elevator {id} has {count} passenger(s).");
                        }
                        break;
                    case "6":
                        if (isAdmin)
                        {
                            SetElevatorStatus();
                        }
                        else
                        {
                            // Users cannot set status, they can only view status
                            GetElevatorStatus();
                        }
                        break;
                    case "7":
                        // Only admins can schedule maintenance
                        if (isAdmin)
                        {
                            _service.ScheduleMaintenance(ReadElevatorId());
                        }
                        else
                        {
                            _logger.LogWarning("You do not have permission to perform this action.");
                        }
                        break;
                    case "8":
                        // Only admins can use this option for status and passenger count
                        if (isAdmin)
                        {
                            int id = ReadElevatorId();
                            int count = _service.GetPassengerCount(id);
                            _logger.LogInfo($"Elevator {id} has {count} passenger(s).");
                        }
                        else
                        {
                            _logger.LogWarning("Admins: use status option 6 or 7.");
                        }
                        break;
                    default:
                        _logger.LogWarning("Invalid choice, please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error handling menu choice '{choice}': {ex.Message}");
            }
        }


        protected int ReadElevatorId() => ReadInt("Enter Elevator ID: ");

        protected int ReadInt(string prompt)
        {
            Console.Write(prompt);
            if (!int.TryParse(Console.ReadLine(), out int value))
            {
                _logger.LogError("Invalid input. Expected a number.");
                throw new ArgumentException("Invalid input.");
            }
            return value;
        }

        protected void SetElevatorStatus()
        {
            int id = ReadElevatorId();
            Console.Write("Enter new status (Available, InTransit, UnderMaintenance): ");
            string input = Console.ReadLine();

            if (Enum.TryParse(input, out ElevatorStatus status))
            {
                _service.SetElevatorStatus(id, status);
                _logger.LogInfo($"Status for Elevator {id} set to {status}.");
            }
            else
            {
                _logger.LogWarning("Invalid status.");
            }
        }

        protected void GetElevatorStatus()
        {
            int id = ReadElevatorId();
            var elevator = _service.GetElevator(id); // Get elevator details from service
            if (elevator != null)
            {
                int count = _service.GetPassengerCount(id);

                // Determine availability based on passenger count
                string availability = count >= 10 ? $"{ElevatorStatus.NotAvailable} (Full Capacity)" : elevator.Status.ToString();

                // Determine direction only if in transit
                string direction = "Idle";
                if (elevator.Status == ElevatorStatus.InTransit)
                {
                    if (elevator.CurrentFloor < elevator.TargetFloor)
                        direction = "Moving Up";
                    else if (elevator.CurrentFloor > elevator.TargetFloor)
                        direction = "Moving Down";
                }

                _logger.LogInfo($"Elevator {id} status: {availability}, Floor: {elevator.CurrentFloor}, {direction}");
                _logger.LogInfo($"Elevator {id} has {count} passenger(s).");
            }
            else
            {
                _logger.LogWarning("Elevator not found.");
            }
        }


    }
}
