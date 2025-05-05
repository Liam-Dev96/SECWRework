using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SECWRework.Model;
using SECWRework.Services;

namespace SECWRework.ViewModels
{
    /// <summary>
    /// ViewModel for the main page, handling user data and operations.
    /// </summary>
    public partial class MainViewModel : ObservableObject
    {
        private readonly LocalDBService _dbService;
        private readonly BackupService _backupService;

        private string firstName = string.Empty;

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        public string FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }

        private string lastName = string.Empty;

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value);
        }

        private string email = string.Empty;

        /// <summary>
        /// Gets or sets the email of the user.
        /// </summary>
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        private string phoneNumber = string.Empty;

        /// <summary>
        /// Gets or sets the phone number of the user.
        /// </summary>
        public string PhoneNumber
        {
            get => phoneNumber;
            set => SetProperty(ref phoneNumber, value);
        }

        private string selectedRole = string.Empty;

        /// <summary>
        /// Gets or sets the selected role of the user.
        /// </summary>
        public string SelectedRole
        {
            get => selectedRole;
            set => SetProperty(ref selectedRole, value);
        }

        private UserModel? selectedUser;

        /// <summary>
        /// Gets or sets the currently selected user.
        /// </summary>
        public UserModel? SelectedUser
        {
            get => selectedUser;
            set => SetProperty(ref selectedUser, value);
        }

        /// <summary>
        /// Collection of users displayed in the UI.
        /// </summary>
        public ObservableCollection<UserModel> Users { get; } = new();

        private int _editUserId;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="dbService">The database service for user operations.</param>
        /// <param name="backupService">The backup service for database operations.</param>
        public MainViewModel(LocalDBService dbService, BackupService backupService)
        {
            _dbService = dbService;
            _backupService = backupService;
            LoadUsers();
        }

        /// <summary>
        /// Loads the list of users from the database.
        /// </summary>
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
        /// Saves a new or updated user to the database.
        /// </summary>
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

        /// <summary>
        /// Deletes a user from the database.
        /// </summary>
        /// <param name="user">The user to delete.</param>
        [RelayCommand]
        public async void DeleteUserAsync(UserModel user)
        {
            await _dbService.DeleteUser(user);
            LoadUsers();
        }

        /// <summary>
        /// Prepares a user for editing by loading their data into the form.
        /// </summary>
        /// <param name="user">The user to edit.</param>
        [RelayCommand]
        public void EditUser(UserModel user)
        {
            if (user != null)
            {
                _editUserId = user.Id;
                FirstName = user.F_Name ?? string.Empty;
                LastName = user.L_Name ?? string.Empty;
                Email = user.Email ?? string.Empty;
                PhoneNumber = user.Phone_number ?? string.Empty;
                SelectedRole = user.Role ?? string.Empty;
            }
        }

        /// <summary>
        /// Backs up the database to a specified location.
        /// </summary>
        [RelayCommand]
        public async void BackupAsync()
        {
            string backupPath = Path.Combine(FileSystem.AppDataDirectory, "Backup.db");
            await _backupService.BackupDatabaseAsync(backupPath);
            Console.WriteLine($"Database backed up to {backupPath}");
        }

        /// <summary>
        /// Restores the database from a backup.
        /// </summary>
        [RelayCommand]
        public async void RestoreAsync()
        {
            string backupPath = Path.Combine(FileSystem.AppDataDirectory, "Backup.db");
            await _backupService.RestoreDatabaseAsync(backupPath);
            LoadUsers();
            Console.WriteLine($"Database restored from {backupPath}");
        }

        /// <summary>
        /// Clears the form fields.
        /// </summary>
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