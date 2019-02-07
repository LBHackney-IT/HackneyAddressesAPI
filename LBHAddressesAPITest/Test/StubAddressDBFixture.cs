using Xunit;

namespace LBHAddressesAPITest.Test
{
    using System;
    using System.Data.SqlClient;
    using System.IO;

    public class DatabaseFixture : IDisposable
    {
        public SqlConnection Db { get; }
        public string ConnectionString { get; }

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
            string constr = "server=localhost\\SQLExpress; database=ADDRESSES_API_LOCAL; User Id=addressesAPI_dev;Password=LLPGdev;";
            //Db = new SqlConnection(DotNetEnv.Env.GetString("LLPGConnectionStringLOCAL"));
            Db = new SqlConnection(constr);

            Db.Open();
        }

        
        public void Dispose()
        {
            Db.Close();
            Db.Dispose();
        }
    }
}
