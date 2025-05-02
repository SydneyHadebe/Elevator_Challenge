using ElevatorChallenge.Enums;
using ElevatorChallenge.Logging;
using ElevatorChallenge.Models;
using ElevatorChallenge.Service;
using NUnit.Framework;
using System.Collections.Generic;

namespace ElevatorChallenge.Tests
{
    [TestFixture]
    public class PassengerServiceTests
    {
        private PassengerService _passengerService;
        private TestLogger _testLogger;
        private MockDoorService _mockDoorService;

        [SetUp]
        public void Setup()
        {
            _testLogger = new TestLogger();
            _mockDoorService = new MockDoorService();
            _passengerService = new PassengerService(_testLogger, _mockDoorService);
        }

        [Test]
        public void AddPassenger_ShouldIncreasePassengerCount_WhenNotAtMaxCapacity()
        {
            // Arrange
            var elevator = new Elevator(1, maxCapacity: 5) { PassengerCount = 3 };

            // Act
            _passengerService.AddPassenger(elevator);

            // Assert
            Assert.AreEqual(4, elevator.PassengerCount);
            Assert.Contains("Passenger added. Total: 4", _testLogger.Messages);
        }

        [Test]
        public void AddPassenger_ShouldLogWarningAndOpenDoors_WhenAtMaxCapacity()
        {
            // Arrange
            var elevator = new Elevator(1, maxCapacity: 5) { PassengerCount = 5 };

            // Act
            _passengerService.AddPassenger(elevator);

            // Assert
            Assert.AreEqual(5, elevator.PassengerCount);
            Assert.IsTrue(elevator.DoorsOpen);
            Assert.Contains("Elevator 1 is overloaded.", _testLogger.Messages);
        }

        [Test]
        public void AddPassenger_ShouldDoNothing_WhenElevatorIsUnderMaintenance()
        {
            // Arrange
            var elevator = new Elevator(1, maxCapacity: 5) { PassengerCount = 3, Status = ElevatorStatus.UnderMaintenance };

            // Act
            _passengerService.AddPassenger(elevator);

            // Assert
            Assert.AreEqual(3, elevator.PassengerCount);
            Assert.IsEmpty(_testLogger.Messages);
        }

        [Test]
        public void RemovePassenger_ShouldDecreasePassengerCount_WhenPassengerCountIsGreaterThanZero()
        {
            // Arrange
            var elevator = new Elevator(1, maxCapacity: 5) { PassengerCount = 3 };

            // Act
            _passengerService.RemovePassenger(elevator);

            // Assert
            Assert.AreEqual(2, elevator.PassengerCount);
            Assert.Contains("Passenger removed. Total: 2", _testLogger.Messages);
        }

        [Test]
        public void RemovePassenger_ShouldCloseDoors_WhenPassengerCountIsAtOrBelowMaxCapacity()
        {
            // Arrange
            var elevator = new Elevator(1, maxCapacity: 5) { PassengerCount = 5 };

            // Act
            _passengerService.RemovePassenger(elevator);

            // Assert
            Assert.AreEqual(4, elevator.PassengerCount);
            Assert.IsTrue(_mockDoorService.DoorsClosed);
            Assert.Contains("Passenger removed. Total: 4", _testLogger.Messages);
        }

        [Test]
        public void RemovePassenger_ShouldDoNothing_WhenPassengerCountIsZero()
        {
            // Arrange
            var elevator = new Elevator(1, maxCapacity: 5) { PassengerCount = 0 };

            // Act
            _passengerService.RemovePassenger(elevator);

            // Assert
            Assert.AreEqual(0, elevator.PassengerCount);
            Assert.IsEmpty(_testLogger.Messages);
        }

        private class MockDoorService : IDoorService
        {
            public bool DoorsClosed { get; private set; }

            public void OpenDoors(Elevator elevator) { }

            public void CloseDoors(Elevator elevator)
            {
                DoorsClosed = true;
            }
        }
    }
}
