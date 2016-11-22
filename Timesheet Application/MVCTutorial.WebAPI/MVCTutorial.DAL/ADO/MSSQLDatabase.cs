using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MVCTutorial.DAL.ADO
{
    public class MSSQLDatabase : IDatabase
    {
        private ArrayList _connections;
        private Object _lockObj;
        private Hashtable _lookupTbl;
        private int lastconnection = 0;
        private int minConnection = 5;
        private string _connectionString = @"Data Source=.;Initial Catalog=Timesheet;Integrated Security=True";
        //private string _connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Timesheet;Integrated Security=True";

        public MSSQLDatabase(string connectionString)
        {
            _connectionString = connectionString;
            _lockObj = new Object();
            _connections = new ArrayList();
            _lookupTbl = new Hashtable();
            for (int i = 0; i < minConnection; i++)
            {
                _connections.Add(new MSSQLConnectionPoolItem(_connectionString));
            }
        }

        public string getDetails()
        {
            int avail = 0; int used = 0;
            lock (_lockObj)
            {
                for (int i = 0; i < _connections.Count; i++)
                {
                    MSSQLConnectionPoolItem item = (MSSQLConnectionPoolItem)_connections[0];
                    if (item.getBusy())
                        used++;
                    else
                        avail++;
                }
            }
            return "Available Connections " + avail + " - Busy Connections " + used;
        }

        public string ConnectionString()
        {
            return _connectionString;
        }

        public DbConnection getConnection()
        {
            MSSQLConnectionPoolItem item = getIntConnection();
            DbConnection con = item.getConnection();
            _lookupTbl.Add(con, item);
            return con;
        }

        public MSSQLConnectionPoolItem getIntConnection()
        {
            // DebugLog.LogMessageToFile("Trying to get connection");
            lock (_lockObj)
            {
                for (int i = 0; i < _connections.Count; i++)
                {
                    lastconnection++;
                    if (lastconnection > _connections.Count) lastconnection = lastconnection - _connections.Count;
                    // DebugLog.LogMessageToFile("Connection ID " + lastconnection);
                    MSSQLConnectionPoolItem item = (MSSQLConnectionPoolItem)_connections[lastconnection - 1];
                    if (!item.getBusy())
                    {
                        // DebugLog.LogMessageToFile("Connection Success " + lastconnection);
                        item.setBusy(true);
                        return item;
                    }
                }
                // DebugLog.LogMessageToFile("Create new Connection " + lastconnection + 1);
                MSSQLConnectionPoolItem newitem = new MSSQLConnectionPoolItem(_connectionString);
                newitem.setBusy(true);
                _connections.Add(newitem);
                lastconnection = _connections.Count;
                return newitem;
            }
        }

        public void closeConnection(DbConnection cmd)
        {
        }

        public void closeConnection(MSSQLConnectionPoolItem item)
        {
            item.setBusy(false);
        }

        public void closeConnection()
        {
        }

        public void releaseCommand(DbCommand cmd)
        {
            if (cmd == null) return;
            if (_lookupTbl.Contains(cmd))
            {
                MSSQLConnectionPoolItem item = (MSSQLConnectionPoolItem)_lookupTbl[cmd];
                item.setBusy(false);
            }
        }

        public DbCommand BuildQueryCommand(string sql, IDataParameter[] param)
        {
            MSSQLConnectionPoolItem item = getIntConnection();
            SqlCommand com = new SqlCommand(sql, item.getConnection());
            if (com == null) return null;
            if (param != null)
                foreach (SqlParameter sp in param)
                    com.Parameters.Add(sp);
            _lookupTbl.Add(com, item);
            return com;
        }

        public DbCommand BuildQueryCommand(DbConnection conn, DbTransaction trans, string sql, IDataParameter[] param)
        {
            SqlCommand com = new SqlCommand(sql, (SqlConnection)conn);
            com.Transaction = (SqlTransaction)trans;
            if (param != null)
                foreach (SqlParameter sp in param)
                    com.Parameters.Add(sp);
            return com;
        }

        public DataSet ExecuteDataSet(string sql, IDataParameter[] param, string tableName)
        {
            SqlCommand cmd = null;
            try
            {
                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter();
                cmd = (SqlCommand)BuildQueryCommand(sql, param);
                if (cmd != null)
                {
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    releaseCommand(cmd);
                    return ds;
                }
                return null;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (cmd != null)
                    releaseCommand(cmd);
            }
        }

        public DataTable ExecuteDataTable(string sql)
        {
            SqlCommand cmd = null;
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                cmd = (SqlCommand)BuildQueryCommand(sql, null);
                if (cmd != null)
                {
                    DataTable dt = new DataTable();
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);
                    releaseCommand(cmd);
                    //DebugLog.LogMessageToFile("Datasync.cs", "Execute Datatable =" + dt.Rows.Count + " ~~ " + sql );
                    return dt;
                }
                return null;
            }
            catch 
            {
                //DebugLog.LogMessageToFile("Datasync.cs", "Execute Datatable =" + ex.Message);
                //DebugLog.LogMessageToFile("Datasync.cs", "Execute Datatable =" + ex.StackTrace);
                return null;
            }
            finally
            {
                if (cmd != null)
                    releaseCommand(cmd);
            }
        }

        public DbDataReader ExecuteReader(string sql, IDataParameter[] param)
        {
            SqlCommand scq = (SqlCommand)BuildQueryCommand(sql, param);
            if (scq != null)
            {
                SqlDataReader sdr = scq.ExecuteReader();
                return sdr;
            }
            else
            {
                return null;
            }
        }

        public DbDataReader ExecuteReader(DbConnection conn, DbTransaction trans, string sql, IDataParameter[] param)
        {
            SqlCommand scq = (SqlCommand)BuildQueryCommand(conn, trans, sql, param);
            if (scq != null)
            {
                SqlDataReader sdr = scq.ExecuteReader();
                return sdr;
            }
            else
            {
                return null;
            }
        }


        public int ExecuteNonQuery(DbConnection conn, DbTransaction trans, string sql, IDataParameter[] param)
        {
            int row = -1;
            SqlCommand sc = null;
            try
            {
                sc = (SqlCommand)BuildQueryCommand(conn, (SqlTransaction)trans, sql, param);
                row = sc.ExecuteNonQuery();
            }
            catch
            {
                return -1;
            }
            finally
            {
            }
            return row;
        }

        public int ExecuteNonQuery(string sql, IDataParameter[] param)
        {
            int row = -1;
            SqlCommand sc = null;
            try
            {
                sc = (SqlCommand)BuildQueryCommand(sql, param);
                row = sc.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                releaseCommand(sc);
            }
            return row;
        }

        public int ExecuteIdentityNonQuery(string sql, IDataParameter[] param)
        {
            int row = -1;
            SqlCommand sc = null;
            // MSSQLConnectionPoolItem item = null;
            try
            {
                sc = (SqlCommand)BuildQueryCommand(sql + "; SELECT @@IDENTITY;", param);
                object rowt = sc.ExecuteScalar();
                releaseCommand(sc);
                row = Int32.Parse(rowt.ToString());
            }
            catch
            {
                return -1;
            }
            finally
            {
            }
            return row;
        }

        public object ExecuteScalar(string sql, IDataParameter[] param)
        {
            object scalar = null;
            SqlCommand sc = null;
            try
            {
                sc = (SqlCommand)BuildQueryCommand(sql, param);
                scalar = sc.ExecuteScalar();
            }
            catch
            {
                return -1;
            }
            finally
            {
                releaseCommand(sc);
            }
            return scalar;
        }

        public String ExecuteScalarString(string sql, IDataParameter[] param)
        {
            object scalar = null;
            SqlCommand sc = null;
            try
            {
                sc = (SqlCommand)BuildQueryCommand(sql, param);
                scalar = sc.ExecuteScalar();
            }
            catch
            {
                return null;
            }
            finally
            {
                releaseCommand(sc);
            }
            if (scalar == null) return "";
            return (String)scalar;
        }

        public int ExecuteScalarInt(string sql, IDataParameter[] param)
        {
            object scalar = null;
            SqlCommand sc = null;
            try
            {
                sc = (SqlCommand)BuildQueryCommand(sql, param);
                scalar = sc.ExecuteScalar();
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                releaseCommand(sc);
            }
            if (scalar == null) return 0;
            try
            {
                return (int)scalar;
            }
            catch
            {
                return Convert.ToInt32(scalar);
            }
        }

        public object ExecuteScalar(DbConnection conn, DbTransaction trans, string sql, IDataParameter[] param)
        {
            object scalar = null;
            SqlCommand sc = null;
            try
            {
                sc = (SqlCommand)BuildQueryCommand(conn, (SqlTransaction)trans, sql, param);
                scalar = sc.ExecuteScalar();
            }
            catch
            {
                return 0;
            }
            finally
            {
            }
            return scalar;
        }

        public DbParameter getDbParameter(string paramname, object value)
        {
            string tparam = paramname.Replace("?", "@");
            return new SqlParameter(tparam, value);
        }

        public DbParameter getDbParameter(string paramname, object value, int id)
        {
            string tparam = paramname.Replace("?", "@");
            return new SqlParameter(tparam, value);
        }

        public DbParameter getDbParameter(string paramname, object value, SqlDbType dbtype)
        {
            string tparam = paramname.Replace("?", "@");
            SqlParameter param = new SqlParameter(tparam, value);
            param.SqlDbType = dbtype;
            param.IsNullable = true;
            return param;
        }
        public String sanitize(string sql)
        {
            sql = sql.Replace("?", "@");
            sql = sql.Replace("autoincrement", "IDENTITY(1,1) ");
            return sql;
        }

        public String getStatement(string name)
        {
            return MSSQLStatements.GetStatement(name);
        }
    }

    public class MSSQLConnectionPoolItem
    {
        private SqlConnection _sqlConnection;
        private string _connectionStr;
        private bool _busy = false;
        private static DateTime _dtdb = DateTime.Now.AddDays(-1);
        private static DateTime _busyDate;

        public MSSQLConnectionPoolItem(string connectionStr)
        {
            _connectionStr = connectionStr;
            _sqlConnection = new SqlConnection(connectionStr);
            _dtdb = DateTime.Now;
            _busy = false;
        }

        public SqlConnection getConnection()
        {
            try
            {
                System.TimeSpan diffResult = DateTime.Now.Subtract(_dtdb);
                if (diffResult.Hours > 2 || diffResult.Days >= 1)
                {
                    _dtdb = DateTime.Now;
                    _sqlConnection.Open();
                }
                else if (_sqlConnection.State != ConnectionState.Open)
                {
                    _dtdb = DateTime.Now;
                    _sqlConnection.Open();
                }
            }
            catch 
            {
                _sqlConnection.Dispose();
                _sqlConnection = new SqlConnection(_connectionStr);
                try
                {
                    _dtdb = DateTime.Now;
                    _sqlConnection.Open();
                }
                catch
                {
                }
            }
            return _sqlConnection;
        }

        public void setBusy(bool status)
        {
            _busyDate = DateTime.Now;
            _busy = status;
        }

        public void closeConnection()
        {
            setBusy(false);
        }

        public bool getBusy()
        {
            if (_busy)
            {
                System.TimeSpan diffResult = DateTime.Now.Subtract(_busyDate);
                // Ten minutes for an open connection -> destroy it
                if (diffResult.Minutes > 10)
                {
                    _busy = false;
                }
            }
            return _busy;
        }
    }
}