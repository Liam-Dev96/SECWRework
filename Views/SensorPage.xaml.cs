using SECWRework.Model;
using SECWRework.Services;
using SECWRework.ViewModels;

namespace SECWRework.Views
{
    /// <summary>
    /// Represents the main page of the application.
    /// </summary>
    public partial class SensorPage : ContentPage
    {
        private readonly LocalDBService _dbService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SensorPage"/> class.
        /// </summary>
        public SensorPage()
        {
            InitializeComponent();
            _dbService = new LocalDBService();
            BindingContext = new SensorViewModel(_dbService);
        }
    }
}