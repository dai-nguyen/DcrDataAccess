/** 
 * This file is part of the DcrDataAccess project.
 * Copyright (c) 2014 Dai Nguyen
 * Author: Dai Nguyen
**/

using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DcrDataAccess
{
    public class DataService : IDisposable
    {
        const string SERVER = "";
        const string DB = "";
        const string USER = "";
        const string PASS = "";
        const string DBMAIL_PROFILE = "";

        public SqlConnection SqlConn { get; private set; }

        public DataService()
        {
            string connStr = string.Format("server={0};database={1};user id={2};password={3}", SERVER, DB, USER, PASS);
            SqlConn = new SqlConnection(connStr);
        }

        public DataService(string server, string db, string user, string pass)
        {
            string connStr = string.Format("server={0};database={1};user id={2};password={3}", server, db, user, pass);
            SqlConn = new SqlConnection(connStr);
        }

        public DataService(string server, string db)
        {
            string connStr = string.Format("server={0};database={1};user id={2};password={3}", server, db, USER, PASS);
            SqlConn = new SqlConnection(connStr);
        }

        public void SendDbMail(string[] tos, string reply_to, string subject, string body)
        {
            using (SqlCommand cmd = new SqlCommand("msdb.dbo.sp_send_dbmail", SqlConn))
            {

                StringBuilder builder = new StringBuilder();
                foreach (string to in tos)
                {
                    builder.Append(to + ";");
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@profile_name", DBMAIL_PROFILE);
                cmd.Parameters.AddWithValue("@recipients", builder.ToString());
                cmd.Parameters.AddWithValue("@copy_recipients", "");
                cmd.Parameters.AddWithValue("@blind_copy_recipients", "");
                cmd.Parameters.AddWithValue("@subject", subject);
                cmd.Parameters.AddWithValue("@body", body);
                cmd.Parameters.AddWithValue("@body_format", "HTML");

                if (!string.IsNullOrEmpty(reply_to))
                    cmd.Parameters.AddWithValue("@reply_to", reply_to);

                try
                {
                    SqlConn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch { }
                finally
                {
                    SqlConn.Close();
                }
            }
        }

        public virtual void Dispose()
        {
            SqlConn.Dispose();
        }
    }
}
