using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ZjkBlog.Common
{
    public class MysqlHelper
    { /// <summary>
      /// 连接字符串
      /// </summary>
        public static string conf = AppConfigurtaionServices.Configuration["Connection:DbConnectionString"];
        /// <summary>
        /// 返回首行首列
        /// </summary>
        /// <param name="sqltext"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string sqltext)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                MySqlCommand comm = new MySqlCommand(sqltext, conn);
                return comm.ExecuteScalar();
            }
        }
        /// <summary>
        /// 返回受影响行数
        /// </summary>
        /// <param name="sqltext"></param>
        /// <returns></returns>
        public static int ExecuteNoQuery(string sqltext)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                MySqlCommand comm = new MySqlCommand(sqltext, conn);
                return comm.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// 返回受影响行数
        /// </summary>
        /// <param name="sqltext"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(string sqltext)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(sqltext, conf);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }
        /// <summary>
        /// 返回dataset 传入sqlparameter
        /// </summary>
        /// <param name="sqltext"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(string sqltext, MySqlParameter[] param)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(sqltext, conn);
                //adapter.SelectCommand.Connection = conn;
                adapter.SelectCommand.CommandType = CommandType.Text;
                //  adapter.SelectCommand.CommandText = sqltext;
                adapter.SelectCommand.Parameters.AddRange(param);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }

       
    }

}

