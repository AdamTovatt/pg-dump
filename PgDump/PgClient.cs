using PgDump.ProcessStarting;
using System.Diagnostics;

namespace PgDump
{
    /// <summary>
    /// Provides functionality to dump a PostgreSQL database using pg_dump.
    /// </summary>
    public class PgClient
    {
        private readonly ConnectionOptions _options;
        private readonly IProcessStarter _processStarter;

        /// <summary>
        /// Initializes a new instance of the <see cref="PgClient"/> class.
        /// </summary>
        /// <param name="options">The connection options for the PostgreSQL server.</param>
        /// <param name="processStarter">
        /// An optional <see cref="IProcessStarter"/> for starting the pg_dump process. 
        /// If not provided, a default real process starter will be used.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is <c>null</c>.</exception>
        public PgClient(ConnectionOptions options, IProcessStarter? processStarter = null)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _processStarter = processStarter ?? new DefaultProcessStarter();
        }

        /// <summary>
        /// Dumps the PostgreSQL database using pg_dump and writes the output to the specified output provider.
        /// </summary>
        /// <param name="outputProvider">The output provider that handles the dump output stream.</param>
        /// <param name="timeout">The maximum time allowed for the dump operation before cancellation.</param>
        /// <param name="format">The desired output format for the dump. Defaults to <see cref="DumpFormat.Tar"/>.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="IOException">Thrown when pg_dump fails to start or exits with an error.</exception>
        /// <exception cref="TimeoutException">Thrown when the operation exceeds the specified timeout.</exception>
        public async Task DumpAsync(IOutputProvider outputProvider, TimeSpan timeout, DumpFormat format = DumpFormat.Tar, CancellationToken cancellationToken = default)
        {
            using CancellationTokenSource timeoutCts = new CancellationTokenSource(timeout);
            using CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);

            string formatArgument = GetFormatArgument(format);

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "pg_dump",
                Arguments = $"-h {_options.Host} -p {_options.Port} -U {_options.Username} -d {_options.Database} -F {formatArgument}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            startInfo.EnvironmentVariables["PGPASSWORD"] = _options.Password;

            using IRunningProcess process = _processStarter.Start(startInfo);

            Task copyTask = outputProvider.WriteAsync(process.StandardOutput.BaseStream, linkedCts.Token);
            Task<string> errorTask = ReadStandardErrorAsync(process.StandardError, linkedCts.Token);

            try
            {
                await copyTask;
            }
            catch (OperationCanceledException) when (timeoutCts.IsCancellationRequested)
            {
                try
                {
                    process.Kill();
                }
                catch
                {
                    // Ignore
                }
                throw new TimeoutException("pg_dump process timed out.");
            }

            string errorOutput = await errorTask;
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new IOException($"pg_dump failed with exit code {process.ExitCode}: {errorOutput}");
            }
        }

        /// <summary>
        /// Lists all database names on the PostgreSQL server.
        /// </summary>
        /// <param name="timeout">The maximum time allowed for the operation before cancellation.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>A task that returns a list of database names.</returns>
        /// <exception cref="IOException">Thrown when psql fails to start or exits with an error.</exception>
        /// <exception cref="TimeoutException">Thrown when the operation exceeds the specified timeout.</exception>
        public async Task<List<string>> ListDatabasesAsync(TimeSpan timeout, CancellationToken cancellationToken = default)
        {
            using CancellationTokenSource timeoutCancellationTokenSource = new CancellationTokenSource(timeout);
            using CancellationTokenSource linkedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCancellationTokenSource.Token);

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "psql",
                Arguments = $"-h {_options.Host} -p {_options.Port} -U {_options.Username} -d postgres -c \"SELECT datname FROM pg_database WHERE datistemplate = false;\" -At",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            startInfo.EnvironmentVariables["PGPASSWORD"] = _options.Password;

            using IRunningProcess process = _processStarter.Start(startInfo);

            string output = await process.StandardOutput.ReadToEndAsync(linkedCancellationTokenSource.Token);

            try
            {
                await Task.Run(() => process.WaitForExit(), linkedCancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                try
                {
                    process.Kill();
                }
                catch
                {
                    // Ignore
                }
                throw new TimeoutException("psql process timed out.");
            }

            if (process.ExitCode != 0)
            {
                string errorOutput = await process.StandardError.ReadToEndAsync();
                throw new IOException($"psql failed with exit code {process.ExitCode}: {errorOutput}");
            }

            return output.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
        }

        private static async Task<string> ReadStandardErrorAsync(StreamReader reader, CancellationToken cancellationToken)
        {
            char[] buffer = new char[4096];
            using StringWriter stringWriter = new StringWriter();
            while (!reader.EndOfStream)
            {
                int read = await reader.ReadAsync(buffer.AsMemory(0, buffer.Length), cancellationToken);
                if (read > 0)
                {
                    stringWriter.Write(buffer, 0, read);
                }
            }
            return stringWriter.ToString();
        }

        private static string GetFormatArgument(DumpFormat format)
        {
            return format switch
            {
                DumpFormat.Plain => "p",
                DumpFormat.Custom => "c",
                DumpFormat.Directory => "d",
                DumpFormat.Tar => "t",
                _ => "p"
            };
        }
    }
}
