using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace ZjkBlog.Dal
{
    internal class ConnectionFactory
    {

        public static DbConnection GetOpenConnection(string connstr)
        {
            var connection = new MySql.Data.MySqlClient.MySqlConnection(connstr);
            connection.Open();
            return connection;

        }
    }

}
