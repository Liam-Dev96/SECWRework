using SECWRework.Model;
using SQLite;

namespace SECWRework
{
    public class LocalDBService
    {

        private const string DB_NAME = "SoftwareEngineering.db";
        private readonly SQLiteAsyncConnection _connection;

        public LocalDBService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            _connection.CreateTableAsync<UserModel>();
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
            return await _connection.Table<UserModel>().ToListAsync();
        }

        public async Task<UserModel> GetUserById(int id)
        {
            return await _connection.Table<UserModel>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> AddUser(UserModel user)
        {
            return await _connection.InsertAsync(user);
        }

        public async Task<int> UpdateUser(UserModel user)
        {
            return await _connection.UpdateAsync(user);
        }

        public async Task<int> DeleteUser(UserModel user)
        {
            return await _connection.DeleteAsync(user);
        }

    }
}
