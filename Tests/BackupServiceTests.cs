using System.IO;
using System.Threading.Tasks;
using SECWRework.Services;
using Xunit;

namespace SECWRework.Tests
{
    /// <summary>
    /// Unit tests for the <see cref="BackupService"/> class.
    /// </summary>
    public class BackupServiceTests
    {
        /// <summary>
        /// Tests that <see cref="BackupService.BackupDatabaseAsync"/> creates a backup file successfully.
        /// </summary>
        [Fact]
        public async Task BackupDatabaseAsync_ShouldCreateBackupFile()
        {
            // Arrange
            string dbPath = Path.GetTempFileName();
            string backupPath = Path.GetTempFileName();
            try
            {
                File.WriteAllText(dbPath, "Test Database Content");
                var backupService = new BackupService(dbPath);

                // Act
                await backupService.BackupDatabaseAsync(backupPath);

                // Assert
                Assert.True(File.Exists(backupPath));
                Assert.Equal(File.ReadAllText(dbPath), File.ReadAllText(backupPath));
            }
            finally
            {
                // Cleanup
                File.Delete(dbPath);
                File.Delete(backupPath);
            }
        }

        /// <summary>
        /// Tests that <see cref="BackupService.RestoreDatabaseAsync"/> restores a backup file successfully.
        /// </summary>
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