using System.IO;
using System.Threading.Tasks;

namespace SECWRework.Services
{
    public class BackupService
    {
        private readonly string _dbPath;

        public BackupService(string dbPath)
        {
            _dbPath = dbPath;
        }

        public async Task BackupDatabaseAsync(string backupPath)
        {
            if (File.Exists(_dbPath))
            {
                await Task.Run(() => File.Copy(_dbPath, backupPath, overwrite: true));
            }
        }

        public async Task RestoreDatabaseAsync(string backupPath)
        {
            if (File.Exists(backupPath))
            {
                await Task.Run(() => File.Copy(backupPath, _dbPath, overwrite: true));
            }
        }
    }
}