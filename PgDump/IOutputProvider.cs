namespace PgDump
{
    /// <summary>
    /// Defines an output target for receiving the database dump stream.
    /// </summary>
    public interface IOutputProvider
    {
        /// <summary>
        /// Writes the input stream to the output destination asynchronously.
        /// </summary>
        /// <param name="inputStream">The input stream containing the database dump data.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous write operation.</returns>
        Task WriteAsync(Stream inputStream, CancellationToken cancellationToken);
    }
}
