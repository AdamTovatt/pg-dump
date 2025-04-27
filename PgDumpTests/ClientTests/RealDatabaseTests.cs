using PgDump;

namespace PgDumpTests.ClientTests
{
    [TestClass]
    public class RealDatabaseTests
    {
        private const string Host = "localhost";
        private const int Port = 5432;
        private const string Username = "postgres";
        private const string Password = "password"; // Replace with real password
        private const string Database = "pg_dump_fake_db";

        [TestMethod]
        public async Task DumpAsync_CreatesValidTarDump_WhenUsingRealPgDump()
        {
            // Arrange
            string tempFilePath = Path.GetTempFileName();
            FileOutputProvider outputProvider = new FileOutputProvider(tempFilePath);

            ConnectionOptions options = new ConnectionOptions(Host, Port, Username, Password, Database);
            PgClient client = new PgClient(options);

            TimeSpan timeout = TimeSpan.FromMinutes(1);

            // Act
            await client.DumpAsync(outputProvider, timeout, DumpFormat.Tar, CancellationToken.None);

            // Assert
            Assert.IsTrue(File.Exists(tempFilePath));
            FileInfo fileInfo = new FileInfo(tempFilePath);
            Assert.IsTrue(fileInfo.Length > 0, "The dump file should not be empty.");

            // Cleanup
            File.Delete(tempFilePath);
        }

        [TestMethod]
        public async Task DumpAsync_CreatesValidTarDumpInMemory_WhenUsingRealPgDump()
        {
            // Arrange
            using MemoryStream memoryStream = new MemoryStream();
            StreamOutputProvider outputProvider = new StreamOutputProvider(memoryStream);

            ConnectionOptions options = new ConnectionOptions(Host, Port, Username, Password, Database);
            PgClient client = new PgClient(options);

            TimeSpan timeout = TimeSpan.FromMinutes(1);

            // Act
            await client.DumpAsync(outputProvider, timeout, DumpFormat.Tar, CancellationToken.None);

            // Assert
            Assert.IsTrue(memoryStream.Length > 0, "The in-memory dump should not be empty.");
        }

        [TestMethod]
        public async Task DumpAsync_CreatesValidPlainTextDump_WhenUsingRealPgDump()
        {
            // Arrange
            string tempFilePath = Path.GetTempFileName();
            FileOutputProvider outputProvider = new FileOutputProvider(tempFilePath);

            ConnectionOptions options = new ConnectionOptions(Host, Port, Username, Password, Database);
            PgClient client = new PgClient(options);

            TimeSpan timeout = TimeSpan.FromMinutes(1);

            // Act
            await client.DumpAsync(outputProvider, timeout, DumpFormat.Plain, CancellationToken.None);

            // Assert
            Assert.IsTrue(File.Exists(tempFilePath));
            string content = await File.ReadAllTextAsync(tempFilePath);
            Assert.IsFalse(string.IsNullOrWhiteSpace(content), "The plain text dump should not be empty.");

            // Cleanup
            File.Delete(tempFilePath);
        }

        [TestMethod]
        public async Task DumpAsync_CreatesValidPlainTextDumpInMemory_WhenUsingRealPgDump()
        {
            // Arrange
            using MemoryStream memoryStream = new MemoryStream();
            StreamOutputProvider outputProvider = new StreamOutputProvider(memoryStream);

            ConnectionOptions options = new ConnectionOptions(Host, Port, Username, Password, Database);
            PgClient client = new PgClient(options);

            TimeSpan timeout = TimeSpan.FromMinutes(1);

            // Act
            await client.DumpAsync(outputProvider, timeout, DumpFormat.Plain, CancellationToken.None);

            // Assert
            memoryStream.Position = 0;
            using StreamReader reader = new StreamReader(memoryStream);
            string content = await reader.ReadToEndAsync();
            Assert.IsFalse(string.IsNullOrWhiteSpace(content), "The in-memory plain text dump should not be empty.");
        }

        [TestMethod]
        public async Task ListDatabasesAsync_ReturnsNonEmptyList_WhenUsingRealPgDump()
        {
            // Arrange
            ConnectionOptions options = new ConnectionOptions(Host, Port, Username, Password, Database);
            PgClient client = new PgClient(options);

            TimeSpan timeout = TimeSpan.FromSeconds(30);

            // Act
            List<string> databases = await client.ListDatabasesAsync(timeout, CancellationToken.None);

            // Assert
            Assert.IsNotNull(databases);
            Assert.IsTrue(databases.Count > 0, "Database list should not be empty.");
            Assert.IsTrue(databases.Contains(Database), $"Database list should contain '{Database}'.");
        }
    }
}
