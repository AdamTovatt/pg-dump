namespace PgDumpTests.Infrastructure
{
    public class SlowMemoryStream : MemoryStream // so that we can test cancellation (the normal memory stream is to fast for being cancelled)
    {
        private readonly int _delayMilliseconds;

        public SlowMemoryStream(byte[] buffer, int delayMilliseconds)
            : base(buffer)
        {
            _delayMilliseconds = delayMilliseconds;
        }

        public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            await Task.Delay(_delayMilliseconds, cancellationToken);
            return await base.ReadAsync(buffer, cancellationToken);
        }
    }
}
