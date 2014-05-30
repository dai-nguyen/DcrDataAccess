using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DcrDataAccess.Models
{
    public class SessionInfo
    {
        public string Server { get; private set; }
        public string Db { get; private set; }
        public string User { get; private set; }

        public SessionInfo(string server, string db, string user)
        {
            Server = server;
            Db = db;
            User = user;
        }
    }
}
