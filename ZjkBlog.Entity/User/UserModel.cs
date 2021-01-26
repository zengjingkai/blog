using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZjkBlog.Model
{
    public class UserModel:BaseModel
    {
        //public UserModel(UserModel us)
        //{
        //    //this.Auditor = us.Auditor;
        //    //this.UserName = us.UserName;
        //    //this.Sex = us.Sex;
        //    //this.Birthday = us.Birthday;
        //    //this.Pwd = us.Pwd;;
        //}

        public UserModel()
        {

        }
        private string _UserName;
        private int _Sex;
        private string _Birthday;
        private string _Auditor;
        private string _Pwd;
        private Func<object, object> p;

        public UserModel(Func<object, object> p)
        {
            this.p = p;     
        }

        /// <summary>
        /// 用户名
        /// </summary>
        [MaxLength(60)]
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        /// <summary>
        /// 性别：0，保密；1，男；2，女
        /// </summary>
        [MaxLength(2)]
        public int Sex
        {
            get { return _Sex; }
            set { _Sex = value; }
        }

        /// <summary>
        /// 生日
        /// </summary>
        [MaxLength(30)]
        public string Birthday
        {
            get { return _Birthday; }
            set { _Birthday = value; }
        }

        /// <summary>
        /// 登录名
        /// </summary>
        [MaxLength(30)]
        public string Auditor
        {
            get { return _Auditor; }
            set { _Auditor = value; }
        }

        /// <summary>
        /// 密码
        /// </summary>
        [MaxLength(60)]
        public string Pwd
        {
            get { return _Pwd; }
            set { _Pwd = value; }
        }

    }
}
