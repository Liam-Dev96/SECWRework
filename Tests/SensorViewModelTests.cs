using Moq;
using SECWRework.Model;
using SECWRework.ViewModels;
using System;
using System.Collections.Generic;
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

        [Fact]
        public void GenerateReport_ShouldReturnReportWithSensorData()
        {
            // Arrange
            var sensors = new List<SensorModel>
            {
                new SensorModel
                {
                    Id = 1,
                    Quantity = "Nitrogen Dioxide",
                    Unit = "ug/m3",
                    Status = "Online",
                    HistoricalMeasurements = new List<double> { 10.5, 12.3, 9.8 },
                    Timestamps = new List<DateTime>
                    {
                        new DateTime(2025, 5, 1, 10, 0, 0),
                        new DateTime(2025, 5, 2, 10, 0, 0),
                        new DateTime(2025, 5, 3, 10, 0, 0)
                    }
                },
                new SensorModel
                {
                    Id = 2,
                    Quantity = "Humidity",
                    Unit = "%",
                    Status = "Offline",
                    HistoricalMeasurements = new List<double> { 45.2, 47.8 },
                    Timestamps = new List<DateTime>
                    {
                        new DateTime(2025, 5, 1, 12, 0, 0),
                        new DateTime(2025, 5, 2, 12, 0, 0)
                    }
                }
            };

            var viewModel = new SensorViewModel(null); // Pass null for the service in this test
            foreach (var sensor in sensors)
            {
                viewModel.Sensors.Add(sensor);
            }

            // Act
            var report = viewModel.GenerateReport();

            // Assert
            Assert.Contains("Nitrogen Dioxide", report);
            Assert.Contains("10.5", report);
            Assert.Contains("2025-05-01", report);
            Assert.Contains("Humidity", report);
            Assert.Contains("45.2", report);
            Assert.Contains("Offline", report);
        }

        [Fact]
        public void GenerateReport_ShouldHandleEmptySensorList()
        {
            // Arrange
            var viewModel = new SensorViewModel(null); // Pass null for the service in this test

            // Act
            var report = viewModel.GenerateReport();

            // Assert
            Assert.Equal("Environmental Trends Report\n\n", report);
        }

        [Fact]
        public void GenerateReport_ShouldHandleSensorsWithoutHistoricalData()
        {
            // Arrange
            var sensors = new List<SensorModel>
            {
                new SensorModel
                {
                    Id = 1,
                    Quantity = "Temperature",
                    Unit = "°C",
                    Status = "Online",
                    HistoricalMeasurements = new List<double>(),
                    Timestamps = new List<DateTime>()
                }
            };

            var viewModel = new SensorViewModel(null); // Pass null for the service in this test
            foreach (var sensor in sensors)
            {
                viewModel.Sensors.Add(sensor);
            }

            // Act
            var report = viewModel.GenerateReport();

            // Assert
            Assert.Contains("Temperature", report);
            Assert.Contains("No historical data available", report);
        }
    }
}