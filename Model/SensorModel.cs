using SQLite;

namespace SECWRework.Model
{
    /// <summary>
    /// Represents a sensor and its operational data.
    /// </summary>
    [Table("Sensor")]
    public class SensorModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the sensor.
        /// </summary>
        [PrimaryKey]
        [AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the category of the sensor (e.g., Air quality, Water quality, Weather).
        /// </summary>
        [Column("Category")]
        public string? Category { get; set; }

        /// <summary>
        /// Gets or sets the quantity being measured (e.g., Nitrogen dioxide, Humidity).
        /// </summary>
        [Column("Quantity")]
        public string? Quantity { get; set; }

        /// <summary>
        /// Gets or sets the symbol for the measured quantity (e.g., NO2, H).
        /// </summary>
        [Column("Symbol")]
        public string? Symbol { get; set; }

        /// <summary>
        /// Gets or sets the unit of measurement (e.g., ug/m3, %).
        /// </summary>
        [Column("Unit")]
        public string? Unit { get; set; }

        /// <summary>
        /// Gets or sets the measurement frequency (e.g., Hourly, Daily).
        /// </summary>
        [Column("MeasurementFrequency")]
        public string? MeasurementFrequency { get; set; }

        /// <summary>
        /// Gets or sets the safe level for the measured quantity.
        /// </summary>
        [Column("SafeLevel")]
        public string? SafeLevel { get; set; }

        /// <summary>
        /// Gets or sets the operational status of the sensor (e.g., Online, Offline).
        /// </summary>
        [Column("Status")]
        public string? Status { get; set; }

        /// <summary>
        /// Gets or sets the location of the sensor.
        /// </summary>
        [Column("Location")]
        public string? Location { get; set; }
    }
}