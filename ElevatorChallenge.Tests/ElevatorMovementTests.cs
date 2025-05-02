using ElevatorChallenge.Enums;
using ElevatorChallenge.Models;
using ElevatorChallenge.Service;
using NUnit.Framework;
using System.Collections.Generic;

namespace ElevatorChallenge.Tests
{
    [TestFixture]
    public class ElevatorMovementTests
    {
        private TestLogger _logger;

        [SetUp]
        public void Setup()
        {
            _logger = new TestLogger();
        }

        [Test]
        public void MoveElevator_ShouldMoveToTargetFloor_WhenElevatorIsAvailable()
        {
            // Arrange
            var elevator = new Elevator(1)
            {
                CurrentFloor = 1,
                Status = ElevatorStatus.Available,
                PassengerCount = 0 // Ensure not over capacity
            };

            var service = new ElevatorService(new List<Elevator> { elevator }, _logger);
            int targetFloor = 5;

            // Act
            service.MoveElevator(elevator.Id, targetFloor);

            // Assert
            Assert.That(elevator.CurrentFloor, Is.EqualTo(targetFloor));
            Assert.That(elevator.Direction, Is.EqualTo(Direction.Stopped));
            Assert.That(elevator.Status, Is.EqualTo(ElevatorStatus.Available));

            // Match full updated log messages
            Assert.That(_logger.Messages, Does.Contain("Elevator 1 arrived at floor 5. Status: Available."));
            Assert.That(_logger.Messages, Does.Contain("Moving Elevator 1 from floor 1 to 5 – Direction: Up"));
        }


        [Test]
        public void MoveElevator_ShouldNotMove_WhenElevatorIsUnderMaintenance()
        {
            // Arrange
            int elevatorId = 1;
            int currentFloor = 1;
            int targetFloor = 5;

            var elevator = new Elevator(elevatorId)
            {
                CurrentFloor = currentFloor,
                Status = ElevatorStatus.UnderMaintenance
            };

            var service = new ElevatorService(new List<Elevator> { elevator }, _logger);

            // Act
            service.MoveElevator(elevator.Id, targetFloor);

            // Assert
            Assert.That(elevator.CurrentFloor, Is.EqualTo(currentFloor));
            Assert.That(elevator.Status, Is.EqualTo(ElevatorStatus.UnderMaintenance));
            Assert.That(_logger.Messages, Does.Contain($"Elevator {elevatorId} is under maintenance and cannot be moved."));
        }


        [Test]
        public void MoveElevator_ShouldSetDirectionCorrectly_WhenMovingUp()
        {
            var elevator = new Elevator(1) { CurrentFloor = 1, Status = ElevatorStatus.Available };
            var service = new ElevatorService(new List<Elevator> { elevator }, _logger);
            int targetFloor = 3;

            service.MoveElevator(elevator.Id, targetFloor);

            Assert.That(elevator.CurrentFloor, Is.EqualTo(targetFloor));
            Assert.That(elevator.Direction, Is.EqualTo(Direction.Stopped));
        }

        [Test]
        public void MoveElevator_ShouldSetDirectionCorrectly_WhenMovingDown()
        {
            var elevator = new Elevator(1) { CurrentFloor = 5, Status = ElevatorStatus.Available };
            var service = new ElevatorService(new List<Elevator> { elevator }, _logger);
            int targetFloor = 2;

            service.MoveElevator(elevator.Id, targetFloor);

            Assert.That(elevator.CurrentFloor, Is.EqualTo(targetFloor));
            Assert.That(elevator.Direction, Is.EqualTo(Direction.Stopped));
        }

        [Test]
        public void MoveElevator_ShouldNotMove_WhenAlreadyAtTargetFloor()
        {
            var elevator = new Elevator(1) { CurrentFloor = 3, Status = ElevatorStatus.Available };
            var service = new ElevatorService(new List<Elevator> { elevator }, _logger);
            int targetFloor = 3;

            service.MoveElevator(elevator.Id, targetFloor);

            Assert.That(elevator.CurrentFloor, Is.EqualTo(3));
            Assert.That(elevator.Direction, Is.EqualTo(Direction.Stopped));
            Assert.That(_logger.Messages, Does.Contain("Elevator 1 is already at floor 3."));
        }

        [Test]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
        {
            var elevator = new Elevator(1) { CurrentFloor = 1 };
            var ex = Assert.Throws<ArgumentNullException>(() => new ElevatorService(new List<Elevator> { elevator }, null));

            Assert.That(ex.ParamName, Is.EqualTo("logger"));
        }


        [Test]
        public void MoveElevator_ShouldNotMove_WhenElevatorIsOverCapacity()
        {
            // Arrange
            int elevatorId = 1;
            int currentFloor = 1;
            int maxCapacity = 10;
            int targetFloor = 4;
            int overCapacity = maxCapacity + 1;

            var elevator = new Elevator(elevatorId)
            {
                CurrentFloor = currentFloor,
                Status = ElevatorStatus.Available,
                MaxCapacity = maxCapacity,
                PassengerCount = maxCapacity // set within limit for constructor
            };

            var service = new ElevatorService(new List<Elevator> { elevator }, _logger);

            // Set elevator to over capacity after service init to avoid constructor adjusting it
            elevator.PassengerCount = overCapacity;

            // Act
            service.MoveElevator(elevatorId, targetFloor);

            // Assert
            Assert.That(_logger.Messages, Does.Contain($"Elevator {elevatorId} is over capacity and cannot move."));
            Assert.That(elevator.CurrentFloor, Is.EqualTo(currentFloor));
        }


        [Test]
        public void MoveElevator_ShouldLog_WhenElevatorIdIsInvalid()
        {
            var elevator = new Elevator(1) { CurrentFloor = 1, Status = ElevatorStatus.Available };
            var service = new ElevatorService(new List<Elevator> { elevator }, _logger);

            // Call MoveElevator with an invalid elevator ID (99)
            service.MoveElevator(99, 3);

            // Check that the logger contains the expected message
            Assert.That(_logger.Messages, Does.Contain("Elevator with ID 99 not found."));
        }



        [Test]
        public void MoveElevator_ShouldLogError_WhenTargetFloorIsNegative()
        {
            var elevator = new Elevator(1) { CurrentFloor = 1, Status = ElevatorStatus.Available };
            var service = new ElevatorService(new List<Elevator> { elevator }, _logger);

            service.MoveElevator(elevator.Id, -2);

            Assert.That(_logger.Messages, Does.Contain("Invalid target floor: -2. Movement aborted."));
            Assert.That(elevator.CurrentFloor, Is.EqualTo(1)); // Ensure it didn’t move
        }


    }
}
