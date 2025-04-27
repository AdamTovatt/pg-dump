using System.Diagnostics;

namespace PgDump.ProcessStarting
{
    /// <summary>
    /// Wraps a real <see cref="Process"/> to implement <see cref="IRunningProcess"/>.
    /// </summary>
    public sealed class RealRunningProcess : IRunningProcess
    {
        private readonly Process _process;

        /// <summary>
        /// Initializes a new instance of the <see cref="RealRunningProcess"/> class.
        /// </summary>
        /// <param name="process">The real process to wrap.</param>
        public RealRunningProcess(Process process)
        {
            _process = process ?? throw new ArgumentNullException(nameof(process));
        }

        /// <inheritdoc/>
        public StreamReader StandardOutput => _process.StandardOutput;

        /// <inheritdoc/>
        public StreamReader StandardError => _process.StandardError;

        /// <inheritdoc/>
        public int ExitCode => _process.ExitCode;

        /// <inheritdoc/>
        public void Kill() => _process.Kill();

        /// <inheritdoc/>
        public void WaitForExit() => _process.WaitForExit();

        /// <inheritdoc/>
        public void Dispose() => _process.Dispose();
    }
}
