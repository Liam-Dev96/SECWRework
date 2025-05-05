using SECWRework.Model;
using SECWRework.Services;
using SECWRework.ViewModels;

namespace SECWRework.Views
{
    /// <summary>
    /// Represents the main page of the application.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        private readonly LocalDBService _dbService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            _dbService = new LocalDBService();
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SoftwareEngineering.db");
            BindingContext = new MainViewModel(_dbService, new BackupService(dbPath));
        }
    }
}