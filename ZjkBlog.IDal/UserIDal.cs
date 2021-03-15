using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ZjkBlog.Model;

namespace ZjkBlog.IDal
{
    public interface UserIDal
    {
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataSet Login(UserModel model);

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllUser();

        /// <summary>
        /// 新增一个用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataSet AddUser(UserModel model);
    }
}
