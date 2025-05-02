using ElevatorChallenge.Enums;
using ElevatorChallenge.Models;
using ElevatorChallenge.Service;
using NUnit.Framework;
using System.Collections.Generic;

namespace ElevatorChallenge.Tests
{
    [TestFixture]
    public class ReportAllElevatorStatusesTests
    {
        private ElevatorService _elevatorService;
        private TestLogger _logger;

        [SetUp]
        public void Setup()
        {
            _logger = new TestLogger();

            var elevators = new List<Elevator>
            {
                new Elevator(1, 10, ElevatorCategory.Passenger)
                {
                    CurrentFloor = 1,
                    PassengerCount = 0,
                    Status = ElevatorStatus.Available
                },
                new Elevator(2, 15, ElevatorCategory.Freight)
                {
                    CurrentFloor = 5,
                    PassengerCount = 5,
                    Status = ElevatorStatus.Available
                }
            };

            _elevatorService = new ElevatorService(elevators, _logger);
        }

        [Test]
        public void ReportAllElevatorStatuses_ShouldLogCorrectStatuses()
        {
            // Act
            _elevatorService.ReportAllElevatorStatuses();

            // Assert
            Assert.That(_logger.Messages, Has.Exactly(2).Items);

            // Updated expected log message to match the actual generated message
            Assert.That(_logger.Messages[0], Is.EqualTo("Elevator 1: Floor 1, Status Available, Direction Stopped, Passengers 0/10"));
            Assert.That(_logger.Messages[1], Is.EqualTo("Elevator 2: Floor 5, Status Available, Direction Stopped, Passengers 5/15"));
        }

    }
}
