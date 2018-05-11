using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackneyAddressesAPI.Models;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using HackneyAddressesAPI.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Collections.Specialized;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace HackneyAddressesAPI.DB
{
    public class OracleHelper : IDB_Helper
    {
        
        public OracleHelper(){}

        private DataTable ExecuteOracleToDataTable(string conn, string query, OracleParameter[] oracleParameters)
        {
            try
            {
                using (OracleConnection con = new OracleConnection())
                {
                    OracleCommand cmd = PrepareCommand(con, conn, query, oracleParameters);
                    DataTable dt = new DataTable();
                    OracleDataReader reader = cmd.ExecuteReader();
                    dt.Load(reader);
                    return dt;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private OracleCommand PrepareCommand(OracleConnection con, string conn, string query, OracleParameter[] oracleParameters)
        {
            con.ConnectionString = conn;
            con.Open();
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = query;
            cmd.BindByName = true;
            cmd.Parameters.AddRange(oracleParameters);
            return cmd;
        }

        private object ExecuteOracleScalar(string conn, string query, OracleParameter[] oracleParameters)
        {
            try
            {
                using (OracleConnection con = new OracleConnection())
                {
                    OracleCommand cmd = PrepareCommand(con, conn, query, oracleParameters);
                    DataTable dt = new DataTable();
                    var val = cmd.ExecuteScalar();
                    return val;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private int ExecuteOracleScalarInt(string conn, string query, OracleParameter[] oracleParameters)
        {
            int val = int.Parse(ExecuteOracleScalar(conn, query, oracleParameters).ToString());
            return val;
        }

        public DataTable ExecuteToDataTable(string conn, string query, DbParameter[] dbParameters)
        {
            OracleParameter[] oracleParams = dbParameters.Cast<OracleParameter>().ToArray();
            return ExecuteOracleToDataTable(conn, query, oracleParams);
        }

        public int ExecuteScalarToInt(string conn, string query, DbParameter[] dbParameters)
        {
            OracleParameter[] oracleParams = dbParameters.Cast<OracleParameter>().ToArray();
            return ExecuteOracleScalarInt(conn, query, oracleParams);
        }

    }
}
