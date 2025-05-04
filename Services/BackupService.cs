using System.IO;
using System.Threading.Tasks;

namespace SECWRework.Services
{
    /// <summary>
    /// Service for handling database backup and restore operations.
    /// </summary>
    public class BackupService
    {
        /// <summary>
        /// The path to the database file.
        /// </summary>
        private readonly string _dbPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackupService"/> class.
        /// </summary>
        /// <param name="dbPath">The path to the database file.</param>
        public BackupService(string dbPath)
        {
            _dbPath = dbPath;
        }

        /// <summary>
        /// Creates a backup of the database at the specified path.
        /// </summary>
        /// <param name="backupPath">The path where the backup file will be saved.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task BackupDatabaseAsync(string backupPath)
        {
            if (File.Exists(_dbPath))
            {
                await Task.Run(() => File.Copy(_dbPath, backupPath, overwrite: true));
            }
        }

        /// <summary>
        /// Restores the database from a backup file.
        /// </summary>
        /// <param name="backupPath">The path to the backup file.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task RestoreDatabaseAsync(string backupPath)
        {
            if (File.Exists(backupPath))
            {
                await Task.Run(() => File.Copy(backupPath, _dbPath, overwrite: true));
            }
        }
    }
}