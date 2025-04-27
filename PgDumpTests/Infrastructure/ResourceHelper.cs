using System.Reflection;

namespace PgDumpTests.Infrastructure
{
    /// <summary>
    /// Helper for loading embedded resource files in tests.
    /// </summary>
    public static class ResourceHelper
    {
        private const string ResourcePrefix = "PgDumpTests.Resources.";

        /// <summary>
        /// Gets a stream for any embedded resource by its full resource name.
        /// </summary>
        /// <param name="fullResourceName">The full resource name including namespace and file name.</param>
        /// <returns>A stream containing the resource data.</returns>
        /// <exception cref="FileNotFoundException">Thrown if the resource is not found.</exception>
        public static Stream GetEmbeddedResourceStream(string fullResourceName)
        {
            Assembly assembly = typeof(ResourceHelper).Assembly;
            Stream? stream = assembly.GetManifestResourceStream(fullResourceName);
            if (stream == null)
            {
                throw new FileNotFoundException($"Embedded resource '{fullResourceName}' not found.");
            }
            return stream;
        }

        /// <summary>
        /// Gets a stream for a test resource from the 'Resources' folder by file name only.
        /// </summary>
        /// <param name="fileName">The file name of the resource inside the Resources folder.</param>
        /// <returns>A stream containing the resource data.</returns>
        public static Stream GetTestResourceStream(string fileName)
        {
            return GetEmbeddedResourceStream($"{ResourcePrefix}{fileName}");
        }
    }
}
