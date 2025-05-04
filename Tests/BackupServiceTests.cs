using System.IO;
using System.Threading.Tasks;
using SECWRework.Services;
using Xunit;

namespace SECWRework.Tests
{
    public class BackupServiceTests
    {
        [Fact]
        public async Task BackupDatabaseAsync_ShouldCreateBackupFile()
        {
            string dbPath = Path.GetTempFileName();
            string backupPath = Path.GetTempFileName();
            try
            {
                File.WriteAllText(dbPath, "Test Database Content");
                var backupService = new BackupService(dbPath);

                await backupService.BackupDatabaseAsync(backupPath);

                Assert.True(File.Exists(backupPath));
                Assert.Equal(File.ReadAllText(dbPath), File.ReadAllText(backupPath));
            }
            finally
            {
                File.Delete(dbPath);
                File.Delete(backupPath);
            }
        }

        [Fact]
        public async Task RestoreDatabaseAsync_ShouldRestoreBackupFile()
        {
            // Arrange
            string dbPath = Path.GetTempFileName();
            string backupPath = Path.GetTempFileName();
            File.WriteAllText(backupPath, "Backup Database Content");
            var backupService = new BackupService(dbPath);

            // Act
            await backupService.RestoreDatabaseAsync(backupPath);

            // Assert
            Assert.True(File.Exists(dbPath));
            Assert.Equal(File.ReadAllText(backupPath), File.ReadAllText(dbPath));

            // Cleanup
            File.Delete(dbPath);
            File.Delete(backupPath);
        }
    }
}