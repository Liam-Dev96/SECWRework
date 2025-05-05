
using Moq;
using SECWRework.Model;
using SECWRework.ViewModels;
using Xunit;

namespace SECWRework.Tests
{
    /// <summary>
    /// Unit tests for the <see cref="SensorViewModel"/> class.
    /// </summary>
    public class SensorViewModelTests
    {
        /// <summary>
        /// Tests that the <see cref="SensorViewModel"/> loads sensors correctly from the database.
        /// </summary>
        [Fact]
        public async Task LoadSensors_ShouldPopulateSensorsCollection()
        {
            // Arrange
            var mockDbService = new Mock<LocalDBService>();
            var testSensors = new List<SensorModel>
            {
                new SensorModel { Id = 1, Quantity = "Nitrogen dioxide", Status = "Online" },
                new SensorModel { Id = 2, Quantity = "Sulphur dioxide", Status = "Offline" }
            };

            mockDbService.Setup(db => db.GetAllSensors()).ReturnsAsync(testSensors);

            var viewModel = new SensorViewModel(mockDbService.Object);

            // Act
            await Task.Delay(100); // Allow async LoadSensors to complete

            // Assert
            Assert.Equal(2, viewModel.Sensors.Count);
            Assert.Equal("Nitrogen dioxide", viewModel.Sensors[0].Quantity);
            Assert.Equal("Online", viewModel.Sensors[0].Status);
        }
    }
}