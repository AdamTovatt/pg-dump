using System.Diagnostics;

namespace PgDump.ProcessStarting
{
    /// <summary>
    /// Abstraction for starting processes, returning a running process abstraction.
    /// </summary>
    public interface IProcessStarter
    {
        /// <summary>
        /// Starts a process with the specified start information and returns a wrapper around the running process.
        /// </summary>
        /// <param name="startInfo">The information used to start the process.</param>
        /// <returns>An <see cref="IRunningProcess"/> that represents the started process.</returns>
        IRunningProcess Start(ProcessStartInfo startInfo);
    }
}
