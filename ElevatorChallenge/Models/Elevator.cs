using ElevatorChallenge.Enums;

namespace ElevatorChallenge.Models
{
    public class Elevator
    {
        public int Id { get; set; }
        public int CurrentFloor { get; set; }
        public int TargetFloor { get; set; } 
        public Queue<int> DestinationFloors { get; set; }
        public bool IsMoving { get; set; }
        public bool DoorsOpen { get; set; }
        public int PassengerCount { get; set; }
        public int MaxCapacity { get; set; }
        public bool IsInService { get; set; }
        public DateTime LastMaintenanceTime { get; set; }
        public string StatusMessage { get; set; }
        public bool EmergencyStopActivated { get; set; }

        public Direction Direction { get; set; } // Direction Enum
        public ElevatorCategory Type { get; set; } // ElevatorCategory Enum
        public ElevatorStatus Status { get; set; } // ElevatorStatus Enum


        public Elevator(int id, int maxCapacity = 10, ElevatorCategory type = ElevatorCategory.Passenger)
        {
            Id = id;
            MaxCapacity = maxCapacity;
            Type = type;
            CurrentFloor = 1;  // Default starting floor
            IsMoving = false;
            DoorsOpen = false;
            PassengerCount = 0;
            IsInService = true; 
            LastMaintenanceTime = DateTime.Now;  
            StatusMessage = string.Empty; 
            EmergencyStopActivated = false;  
            DestinationFloors = new Queue<int>();  
            Direction = Direction.Stopped;  
            Status = ElevatorStatus.Available; 
        }
    }
}
