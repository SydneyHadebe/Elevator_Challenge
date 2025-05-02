using ElevatorChallenge.Enums;
using ElevatorChallenge.Logging;
using ElevatorChallenge.Models;
using ElevatorChallenge.Service;
using NUnit.Framework;
using System.Collections.Generic;

namespace ElevatorChallenge.Tests
{
    [TestFixture]
    public class MoveElevatorServiceTests
    {
        private TestLogger _testLogger;
        private MoveElevatorService _service;

        [SetUp]
        public void Setup()
        {
            _testLogger = new TestLogger();
            _service = new MoveElevatorService(_testLogger);
        }

        [Test]
        public void MoveElevator_ShouldLogWarning_WhenElevatorIsUnderMaintenance()
        {
            var elevator = new Elevator(1) { Status = ElevatorStatus.UnderMaintenance };

            _service.MoveElevator(elevator, 5);

            Assert.Contains("Elevator 1 is under maintenance and cannot be moved.", _testLogger.Messages);
        }

        [Test]
        public void MoveElevator_ShouldLogWarning_WhenElevatorIsOverCapacity()
        {
            var elevator = new Elevator(1) { PassengerCount = 11, MaxCapacity = 10 };

            _service.MoveElevator(elevator, 5);

            Assert.Contains("Elevator 1 is over capacity and cannot move.", _testLogger.Messages);
        }

        [Test]
        public void MoveElevator_ShouldLogError_WhenTargetFloorIsInvalid()
        {
            var elevator = new Elevator(1);

            _service.MoveElevator(elevator, -1);

            Assert.Contains("Invalid target floor: -1. Movement aborted.", _testLogger.Messages);
        }

        [Test]
        public void MoveElevator_ShouldLogInfo_WhenElevatorIsAlreadyAtTargetFloor()
        {
            var elevator = new Elevator(1) { CurrentFloor = 5 };

            _service.MoveElevator(elevator, 5);

            Assert.Contains("Elevator 1 is already at floor 5.", _testLogger.Messages);
        }

        [Test]
        public void MoveElevator_ShouldMoveElevator_WhenConditionsAreValid()
        {
            var elevator = new Elevator(1) { CurrentFloor = 1, MaxCapacity = 10, PassengerCount = 5 };

            _service.MoveElevator(elevator, 5);

            Assert.Contains("Moving Elevator 1 from floor 1 to 5 – Direction: Up", _testLogger.Messages);
            Assert.Contains("Elevator 1 arrived at floor 5. Status: Available.", _testLogger.Messages);
            Assert.AreEqual(5, elevator.CurrentFloor);
            Assert.AreEqual(ElevatorStatus.Available, elevator.Status);
            Assert.AreEqual(Direction.Stopped, elevator.Direction);
        }
    }
}
