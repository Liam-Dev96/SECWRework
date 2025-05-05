using SECWRework.Model;
using SQLite;

namespace SECWRework
{
    /// <summary>
    /// Service for interacting with the local SQLite database.
    /// </summary>
    public class LocalDBService
    {
        /// <summary>
        /// The name of the database file.
        /// </summary>
        private const string DB_NAME = "SoftwareEngineering.db";

        /// <summary>
        /// The SQLite asynchronous connection instance.
        /// </summary>
        private readonly SQLiteAsyncConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalDBService"/> class.
        /// Creates the database connection and initializes the User table.
        /// </summary>
        public LocalDBService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            _connection.CreateTableAsync<UserModel>();
        }

        /// <summary>
        /// Retrieves all users from the database.
        /// </summary>
        /// <returns>A list of all users.</returns>
        public async Task<List<UserModel>> GetAllUsers()
        {
            return await _connection.Table<UserModel>().ToListAsync();
        }

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The user with the specified ID, or null if not found.</returns>
        public async Task<UserModel> GetUserById(int id)
        {
            return await _connection.Table<UserModel>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="user">The user to add.</param>
        /// <returns>The number of rows added.</returns>
        public async Task<int> AddUser(UserModel user)
        {
            return await _connection.InsertAsync(user);
        }

        /// <summary>
        /// Updates an existing user in the database.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <returns>The number of rows updated.</returns>
        public async Task<int> UpdateUser(UserModel user)
        {
            return await _connection.UpdateAsync(user);
        }

        /// <summary>
        /// Deletes a user from the database.
        /// </summary>
        /// <param name="user">The user to delete.</param>
        /// <returns>The number of rows deleted.</returns>
        public async Task<int> DeleteUser(UserModel user)
        {
            return await _connection.DeleteAsync(user);
        }

        /// <summary>
        /// Retrieves all sensors from the database.
        /// </summary>
        /// <returns>A list of all sensors.</returns>
        public async Task<List<SensorModel>> GetAllSensors()
        {
            return await _connection.Table<SensorModel>().ToListAsync();
        }

        public async Task<SensorModel> GetSensorById(int id)
        {
            return await _connection.Table<SensorModel>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
