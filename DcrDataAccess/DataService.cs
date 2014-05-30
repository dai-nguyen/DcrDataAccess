/** 
 * This file is part of the DcrDataAccess project.
 * Copyright (c) 2014 Dai Nguyen
 * Author: Dai Nguyen
**/

using DcrDataAccess.Models;
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
        const string IT_EMAIL = "";

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
                catch { throw; }
                finally
                {
                    SqlConn.Close();
                }
            }
        }

        public void SendErrorEmail(string dcr, SessionInfo info, string error, string notes)
        {
            string body = string.Format(@"
<p>
    DCR: {0} <br />
    Server: {1} <br />
    Db: {2} <br />
    User: {3}
</p>
<p>
    Notes: <br />
    {4}
</p>
<p>
    Error: <br />
    {5}
</p>
", dcr, info.Server, info.Db, info.Server, notes.Replace("\n", "<br />"), error.Replace("\n", "<br />"));

            string subject = string.Format("DCR Error - {0}", dcr);
            this.SendDbMail(new string[] { IT_EMAIL }, "", subject, body);
        }

        public virtual void Dispose()
        {
            SqlConn.Dispose();
        }
    }
}
