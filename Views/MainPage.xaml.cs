using SECWRework.Model;
using SECWRework.Services;
using SECWRework.ViewModels;

namespace SECWRework.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly LocalDBService _dbService ;
        public MainPage()
        {
            InitializeComponent();
            _dbService = new LocalDBService();
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SoftwareEngineering.db");
            BindingContext = new MainViewModel(_dbService, new BackupService(dbPath));
        }
    }
}