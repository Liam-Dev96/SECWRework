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
        private readonly SensorViewModel? _viewModel;


        /// <summary>
        /// Initializes a new instance of the <see cref="SensorPage"/> class.
        /// </summary>
        public SensorPage()
        {
            InitializeComponent();
            _dbService = new LocalDBService();
            _viewModel = new SensorViewModel(_dbService);
            BindingContext = _viewModel;
        }

        private void OnGenerateReportClicked(object sender, EventArgs e)
        {
            if (_viewModel != null)
            {
                var report = _viewModel.GenerateReport();
                ReportLabel.Text = report;
            }
        }
    }
}