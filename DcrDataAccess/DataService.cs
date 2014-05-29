/** 
 * This file is part of the DcrDataAccess project.
 * Copyright (c) 2014 Dai Nguyen
 * Author: Dai Nguyen
**/

using System;
using System.Data.SqlClient;

namespace DcrDataAccess
{
    public class DataService : IDisposable
    {
        const string SERVER = "";
        const string DB = "";
        const string USER = "";
        const string PASS = "";

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

        public virtual void Dispose()
        {
            SqlConn.Dispose();
        }
    }
}
