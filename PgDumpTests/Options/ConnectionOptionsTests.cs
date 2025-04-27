using PgDump;

namespace PgDumpTests.Options
{
    [TestClass]
    public class ConnectionOptionsTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ThrowsArgumentNullException_WhenHostIsNull()
        {
            _ = new ConnectionOptions(null!, 5432, "user", "pass", "db");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ThrowsArgumentNullException_WhenUsernameIsNull()
        {
            _ = new ConnectionOptions("localhost", 5432, null!, "pass", "db");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ThrowsArgumentNullException_WhenPasswordIsNull()
        {
            _ = new ConnectionOptions("localhost", 5432, "user", null!, "db");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ThrowsArgumentNullException_WhenDatabaseIsNull()
        {
            _ = new ConnectionOptions("localhost", 5432, "user", "pass", null!);
        }

        [TestMethod]
        public void Constructor_Succeeds_WhenAllArgumentsAreValid()
        {
            ConnectionOptions options = new ConnectionOptions("localhost", 5432, "user", "pass", "db");

            Assert.AreEqual("localhost", options.Host);
            Assert.AreEqual(5432, options.Port);
            Assert.AreEqual("user", options.Username);
            Assert.AreEqual("pass", options.Password);
            Assert.AreEqual("db", options.Database);
        }
    }
}
