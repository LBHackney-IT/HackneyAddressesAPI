using Xunit;

namespace LBHAddressesAPITest.Test
{
    using System;
    using System.Data.SqlClient;
    using System.IO;

    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            try
            {
                string dotenv = Path.GetRelativePath(Directory.GetCurrentDirectory(), "../../../../.env");
                DotNetEnv.Env.Load(dotenv);
            }
            catch (Exception)
            {
                // do nothing
            }

            Db = new SqlConnection(DotNetEnv.Env.GetString("ADDRESS_CONNECTION_STRING"));

            Db.Open();
        }

        public SqlConnection Db { get; }

        public void Dispose()
        {
            Db.Close();
            Db.Dispose();
        }
    }
}
