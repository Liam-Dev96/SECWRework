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

        private string firstName = string.Empty;

        public string FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }

        private string lastName = string.Empty;

        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value);
        }

        private string email = string.Empty;

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        private string phoneNumber = string.Empty;

        public string PhoneNumber
        {
            get => phoneNumber;
            set => SetProperty(ref phoneNumber, value);
        }

        private string selectedRole = string.Empty;

        public string SelectedRole
        {
            get => selectedRole;
            set => SetProperty(ref selectedRole, value);
        }

        private UserModel? selectedUser;

        public UserModel? SelectedUser
        {
            get => selectedUser;
            set => SetProperty(ref selectedUser, value);
        }

        public ObservableCollection<UserModel> Users { get; } = [];

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
        public async void SaveAsync()
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
        public async void DeleteUserAsync(UserModel user)
        {
            await _dbService.DeleteUser(user);
            LoadUsers();
        }

        [RelayCommand]
        public void EditUser(UserModel user)
        {
            if (user != null)
            {
                _editUserId = user.Id; // Set the ID for updating
                FirstName = user.F_Name ?? string.Empty; // Load the user's first name into the form
                LastName = user.L_Name ?? string.Empty; // Load the user's last name into the form
                Email = user.Email ?? string.Empty; // Load the user's email into the form
                PhoneNumber = user.Phone_number ?? string.Empty; // Load the user's phone number into the form
                SelectedRole = user.Role ?? string.Empty; // Load the user's role into the form
            }
        }

        [RelayCommand]
        public async void BackupAsync()
        {
            string backupPath = Path.Combine(FileSystem.AppDataDirectory, "Backup.db");
            await _backupService.BackupDatabaseAsync(backupPath);
            Console.WriteLine($"Database backed up to {backupPath}");
        }

        [RelayCommand]
        public async void RestoreAsync()
        {
            string backupPath = Path.Combine(FileSystem.AppDataDirectory, "Backup.db");
            await _backupService.RestoreDatabaseAsync(backupPath);
            LoadUsers();
            Console.WriteLine($"Database restored from {backupPath}");
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