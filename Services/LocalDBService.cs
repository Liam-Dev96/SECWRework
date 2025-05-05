using SECWRework.Model;
using SQLite;
using OfficeOpenXml;

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
            _connection.CreateTableAsync<SensorModel>();
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

        public async Task ImportSensorsFromExcel(string filePath)
        {
            
            // Ensure the EPPlus library is licensed for non-commercial use or use a commercial license
            ExcelPackage.License.SetNonCommercialOrganization("My Noncommercial organization");

            using var package = new ExcelPackage(new FileInfo(filePath));
            var worksheet = package.Workbook.Worksheets[0]; // Assuming the first sheet contains the data

            var sensors = new List<SensorModel>();

            // Start reading from the second row (assuming the first row contains headers)
            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                var sensor = new SensorModel
                {
                    Category = worksheet.Cells[row, 1].Text,
                    Quantity = worksheet.Cells[row, 2].Text,
                    Symbol = worksheet.Cells[row, 3].Text,
                    Unit = worksheet.Cells[row, 4].Text,
                    MeasurementFrequency = worksheet.Cells[row, 6].Text,
                    SafeLevel = worksheet.Cells[row, 7].Text,
                    Status = "Online", // Default status
                    Location = worksheet.Cells[row, 9].Text
                };

                sensors.Add(sensor);
            }

            // Use a bulk insert for better performance
            await BulkInsertSensors(sensors);
        }

        public async Task BulkInsertSensors(List<SensorModel> sensors)
        {
        await _connection.RunInTransactionAsync(tran =>
            {
            foreach (var sensor in sensors)
            {
                tran.Insert(sensor);
            }
            });     
        }
    }
}
