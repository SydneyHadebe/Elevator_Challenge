using ElevatorChallenge.Enums;
using ElevatorChallenge.Logging;
using ElevatorChallenge.Models;
using ElevatorChallenge.Service;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ElevatorChallenge.Tests
{
    [TestFixture]
    public class ElevatorServiceTests
    {
        private ElevatorService _elevatorService;
        private TestLogger _testLogger;
        private Mock<IMoveElevatorService> _mockMoveElevatorService;
        private Mock<IPassengerService> _mockPassengerService;
        private Mock<IDoorService> _mockDoorService;
        private Mock<IStatusService> _mockStatusService;

        [SetUp]
        public void SetUp()
        {
            var elevators = new List<Elevator>
            {
                new Elevator(1, 5) { PassengerCount = 3, CurrentFloor = 1 },
                new Elevator(2, 5) { PassengerCount = 6, CurrentFloor = 2 } 
            };

            _testLogger = new TestLogger();
            _mockMoveElevatorService = new Mock<IMoveElevatorService>();
            _mockPassengerService = new Mock<IPassengerService>();
            _mockDoorService = new Mock<IDoorService>();
            _mockStatusService = new Mock<IStatusService>();

            _elevatorService = new ElevatorService(
                elevators,
                _testLogger,
                _mockMoveElevatorService.Object,
                _mockPassengerService.Object,
                _mockDoorService.Object,
                _mockStatusService.Object
            );
        }

        [Test]
        public void Constructor_ShouldLogWarningForOverCapacityElevators()
        {
            Assert.That(_testLogger.Messages, Does.Contain("Elevator 2 is over capacity and cannot move."));
        }

        [Test]
        public void GetElevator_ShouldReturnElevator_WhenElevatorExists()
        {
            var elevator = _elevatorService.GetElevator(1);
            Assert.IsNotNull(elevator);
            Assert.AreEqual(1, elevator.Id);
        }

        [Test]
        public void GetElevator_ShouldLogError_WhenElevatorDoesNotExist()
        {
            var elevator = _elevatorService.GetElevator(99);
            Assert.IsNull(elevator);
            Assert.That(_testLogger.Messages, Does.Contain("Elevator 99 not found."));
        }

        [Test]
        public void MoveElevator_ShouldCallMoveElevatorService_WhenElevatorExists()
        {
            _elevatorService.MoveElevator(1, 5);
            _mockMoveElevatorService.Verify(m => m.MoveElevator(It.Is<Elevator>(e => e.Id == 1), 5), Times.Once);
        }

        [Test]
        public void AddPassenger_ShouldCallPassengerService_WhenElevatorExists()
        {
            _elevatorService.AddPassenger(1);
            _mockPassengerService.Verify(p => p.AddPassenger(It.Is<Elevator>(e => e.Id == 1)), Times.Once);
        }

        [Test]
        public void RemovePassenger_ShouldCallPassengerService_WhenElevatorExists()
        {
            _elevatorService.RemovePassenger(1);
            _mockPassengerService.Verify(p => p.RemovePassenger(It.Is<Elevator>(e => e.Id == 1)), Times.Once);
        }

        [Test]
        public void OpenDoors_ShouldCallDoorService_WhenElevatorExists()
        {
            _elevatorService.OpenDoors(1);
            _mockDoorService.Verify(d => d.OpenDoors(It.Is<Elevator>(e => e.Id == 1)), Times.Once);
        }

        [Test]
        public void CloseDoors_ShouldCallDoorService_WhenElevatorExists()
        {
            _elevatorService.CloseDoors(1);
            _mockDoorService.Verify(d => d.CloseDoors(It.Is<Elevator>(e => e.Id == 1)), Times.Once);
        }

        [Test]
        public void SetElevatorStatus_ShouldCallStatusService_WhenElevatorExists()
        {
            _elevatorService.SetElevatorStatus(1, ElevatorStatus.InTransit);
            _mockStatusService.Verify(s => s.SetStatus(It.Is<Elevator>(e => e.Id == 1), ElevatorStatus.InTransit), Times.Once);
        }

        [Test]
        public void ScheduleMaintenance_ShouldSetElevatorStatusToUnderMaintenance()
        {
            _elevatorService.ScheduleMaintenance(1);
            _mockStatusService.Verify(s => s.SetStatus(It.Is<Elevator>(e => e.Id == 1), ElevatorStatus.UnderMaintenance), Times.Once);
        }

        [Test]
        public void GetPassengerCount_ShouldReturnPassengerCount_WhenElevatorExists()
        {
            var count = _elevatorService.GetPassengerCount(1);
            Assert.AreEqual(3, count);
        }

        [Test]
        public void GetPassengerCount_ShouldReturnNegativeOne_WhenElevatorDoesNotExist()
        {
            var count = _elevatorService.GetPassengerCount(99);
            Assert.AreEqual(-1, count);
        }

        [Test]
        public void ReportAllElevatorStatuses_ShouldLogStatusesForAllElevators()
        {
            _mockStatusService.Setup(s => s.GetFormattedStatus(It.IsAny<Elevator>()))
                .Returns<Elevator>(e => $"Elevator {e.Id} status: {e.Status}");

            _elevatorService.ReportAllElevatorStatuses();

            Assert.That(_testLogger.Messages, Does.Contain("Elevator 1 status: Available"));
            Assert.That(_testLogger.Messages, Does.Contain("Elevator 2 status: Available"));
        }
    }
}
