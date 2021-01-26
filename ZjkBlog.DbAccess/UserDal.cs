using Dapper;
using Microsoft.Extensions.Logging;
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
        public int Login(UserModel model)
        {
           
            int i = 0;
            
            StringBuilder strSql = new StringBuilder();        
            DataSet ds = new DataSet();
            try
            {
               
                strSql.Append("select *");
                strSql.Append(" FROM `pub_user` where auditor='"+ model .Auditor+ "' and pwd='"+model.Pwd+"' ");
            
                ds= MysqlHelper.ExecuteDataset(strSql.ToString());
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    i = ds.Tables[0].Rows.Count;
                }
            }
            catch (Exception ex )
            {
                this._logger.LogError("错误！"+ex.Message);
            }
           
            return i;
        }
    }
}
