using System;
using System.Collections.Generic;
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
        public int Login(UserModel model);
    }
}
