using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCTutorial.DAL.ADO
{
    public interface IDatabase
    {
        String ConnectionString();
        // Builds a query command
        DbCommand BuildQueryCommand(string sql, IDataParameter[] param);
        // Builds a query command
        // DbCommand BuildQueryCommand(DbConnection conn, DbTransaction trans, string sql, IDataParameter[] param);
        // Execustes a data set
        DataSet ExecuteDataSet(string sql, IDataParameter[] param, string tableName);
        // Execustes a data datatable
        DataTable ExecuteDataTable(string sql);
        // Execustes a data reader
        DbDataReader ExecuteReader(string sql, IDataParameter[] param);
        // Execustes a data reader
        // DbDataReader ExecuteReader(DbConnection conn, DbTransaction trans, string sql, IDataParameter[] param);
        // close connection
        void closeConnection();
        // Execustes a non query
        // int ExecuteNonQuery(DbConnection conn, DbTransaction trans, string sql, IDataParameter[] param);
        // Execustes a non query
        int ExecuteNonQuery(string sql, IDataParameter[] param);
        // Execustes a non query
        int ExecuteIdentityNonQuery(string sql, IDataParameter[] param);
        // Execustes a scalar
        object ExecuteScalar(string sql, IDataParameter[] param);
        // Execustes a scalar
        String ExecuteScalarString(string sql, IDataParameter[] param);
        // Execustes a scalar
        int ExecuteScalarInt(string sql, IDataParameter[] param);
        // Execustes a scalar
        // object ExecuteScalar(DbConnection conn, DbTransaction trans, string sql, IDataParameter[] param);
        // Gets a database parameter
        DbParameter getDbParameter(string paramname, object value);
        // Gets a database parameter
        DbParameter getDbParameter(string paramname, object value, int id);
        DbParameter getDbParameter(string paramname, object value, SqlDbType dbtype);
        // Sanitize the sql string
        String sanitize(string sql);
        // Returns the sql statement for this
        String getStatement(string name);
    }
}
