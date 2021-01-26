using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZjkBlog.WebApi
{
    /// <summary>
    /// 登录请求类
    /// </summary>
    public class JwtLoginRequest
    {
        [Required(ErrorMessage = "请输入账号")]
        public string LoginID { get; set; }
        [Required(ErrorMessage = "请输入密码")]
        public string LoginPwd { get; set; }
    }
}
