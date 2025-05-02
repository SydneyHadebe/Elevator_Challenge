# Elevator System Simulation

This C# console application simulates the operation of multiple elevators in a large building, aimed at optimizing passenger transportation. The application uses **Object-Oriented Programming (OOP)** principles to ensure modularity, maintainability, and extensibility. It includes enhanced features such as logging, elevator movement, passenger management, door operations, and status reporting.

---

## Key Features

### 1. **Real-Time Elevator Status**
- Displays live information about each elevator:
  - **Current Floor**: The elevator's current position.
  - **Direction**: Whether the elevator is moving up, down, or stationary.
  - **Motion Status**: Indicates if the elevator is moving or idle.
  - **Passenger Count**: The number of passengers inside the elevator.
  - **Doors Status**: Indicates whether the elevator's doors are open or closed.
  - **Maintenance Status**: If the elevator is under maintenance, it is not operational.

### 2. **Interactive Elevator Control**
- **Call an Elevator**: Users can request an elevator to a specific floor.
- **Set Passenger Count**: Users can specify the number of passengers waiting on each floor.
- **Manage Passengers**: Add or remove passengers from an elevator.
- **Open/Close Doors**: Control the elevator doors based on user actions.

### 3. **Logging and Error Handling**
- The system includes comprehensive logging of events such as:
  - **Warning**: When an elevator is over its passenger capacity.
  - **Error**: If an elevator is not found during operations.
  - **Info**: Logs the status of elevators for monitoring.

### 4. **Support for Multiple Floors and Elevators**
- The system supports buildings with multiple floors and elevators, efficiently handling requests and managing elevator movements.

### 5. **Efficient Elevator Dispatching**
- An algorithm efficiently directs the nearest available elevator to a request, minimizing wait times and optimizing elevator usage.

### 6. **Passenger Limit Handling**
- Each elevator has a defined maximum passenger capacity, and the system prevents the elevator from exceeding this capacity.

### 7. **Maintenance Scheduling**
- Elevators can be set under maintenance, and their operational status can be tracked.

### 8. **Consideration for Different Elevator Types**
- The system can accommodate various elevator types such as:
  - **High-Speed Elevators**
  - **Glass Elevators**
  - **Freight Elevators**

### 9. **Real-Time Operation**
- The system provides immediate responses to user input, reflecting real-time elevator movements, status updates, and door operations.

---

## System Design

### Class Structure

- **ElevatorService**: Manages the core functionality for controlling elevators.
  - **Properties**:
    - `_elevators`: A dictionary that stores all elevators by their ID.
    - `_logger`: Logs information, warnings, and errors.
    - `_moveElevatorService`: Handles movement of elevators.
    - `_passengerService`: Manages passengers boarding and exiting.
    - `_doorService`: Manages door operations (open/close).
    - `_statusService`: Updates and retrieves the status of each elevator.
  
  - **Methods**:
    - `MoveElevator(int elevatorId, int targetFloor)`: Moves the elevator to the target floor.
    - `AddPassenger(int elevatorId)`: Adds a passenger to the elevator.
    - `RemovePassenger(int elevatorId)`: Removes a passenger from the elevator.
    - `OpenDoors(int elevatorId)`: Opens the elevator doors.
    - `CloseDoors(int elevatorId)`: Closes the elevator doors.
    - `SetElevatorStatus(int elevatorId, ElevatorStatus status)`: Sets the operational status of the elevator.
    - `ScheduleMaintenance(int elevatorId)`: Schedules maintenance for an elevator.
    - `GetPassengerCount(int elevatorId)`: Retrieves the number of passengers in the elevator.
    - `ReportAllElevatorStatuses()`: Logs the current status of all elevators.

- **Logger**: Responsible for logging events such as warnings, errors, and status updates.
  
- **Elevator**: Represents a single elevator.
  - **Properties**: `Id`, `CurrentFloor`, `MaxCapacity`, `PassengerCount`, `DoorsOpen`, `Status`.

- **ElevatorStatus**: Enum that represents the operational state of the elevator (e.g., `Moving`, `Idle`, `UnderMaintenance`).

- **Services**:
  - `IMoveElevatorService`: Interface for handling elevator movement.
  - `IPassengerService`: Interface for managing passenger boarding and exiting.
  - `IDoorService`: Interface for handling door operations.
  - `IStatusService`: Interface for setting and getting the elevator's status.

---
