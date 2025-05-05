using System.Collections.ObjectModel;
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
        public ObservableCollection<UserModel> Users { get; } = new();

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

        private async void LoadUsers()
        {
            var users = await _dbService.GetAllUsers();
            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }

        /// <summary>
        /// Deletes a user from the database.
        /// </summary>
        private async void OnDeleteUserClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is UserModel user)
            {
                await DeleteUser(user);
            }
        }

        private async Task DeleteUser(UserModel user)
        {
            if (user == null)
                return;

            bool confirm = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete {user.FirstName} {user.LastName}?", "Yes", "No");
            if (confirm)
            {
                await _dbService.DeleteUser(user); // Assuming LocalDBService has a DeleteUserAsync method
                await DisplayAlert("Success", "User deleted successfully.", "OK");

            }

            LoadUsers(); // Refresh the user list after deletion
        }

        private async void OnNavigateToSensorPageClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SensorPage());
        }
    }
}