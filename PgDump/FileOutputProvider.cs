namespace PgDump
{
    /// <summary>
    /// Provides an output target that writes the database dump to a file.
    /// </summary>
    public class FileOutputProvider : IOutputProvider
    {
        private readonly string _filePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileOutputProvider"/> class.
        /// </summary>
        /// <param name="filePath">The path to the file where the dump will be written.</param>
        public FileOutputProvider(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        /// <summary>
        /// Writes the input stream to the specified file asynchronously.
        /// </summary>
        /// <param name="inputStream">The input stream containing the database dump data.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous write operation.</returns>
        public async Task WriteAsync(Stream inputStream, CancellationToken cancellationToken)
        {
            using FileStream fileStream = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            await inputStream.CopyToAsync(fileStream, cancellationToken);
        }
    }
}
