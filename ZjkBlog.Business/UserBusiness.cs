using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
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
        //在一个类里定义一个接口
        UserIDal iDal=new UserDal();
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public ResultModel UserLogin(string name,string pwd)
        {           
            DataSet ds = new DataSet();
            ResultModel result = new ResultModel();
            try
            {
                UserModel model = new UserModel()
                {
                    Pwd = pwd,
                    Auditor = name
                };
                ds = iDal.Login(model);
                if (DataSetIsOrNotNull(ds))
                {
                    //将查询的datatable转化为对应的Json
                    result.SetSuccessInfo( Newtonsoft.Json.JsonConvert.SerializeObject(UtilsHelper.DataTableToList<UserModel>(ds.Tables[0])[0]));
                }
                else
                {
                    result.SetException("登录失败，请检查登录名或密码！");
                }
            }
            catch (Exception ex)
            {
                result.SetException(ex);
            }
           
            return result;
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        public ResultModel GetAllUser()
        {
            ResultModel result = new ResultModel();
            string AllUser = string.Empty;
            IList<UserModel> userlist = new List<UserModel>();
            DataSet ds = new DataSet();
            string Key = string.Empty;
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                Key = method.Name;
                RedisValue value = redis.GetStringKey(Key);
                if (!value.HasValue)
                {
                    ds = iDal.GetAllUser();
                    if (DataSetIsOrNotNull(ds))
                    {
                        //将查询的datatable转化为对应的Json
                        AllUser = Newtonsoft.Json.JsonConvert.SerializeObject(UtilsHelper.DataTableToList<UserModel>(ds.Tables[0]));
                        redis.StringSet(Key, AllUser);
                        redis.SetExpire(Key,DateTime.Now.AddMinutes(10));
                        result.SetSuccessInfo(AllUser);
                    }
                }
                else
                {
                    result.SetSuccessInfo(value.ToString());
                }
               
            }
            catch (Exception ex)
            {
                result.SetException(ex);
            }
            return result;
        }

        /// <summary>
        /// 增加一个用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ResultModel AddUser(string user)
        {
            ResultModel result = new ResultModel();
            UserModel model = new UserModel();
            DataSet ds = new DataSet();
            try
            {
                if (!string.IsNullOrEmpty(user))
                {
                    //判断该新增用户是否已经存在
                    //如果存在则新增失败，反之则新增成功
                    model = Newtonsoft.Json.JsonConvert.DeserializeObject<UserModel>(user);
                    ds = iDal.AddUser(model);
                    if (DataSetIsOrNotNull(ds))
                    {
                        result = UtilsHelper.DataTableToList<ResultModel>(ds.Tables[0])[0];
                    }
                }
            }
            catch (Exception ex)
            {
                result.SetException(ex);
            }
            return result;
        }
        /// <summary>
        /// 判断DataSet是否为空
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public bool DataSetIsOrNotNull(DataSet ds )
        {
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
