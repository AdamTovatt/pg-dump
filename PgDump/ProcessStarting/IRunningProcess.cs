namespace PgDump.ProcessStarting
{
    /// <summary>
    /// Represents a running process abstraction to allow interaction with standard output, error, and process control.
    /// </summary>
    public interface IRunningProcess : IDisposable
    {
        /// <summary>
        /// Gets the standard output stream reader of the running process.
        /// </summary>
        StreamReader StandardOutput { get; }

        /// <summary>
        /// Gets the standard error stream reader of the running process.
        /// </summary>
        StreamReader StandardError { get; }

        /// <summary>
        /// Gets the exit code of the process after it exits.
        /// </summary>
        int ExitCode { get; }

        /// <summary>
        /// Kills the running process.
        /// </summary>
        void Kill();

        /// <summary>
        /// Waits indefinitely for the associated process to exit.
        /// </summary>
        void WaitForExit();
    }
}
