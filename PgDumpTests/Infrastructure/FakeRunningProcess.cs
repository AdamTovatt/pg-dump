using PgDump.ProcessStarting;

namespace PgDumpTests.Infrastructure
{
    /// <summary>
    /// A fake running process for testing purposes.
    /// </summary>
    public sealed class FakeRunningProcess : IRunningProcess
    {
        private readonly StreamReader _standardOutputReader;
        private readonly int _exitCode;

        public FakeRunningProcess(Stream standardOutputStream, int exitCode)
        {
            _standardOutputReader = new StreamReader(standardOutputStream);
            _exitCode = exitCode;
        }

        public StreamReader StandardOutput => _standardOutputReader;

        public StreamReader StandardError => new StreamReader(Stream.Null);

        public int ExitCode => _exitCode;

        public void Kill()
        {
            // No-op
        }

        public void WaitForExit()
        {
            // No-op
        }

        public void Dispose()
        {
            _standardOutputReader.Dispose();
        }
    }
}
