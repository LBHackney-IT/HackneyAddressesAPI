﻿using Xunit;

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
            ConnectionString =  DotNetEnv.Env.GetString("LLPGConnectionString");
            Db = new SqlConnection(ConnectionString);
            Db.Open();
        }

        
        public void Dispose()
        {
            Db.Close();
            Db.Dispose();
        }
    }
}
