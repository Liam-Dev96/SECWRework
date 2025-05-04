using System.Threading.Tasks;
using SECWRework.Model;
using SECWRework.Services;

namespace SECWRework.Views
{
    public partial class MainPage : ContentPage
    {

        private readonly LocalDBService _dbService;
        private readonly BackupService _backupService;
        private int _editUserId;

        public MainPage(LocalDBService dbService, BackupService backupService)
        {
            InitializeComponent();
            _dbService = dbService;
            _backupService = backupService;
            Task.Run(async () => listView.ItemsSource = await _dbService.GetAllUsers());
        }


        private async void saveButton_Clicked(object sender, EventArgs e)
        {
            if(_editUserId == 0)
            {
                await _dbService.AddUser(new UserModel
                {
                    F_Name = firstNameEntry.Text,
                    L_Name = lastNameEntry.Text,
                    Email = emailEntry.Text,
                    Phone_number = phoneEntry.Text,
                    Role = rolePicker.SelectedItem?.ToString()
                });
            }
            else
            {
                await _dbService.UpdateUser(new UserModel
                {
                    Id = _editUserId,
                    F_Name = firstNameEntry.Text,
                    L_Name = lastNameEntry.Text,
                    Email = emailEntry.Text,
                    Phone_number = phoneEntry.Text,
                    Role = rolePicker.SelectedItem?.ToString()
                });
                _editUserId = 0;
            }
            firstNameEntry.Text = string.Empty;
            lastNameEntry.Text = string.Empty;
            emailEntry.Text = string.Empty;
            phoneEntry.Text = string.Empty;
            rolePicker.SelectedItem = string.Empty;

            listView.ItemsSource = await _dbService.GetAllUsers();

        }

        private async void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var user = (UserModel)e.Item;
            var action = await DisplayActionSheet("Select Action", "Cancel", null, "Edit", "Delete");

            switch (action)
            {
                case "Edit":
                    firstNameEntry.Text = user.F_Name;
                    lastNameEntry.Text = user.L_Name;
                    emailEntry.Text = user.Email;
                    phoneEntry.Text = user.Phone_number;
                    rolePicker.SelectedItem = user.Role;
                    _editUserId = user.Id;
                    break;
                case "Delete":
                    await _dbService.DeleteUser(user);
                    listView.ItemsSource = await _dbService.GetAllUsers();
                    break;
            }
        }

        private async void BackupButton_Clicked(object sender, EventArgs e)
        {
            string backupPath = Path.Combine(FileSystem.AppDataDirectory, "Backup.db");
            await _backupService.BackupDatabaseAsync(backupPath);
            await DisplayAlert("Success", "Database backup completed.", "OK");
        }

        private async void RestoreButton_Clicked(object sender, EventArgs e)
        {
            string backupPath = Path.Combine(FileSystem.AppDataDirectory, "Backup.db");
            await _backupService.RestoreDatabaseAsync(backupPath);
            await DisplayAlert("Success", "Database restored successfully.", "OK");
            listView.ItemsSource = await _dbService.GetAllUsers();
        }

    }
}

