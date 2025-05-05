using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SECWRework.Model;
using SECWRework.Services;

namespace SECWRework.ViewModels
{
    /// <summary>
    /// ViewModel for monitoring sensor data.
    /// </summary>
    public partial class SensorViewModel : ObservableObject
    {
        private readonly LocalDBService _dbService;

        /// <summary>
        /// Collection of sensors displayed in the UI.
        /// </summary>
        public ObservableCollection<SensorModel> Sensors { get; } = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="SensorViewModel"/> class.
        /// </summary>
        /// <param name="dbService">The database service for sensor operations.</param>
        public SensorViewModel(LocalDBService dbService)
        {
            _dbService = dbService;
            LoadSensors();
        }

        /// <summary>
        /// Loads the list of sensors from the database.
        /// </summary>
        private async void LoadSensors()
        {
     
            var sensors = await _dbService.GetAllSensors();
            Sensors.Clear();
            foreach (var sensor in sensors)
            {
                Sensors.Add(sensor);
            }
        }

        
    }
}