using PgDump;
using PgDump.ProcessStarting;
using PgDumpTests.Infrastructure;

namespace PgDumpTests.ClientTests
{
    [TestClass]
    public class EmbeddedFileDataTests
    {
        const string tar_dump_file_name = "tar_dump.tar";

        [TestMethod]
        public async Task DumpAsync_WritesDumpOutputCorrectly_WhenProcessSucceeds()
        {
            // Arrange
            Stream embeddedDumpStream = ResourceHelper.GetTestResourceStream(tar_dump_file_name);
            IProcessStarter fakeStarter = new FakeProcessStarter(embeddedDumpStream, exitCode: 0);

            ConnectionOptions options = new ConnectionOptions("localhost", 5432, "user", "pass", "db");
            PgClient client = new PgClient(options, fakeStarter);
            FakeOutputProvider outputProvider = new FakeOutputProvider();
            TimeSpan timeout = TimeSpan.FromSeconds(5);

            // Act
            await client.DumpAsync(outputProvider, timeout, DumpFormat.Tar, CancellationToken.None);

            // Assert
            Stream expectedStream = ResourceHelper.GetTestResourceStream(tar_dump_file_name);
            byte[] expectedBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                expectedStream.CopyTo(ms);
                expectedBytes = ms.ToArray();
            }

            byte[] actualBytes = outputProvider.Output.ToArray();
            CollectionAssert.AreEqual(expectedBytes, actualBytes);
        }

        [TestMethod]
        public async Task DumpAsync_ThrowsIOException_WhenProcessFails()
        {
            // Arrange
            Stream fakeOutput = new MemoryStream(); // Empty output
            IProcessStarter fakeStarter = new FakeProcessStarter(fakeOutput, exitCode: 1); // Non-zero exit code

            ConnectionOptions options = new ConnectionOptions("localhost", 5432, "user", "pass", "db");
            PgClient client = new PgClient(options, fakeStarter);
            FakeOutputProvider outputProvider = new FakeOutputProvider();
            TimeSpan timeout = TimeSpan.FromSeconds(5);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<IOException>(async () =>
            {
                await client.DumpAsync(outputProvider, timeout, DumpFormat.Tar, CancellationToken.None);
            });
        }
    }
}
