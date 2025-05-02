using ElevatorChallenge.Enums;
using ElevatorChallenge.Models;
using ElevatorChallenge.Service;
using Moq;  // Mocking library
using NUnit.Framework;
using System.Collections.Generic;

namespace ElevatorChallenge.Tests
{
    [TestFixture]
    public class PassengerCountTests
    {
        private ElevatorService _elevatorService;
        private TestLogger _logger;
        private Mock<IMoveElevatorService> _mockMoveElevatorService;
        private Mock<IPassengerService> _mockPassengerService;
        private Mock<IDoorService> _mockDoorService;
        private Mock<IStatusService> _mockStatusService;

        [SetUp]
        public void Setup()
        {
            _logger = new TestLogger();

            // Mock the dependencies
            _mockMoveElevatorService = new Mock<IMoveElevatorService>();
            _mockPassengerService = new Mock<IPassengerService>();
            _mockDoorService = new Mock<IDoorService>();
            _mockStatusService = new Mock<IStatusService>();

            // Set up the elevators
            var elevators = new List<Elevator>
            {
                new Elevator(1, 5) { PassengerCount = 3 },
                new Elevator(2, 8) { PassengerCount = 0 },
                new Elevator(3, 10) { PassengerCount = 7 }
            };

            // Initialize the service with mocked dependencies
            _elevatorService = new ElevatorService(elevators, _logger,
                _mockMoveElevatorService.Object, _mockPassengerService.Object,
                _mockDoorService.Object, _mockStatusService.Object);
        }

        [Test]
        public void GetPassengerCount_ValidElevatorId_ReturnsCorrectPassengerCount()
        {
            int elevatorId = 1;

            int passengerCount = _elevatorService.GetPassengerCount(elevatorId);

            Assert.That(passengerCount, Is.EqualTo(3));
        }

        [Test]
        public void GetPassengerCount_InvalidElevatorId_ReturnsMinusOne()
        {
            int elevatorId = 99;

            int passengerCount = _elevatorService.GetPassengerCount(elevatorId);

            Assert.That(passengerCount, Is.EqualTo(-1));
            Assert.That(_logger.Messages, Does.Contain("Elevator 99 not found."));
        }

        [Test]
        public void GetPassengerCount_ElevatorUnderMaintenance_ReturnsCorrectPassengerCount()
        {
            int elevatorId = 3;
            _elevatorService.SetElevatorStatus(elevatorId, ElevatorStatus.UnderMaintenance);

            int passengerCount = _elevatorService.GetPassengerCount(elevatorId);

            Assert.That(passengerCount, Is.EqualTo(7));
        }
    }
}
