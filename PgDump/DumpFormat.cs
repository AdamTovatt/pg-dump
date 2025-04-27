namespace PgDump
{
    /// <summary>
    /// Specifies the output format for pg_dump.
    /// </summary>
    public enum DumpFormat
    {
        /// <summary>
        /// Plain SQL script.
        /// </summary>
        Plain,

        /// <summary>
        /// Custom format archive.
        /// </summary>
        Custom,

        /// <summary>
        /// Directory format archive.
        /// </summary>
        Directory,

        /// <summary>
        /// Tar archive.
        /// </summary>
        Tar
    }
}
