using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZjkBlog.Model
{
    public class BaseModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        private string _ID;
        [Key]
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        /// <summary>
        /// 更新人
        /// </summary>
        [MaxLength(32)]
        private string _UpdateUser;
        public string UpdateUser
        {
            get { return _UpdateUser; }
            set { _UpdateUser = value; }
        }
        /// <summary>
        /// 更新时间
        /// </summary>
        private string _UpdateTime;
        [MaxLength(32)]
        public string UpdateTime
        {
            get { return _UpdateTime; }
            set { _UpdateTime = value; }
        }

        /// <summary>
        /// 创建人
        /// </summary>
        private string _Createor;
        [MaxLength(32)]
        public string Createor
        {
            get { return _Createor; }
            set { _Createor = value; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        private string _CreateTime;
        [MaxLength(32)]
        public string CreateTime
        {
            get { return _CreateTime; }
            set { _CreateTime = value; }
        }
    }
}
