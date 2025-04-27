using System.Diagnostics;

namespace PgDump.ProcessStarting
{
    /// <summary>
    /// Default implementation of <see cref="IProcessStarter"/> that starts real processes.
    /// </summary>
    public sealed class DefaultProcessStarter : IProcessStarter
    {
        /// <inheritdoc/>
        public IRunningProcess Start(ProcessStartInfo startInfo)
        {
            Process process = Process.Start(startInfo) ?? throw new IOException("Failed to start process.");
            return new RealRunningProcess(process);
        }
    }
}
