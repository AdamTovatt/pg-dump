namespace PgDump
{
    /// <summary>
    /// Provides an output target that writes the database dump to an existing stream.
    /// </summary>
    public class StreamOutputProvider : IOutputProvider
    {
        private readonly Stream _outputStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamOutputProvider"/> class.
        /// </summary>
        /// <param name="outputStream">The output stream where the dump data will be written.</param>
        public StreamOutputProvider(Stream outputStream)
        {
            _outputStream = outputStream ?? throw new ArgumentNullException(nameof(outputStream));
        }

        /// <summary>
        /// Writes the input stream to the specified output stream asynchronously.
        /// </summary>
        /// <param name="inputStream">The input stream containing the database dump data.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous write operation.</returns>
        public async Task WriteAsync(Stream inputStream, CancellationToken cancellationToken)
        {
            await inputStream.CopyToAsync(_outputStream, cancellationToken);
        }
    }
}
