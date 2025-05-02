using ElevatorChallenge.Enums;
using ElevatorChallenge.Logging;
using ElevatorChallenge.Models;
using ElevatorChallenge.Service;
using NUnit.Framework;
using System.Collections.Generic;

namespace ElevatorChallenge.Tests
{
    [TestFixture]
    public class StatusServiceTests
    {
        private StatusService _statusService;
        private TestLogger _testLogger;

        [SetUp]
        public void Setup()
        {
            _testLogger = new TestLogger();
            _statusService = new StatusService(_testLogger);
        }

        [Test]
        public void SetStatus_ShouldUpdateElevatorStatus_AndLogMessage()
        {
            // Arrange  
            var elevator = new Elevator(1) { Status = ElevatorStatus.Available };
            var newStatus = ElevatorStatus.InTransit;

            // Act  
            _statusService.SetStatus(elevator, newStatus);

            // Assert  
            Assert.AreEqual(newStatus, elevator.Status);
            Assert.AreEqual(1, _testLogger.Messages.Count);
            Assert.AreEqual($"Elevator {elevator.Id} status set to {newStatus}.", _testLogger.Messages[0]);
        }

        [Test]
        public void GetFormattedStatus_ShouldReturnCorrectFormattedString()
        {
            // Arrange  
            var elevator = new Elevator(1)
            {
                CurrentFloor = 5,
                PassengerCount = 8,
                MaxCapacity = 10,
                Direction = Direction.Up,
                Status = ElevatorStatus.InTransit
            };

            // Act  
            var result = _statusService.GetFormattedStatus(elevator);

            // Assert  
            var expected = "Elevator 1: Floor 5, Status InTransit, Direction Moving Up, Passengers 8/10";
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetFormattedStatus_ShouldIncludeOverloadedMessage_WhenPassengerCountExceedsMaxCapacity()
        {
            // Arrange  
            var elevator = new Elevator(1)
            {
                CurrentFloor = 3,
                PassengerCount = 12,
                MaxCapacity = 10,
                Direction = Direction.Stopped,
                Status = ElevatorStatus.Available
            };

            // Act  
            var result = _statusService.GetFormattedStatus(elevator);

            // Assert  
            var expected = "Elevator 1: Floor 3, Status Available, Direction Stopped, Passengers 12/10 [OVERLOADED]";
            Assert.AreEqual(expected, result);
        }
    }
}
