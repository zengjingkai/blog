using System;
using System.Collections.Generic;
using System.Text;
using ZjkBlog.Dal;
using ZjkBlog.IDal;
using ZjkBlog.Model;

namespace ZjkBlog.Business
{
    public class UserBusiness
    {
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
    }
}
