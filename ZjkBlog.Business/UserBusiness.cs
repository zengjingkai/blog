using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ZjkBlog.Common;
using ZjkBlog.Dal;
using ZjkBlog.IDal;
using ZjkBlog.Model;

namespace ZjkBlog.Business
{
    public class UserBusiness
    {
        RedisCommon redis = new RedisCommon();
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool UserLogin(string name,string pwd)
        {
            bool bo = false;
            UserModel model = new UserModel()
            {
                Pwd=pwd,
                Auditor=name
            };  
            int i = new UserDal().Login(model);           
            if (i > 0)
            {
                bo = true;
            }
            else
            {
                bo = false;
            }
            return bo;
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        public string GetAllUser()
        {
            string AllUser = string.Empty;
            IList<UserModel> userlist = new List<UserModel>();
            DataSet ds = new DataSet();
            string Key = "GetAllUser";
            try
            {
                RedisValue value = redis.GetStringKey(Key);
                if (!value.HasValue)
                {
                    ds = new UserDal().GetAllUser();
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        //将查询的datatable转化为对应的Json
                        AllUser = Newtonsoft.Json.JsonConvert.SerializeObject(UtilsHelper.DataTableToList<UserModel>(ds.Tables[0]));
                        redis.StringSet(Key, AllUser);

                    }
                }
                else
                {                   
                    AllUser = value.ToString();
                }
               
            }
            catch (Exception ex)
            {

                throw;
            }
            return AllUser;
        }
    }
}
