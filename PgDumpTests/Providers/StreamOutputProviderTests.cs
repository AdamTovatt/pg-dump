using PgDump;
using System.Text;

namespace PgDumpTests.Providers
{
    [TestClass]
    public class StreamOutputProviderTests
    {
        private const string TestContent = "Test data for stream output.";

        [TestMethod]
        public async Task WriteAsync_WritesContentToStream()
        {
            // Arrange
            using MemoryStream outputStream = new MemoryStream();
            StreamOutputProvider provider = new StreamOutputProvider(outputStream);
            byte[] contentBytes = Encoding.UTF8.GetBytes(TestContent);
            using MemoryStream inputStream = new MemoryStream(contentBytes);

            // Act
            await provider.WriteAsync(inputStream, CancellationToken.None);

            // Assert
            outputStream.Seek(0, SeekOrigin.Begin);
            using StreamReader reader = new StreamReader(outputStream);
            string result = await reader.ReadToEndAsync();

            Assert.AreEqual(TestContent, result);
        }
    }
}
