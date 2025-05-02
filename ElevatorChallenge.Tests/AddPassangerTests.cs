using ElevatorChallenge.Enums;
using ElevatorChallenge.Models;
using ElevatorChallenge.Service;
using NUnit.Framework;

namespace ElevatorChallenge.Tests
{
    [TestFixture]
    public class AddPassengerTests
    {
        private TestLogger _logger;

        [SetUp]
        public void Setup()
        {
            _logger = new TestLogger();
        }

        [Test]
        public void AddPassenger_ShouldIncreasePassengerCount_WhenElevatorIsAvailableAndNotFull()
        {
            // Arrange
            var elevator = new Elevator(1, maxCapacity: 5)
            {
                PassengerCount = 3,
                Status = ElevatorStatus.Available
            };
            var service = new ElevatorService(new List<Elevator> { elevator }, _logger);

            // Act
            service.AddPassenger(1);

            // Assert
            Assert.That(elevator.PassengerCount, Is.EqualTo(4));
            Assert.That(_logger.Messages, Does.Contain("Passenger added. Total: 4"));
        }

        [Test]
        public void AddPassenger_ShouldNotIncreasePassengerCount_WhenElevatorIsFull()
        {
            // Arrange
            var elevator = new Elevator(1, maxCapacity: 5)
            {
                PassengerCount = 5,
                Status = ElevatorStatus.Available
            };
            var service = new ElevatorService(new List<Elevator> { elevator }, _logger);

            // Act
            service.AddPassenger(1);

            // Assert
            Assert.That(elevator.PassengerCount, Is.EqualTo(5));
            Assert.That(elevator.DoorsOpen, Is.True);
            Assert.That(_logger.Messages, Does.Contain("Elevator 1 is overloaded."));
        }

        [Test]
        public void AddPassenger_ShouldNotIncreasePassengerCount_WhenElevatorIsUnderMaintenance()
        {
            // Arrange
            var elevator = new Elevator(1, maxCapacity: 5)
            {
                PassengerCount = 3,
                Status = ElevatorStatus.UnderMaintenance
            };
            var service = new ElevatorService(new List<Elevator> { elevator }, _logger);

            // Act
            service.AddPassenger(1);

            // Assert
            Assert.That(elevator.PassengerCount, Is.EqualTo(3));
            Assert.That(_logger.Messages, Is.Empty);
        }

        [Test]
        public void AddPassenger_ShouldLogWarning_WhenElevatorDoesNotExist()
        {
            // Arrange
            var service = new ElevatorService(new List<Elevator>(), _logger);

            // Act & Assert
            Assert.DoesNotThrow(() => service.AddPassenger(99));
            Assert.That(_logger.Messages, Does.Contain("Elevator 99 not found."));
        }
    }
}
