using PgDump;
using System.Text;

namespace PgDumpTests.Providers
{
    [TestClass]
    public class FileOutputProviderTests
    {
        private const string TestContent = "Test data for file output.";

        [TestMethod]
        public async Task WriteAsync_WritesContentToFile()
        {
            // Arrange
            string tempFilePath = Path.GetTempFileName();
            FileOutputProvider provider = new FileOutputProvider(tempFilePath);
            byte[] contentBytes = Encoding.UTF8.GetBytes(TestContent);
            using MemoryStream inputStream = new MemoryStream(contentBytes);

            // Act
            await provider.WriteAsync(inputStream, CancellationToken.None);

            // Assert
            string result = await File.ReadAllTextAsync(tempFilePath);
            Assert.AreEqual(TestContent, result);

            // Cleanup
            File.Delete(tempFilePath);
        }
    }
}
