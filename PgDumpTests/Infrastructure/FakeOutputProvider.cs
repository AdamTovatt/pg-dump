using PgDump;

namespace PgDumpTests.Infrastructure
{
    /// <summary>
    /// A fake output provider that stores the written output in memory for testing.
    /// </summary>
    public sealed class FakeOutputProvider : IOutputProvider
    {
        /// <summary>
        /// Gets the memory stream where the output is written.
        /// </summary>
        public MemoryStream Output { get; } = new MemoryStream();

        /// <inheritdoc/>
        public async Task WriteAsync(Stream inputStream, CancellationToken cancellationToken)
        {
            await inputStream.CopyToAsync(Output, cancellationToken);
        }
    }
}
