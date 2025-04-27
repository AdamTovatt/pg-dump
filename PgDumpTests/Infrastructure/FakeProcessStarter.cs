using PgDump.ProcessStarting;
using System.Diagnostics;

namespace PgDumpTests.Infrastructure
{
    /// <summary>
    /// A fake process starter that returns a fake running process for testing purposes.
    /// </summary>
    public sealed class FakeProcessStarter : IProcessStarter
    {
        private readonly byte[] _outputData;
        private readonly int _exitCode;

        public FakeProcessStarter(Stream standardOutputStream, int exitCode)
        {
            using MemoryStream memoryStream = new MemoryStream();
            standardOutputStream.CopyTo(memoryStream);
            _outputData = memoryStream.ToArray();
            _exitCode = exitCode;
        }

        public IRunningProcess Start(ProcessStartInfo startInfo)
        {
            MemoryStream outputStream = new MemoryStream(_outputData);
            return new FakeRunningProcess(outputStream, _exitCode);
        }
    }
}
