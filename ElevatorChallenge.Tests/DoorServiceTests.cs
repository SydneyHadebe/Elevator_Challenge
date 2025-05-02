using NUnit.Framework;
using ElevatorChallenge.Service;
using ElevatorChallenge.Models;
using System.Collections.Generic;

namespace ElevatorChallenge.Tests
{
    [TestFixture]
    public class DoorServiceTests
    {
        private DoorService _doorService;
        private TestLogger _testLogger;

        [SetUp]
        public void Setup()
        {
            _testLogger = new TestLogger();
            _doorService = new DoorService(_testLogger);
        }

        [Test]
        public void OpenDoors_ShouldSetDoorsOpenToTrue_AndLogInfo()
        {
            // Arrange
            var elevator = new Elevator(1);

            // Act
            _doorService.OpenDoors(elevator);

            // Assert
            Assert.IsTrue(elevator.DoorsOpen);
            Assert.Contains("Opening doors for Elevator 1.", _testLogger.Messages);
        }

        [Test]
        public void CloseDoors_ShouldSetDoorsOpenToFalse_AndLogInfo_WhenNotOverloaded()
        {
            // Arrange
            var elevator = new Elevator(1) { PassengerCount = 5, MaxCapacity = 10 };

            // Act
            _doorService.CloseDoors(elevator);

            // Assert
            Assert.IsFalse(elevator.DoorsOpen);
            Assert.Contains("Closing doors for Elevator 1.", _testLogger.Messages);
        }

        [Test]
        public void CloseDoors_ShouldKeepDoorsOpen_AndLogWarning_WhenOverloaded()
        {
            // Arrange
            var elevator = new Elevator(1) { PassengerCount = 15, MaxCapacity = 10 };

            // Act
            _doorService.CloseDoors(elevator);

            // Assert
            Assert.IsTrue(elevator.DoorsOpen);
            Assert.Contains("Cannot close doors. Elevator 1 is overloaded.", _testLogger.Messages);
        }
    }
}
