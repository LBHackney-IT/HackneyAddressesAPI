using System.Data;
using System.Data.Common;
using Oracle.ManagedDataAccess.Client;

namespace LBHAddressesAPI.Interfaces
{
    public interface IDB_Helper
    {
        DataTable ExecuteToDataTable(string conn, string query, DbParameter[] dbParameters);
        int ExecuteScalarToInt(string conn, string query, DbParameter[] dbParameters);
    }
}