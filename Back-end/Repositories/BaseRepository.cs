using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Back_end
{
    public abstract class BaseRepository
    {
    private SqlDataAdapter adapter;
    public SqlConnection conn;

    public BaseRepository()
    {
        // Initialize the SqlConnection here
        conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["localhost"].ConnectionString);
        adapter = new SqlDataAdapter();
    }

        protected SqlConnection OpenConnection()
        {
            try
            {
                if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                {
                    conn.Open();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return conn;
        }

        protected void CloseConnection()
        {
            conn.Close();
        }

    //     /* For Insert/Update/Delete Queries with transaction */
    //     protected void ExecuteEditTranQuery(string query, SqlParameter[] sqlParameters, SqlTransaction sqlTransaction)
    //     {
    //         SqlCommand command = new SqlCommand(query, conn, sqlTransaction);
    //         try
    //         {
    //             command.Parameters.AddRange(sqlParameters);
    //             adapter.InsertCommand = command;
    //             command.ExecuteNonQuery();
    //         }
    //         catch (Exception)
    //         {
    //             throw;
    //         }
    //     }

    //     /* For Insert/Update/Delete Queries */
    //     protected void ExecuteEditQuery(string query, SqlParameter[] sqlParameters)
    //     {
    //         SqlCommand command = new SqlCommand();

    //         try
    //         {
    //             command.Connection = OpenConnection();
    //             command.CommandText = query;
    //             command.Parameters.AddRange(sqlParameters);
    //             adapter.InsertCommand = command;
    //             command.ExecuteNonQuery();
    //         }
    //         catch (SqlException)
    //         {
    //             throw;
    //         }
    //         finally
    //         {
    //             CloseConnection();
    //         }
    //     }

    //     /* For Select Queries */
    //     protected DataTable ExecuteSelectQuery(SqlCommand command)
    //     {
    //         DataTable dataTable;
    //         DataSet dataSet = new DataSet();

    //         try
    //         {
    //             command.Connection = OpenConnection();
    //             command.ExecuteNonQuery();
    //             adapter.SelectCommand = command;
    //             adapter.Fill(dataSet);
    //             dataTable = dataSet.Tables[0];
    //         }
    //         catch (SqlException)
    //         {
    //             return null;
    //             throw;
    //         }
    //         finally
    //         {
    //             CloseConnection();
    //         }
    //         return dataTable;
    //     }

    //     /* For Select Queries with Paramaters */
    //     protected DataTable ExecuteSelectQuery(string query, params SqlParameter[] sqlParameters)
    //     {
    //         SqlCommand command = new SqlCommand();
    //         DataTable dataTable;
    //         DataSet dataSet = new DataSet();

    //         try
    //         {
    //             command.Connection = OpenConnection();
    //             command.CommandText = query;
    //             command.Parameters.AddRange(sqlParameters);
    //             command.ExecuteNonQuery();
    //             adapter.SelectCommand = command;
    //             adapter.Fill(dataSet);
    //             dataTable = dataSet.Tables[0];
    //         }
    //         catch (SqlException)
    //         {
    //             return null;
    //             throw;
    //         }
    //         finally
    //         {
    //             CloseConnection();
    //         }
    //         return dataTable;
    //     }
    }
}
