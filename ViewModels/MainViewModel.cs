using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SECWRework.Model;
using SECWRework.Services;

namespace SECWRework.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly LocalDBService _dbService;
        private readonly BackupService _backupService;

        [ObservableProperty]
        private string firstName = string.Empty;

        [ObservableProperty]
        private string lastName = string.Empty;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string phoneNumber = string.Empty;

        [ObservableProperty]
        private string selectedRole = string.Empty;

        [ObservableProperty]
        private UserModel selectedUser = new UserModel();

        public ObservableCollection<UserModel> Users { get; } = new();

        private int _editUserId;

        public MainViewModel(LocalDBService dbService, BackupService backupService)
        {
            _dbService = dbService;
            _backupService = backupService;
            LoadUsers();
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

        [RelayCommand]
        public async Task SaveAsync()
        {
            if (_editUserId == 0)
            {
                await _dbService.AddUser(new UserModel
                {
                    F_Name = FirstName,
                    L_Name = LastName,
                    Email = Email,
                    Phone_number = PhoneNumber,
                    Role = SelectedRole
                });
            }
            else
            {
                await _dbService.UpdateUser(new UserModel
                {
                    Id = _editUserId,
                    F_Name = FirstName,
                    L_Name = LastName,
                    Email = Email,
                    Phone_number = PhoneNumber,
                    Role = SelectedRole
                });
                _editUserId = 0;
            }

            ClearForm();
            LoadUsers();
        }

        [RelayCommand]
        public async Task DeleteUserAsync(UserModel user)
        {
            await _dbService.DeleteUser(user);
            LoadUsers();
        }

        [RelayCommand]
        public void EditUser(UserModel user)
        {
            FirstName = user.F_Name ?? string.Empty;
            LastName = user.L_Name ?? string.Empty;
            Email = user.Email ?? string.Empty;
            PhoneNumber = user.Phone_number ?? string.Empty;
            SelectedRole = user.Role ?? string.Empty;
            _editUserId = user.Id;
        }

        [RelayCommand]
        public async Task BackupAsync()
        {
            string backupPath = Path.Combine(FileSystem.AppDataDirectory, "Backup.db");
            await _backupService.BackupDatabaseAsync(backupPath);
            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Database backup completed.", "OK");
            }
        }

        [RelayCommand]
        public async Task RestoreAsync()
        {
            string backupPath = Path.Combine(FileSystem.AppDataDirectory, "Backup.db");
            await _backupService.RestoreDatabaseAsync(backupPath);
            if (App.Current?.MainPage != null)
            {
                await App.Current.MainPage.DisplayAlert("Success", "Database restored successfully.", "OK");
            }
            LoadUsers();
        }

        private void ClearForm()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            SelectedRole = string.Empty;
        }
    }
}