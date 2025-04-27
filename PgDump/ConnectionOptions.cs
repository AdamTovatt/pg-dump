namespace PgDump
{
    /// <summary>
    /// Represents the connection options required to connect to a PostgreSQL database.
    /// </summary>
    public sealed class ConnectionOptions
    {
        /// <summary>
        /// Gets the hostname or IP address of the PostgreSQL server.
        /// </summary>
        public string Host { get; }

        /// <summary>
        /// Gets the port number used to connect to the PostgreSQL server.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Gets the username used for authentication.
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// Gets the password used for authentication.
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// Gets the name of the database to connect to.
        /// </summary>
        public string Database { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionOptions"/> class.
        /// </summary>
        /// <param name="host">The hostname or IP address of the server.</param>
        /// <param name="port">The port number for the connection.</param>
        /// <param name="username">The username for authentication.</param>
        /// <param name="password">The password for authentication.</param>
        /// <param name="database">The database name.</param>
        public ConnectionOptions(string host, int port, string username, string password, string database)
        {
            Host = host ?? throw new ArgumentNullException(nameof(host));
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            Database = database ?? throw new ArgumentNullException(nameof(database));
            Port = port;
        }
    }
}
