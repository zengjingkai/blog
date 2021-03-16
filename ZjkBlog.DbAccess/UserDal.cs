using Dapper;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using ZjkBlog.Common;
using ZjkBlog.IDal;
using ZjkBlog.Model;

namespace ZjkBlog.Dal
{
    public class UserDal : UserIDal
    {
        public ILogger<UserDal> _logger = null;
        public string ConnStr { set; get; }
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataSet Login(UserModel model)
        {         
            StringBuilder strSql = new StringBuilder();
            DataSet ds = new DataSet();
            try
            {
                MySqlParameter[] param = new MySqlParameter[1];
                strSql = strSql.Append(@" SELECT  * FROM pub_user WHERE Auditor=?Auditor");
                param[0] = new MySqlParameter("?Auditor", model.Auditor);
               // param[1] = new MySqlParameter("?Pwd", model.Pwd);
                ds = MysqlHelper.ExecuteDataset(strSql.ToString(), param);
              
            }
            catch (Exception ex)
            {
                this._logger.LogError("错误！" + ex.Message);
            }

            return ds;
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllUser()
        {
            StringBuilder strSql = new StringBuilder();
            DataSet ds = new DataSet();
            try
            {

                strSql.Append("select *  FROM `pub_user`");

                ds = MysqlHelper.ExecuteDataset(strSql.ToString());
            }
            catch (Exception ex)
            {
                this._logger.LogError("错误！" + ex.Message);
            }
            return ds;

        }

        /// <summary>
        /// 新增一个用户
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public DataSet AddUser(UserModel model)
        {
            StringBuilder strSql = new StringBuilder();
            DataSet ds = new DataSet();
            try
            {
                MySqlParameter[] param = new MySqlParameter[8];
                strSql.Append("CALL blog.PROC_SAVE_AddUser (?ACTION,?ID,?UserName,?Sex,?Birthday,?Auditor,?Pwd,?Creator)");
                string action = "ADD";
                model.ID = Guid.NewGuid().ToString("N").ToUpper();
                param[0] = new MySqlParameter("?ACTION", action);
                param[1] = new MySqlParameter("?ID", model.ID);
                param[2] = new MySqlParameter("?UserName", model.UserName);
                param[3] = new MySqlParameter("?Sex", model.Sex);
                param[4] = new MySqlParameter("?Birthday", model.Birthday);
                param[5] = new MySqlParameter("?Auditor", model.Auditor);
                param[6] = new MySqlParameter("?Pwd", model.Pwd);
                param[7] = new MySqlParameter("?Creator", model.Creator);
                ds = MysqlHelper.ExecuteDataset(strSql.ToString(), param);                
            }
            catch (Exception ex)
            {
                this._logger.LogError("错误！" + ex.Message);
            }
            return ds;
        }
    }
}
