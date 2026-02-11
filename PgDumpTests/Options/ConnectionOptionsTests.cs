using PgDump;

namespace PgDumpTests.Options
{
    [TestClass]
    public class ConnectionOptionsTests
    {
        [TestMethod]
        public void Constructor_ThrowsArgumentNullException_WhenHostIsNull()
        {
            Assert.Throws<ArgumentNullException>(() 
                => new ConnectionOptions(null!, 5432, "user", "pass", "db"));
        }

        [TestMethod]
        public void Constructor_ThrowsArgumentNullException_WhenUsernameIsNull()
        {
            Assert.Throws<ArgumentNullException>(()
               => new ConnectionOptions("localhost", 5432, null!, "pass", "db"));
        }

        [TestMethod]
        public void Constructor_ThrowsArgumentNullException_WhenPasswordIsNull()
        {
            Assert.Throws<ArgumentNullException>(()
               => new ConnectionOptions("localhost", 5432, "user", null!, "db"));
        }

        [TestMethod]
        public void Constructor_ThrowsArgumentNullException_WhenDatabaseIsNull()
        {
            Assert.Throws<ArgumentNullException>(()
               => new ConnectionOptions("localhost", 5432, "user", "pass", null!));
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
