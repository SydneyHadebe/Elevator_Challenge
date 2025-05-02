namespace ElevatorChallenge.Enums
{
    public enum Direction
    {
        Up,
        Down,
        Stopped
    }

    public enum ElevatorCategory
    {
        Passenger,
        HighSpeed,
        Glass,
        Freight
    }

    public enum ElevatorStatus
    {
        Available,
        NotAvailable,
        InTransit,
        UnderMaintenance,
        EmergencyStopped
    }
}
