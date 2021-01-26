using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZjkBlog.Business;

namespace ZjkBlog.WebApi.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        // private ILog log;
        UserBusiness bus = new UserBusiness();

        //ILoggerFactory和ILogger都是系统内置的接口，它们两个都可以写日志，随便你用哪个都行
        public ILoggerFactory _Factory = null;
        public ILogger<UserController> _logger = null;

        //注意：ILoggerFactory的命名空间是Microsoft.Extensions.Logging;
        public UserController(ILoggerFactory factory, ILogger<UserController> logger)
        {
            this._Factory = factory;
            this._logger = logger;
        }
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="Author"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>     
        [HttpGet]
        public string Login(string Author, string pwd)
        {
            bool bo = false;
            DateTime n1 = DateTime.Now;
            bo = bus.UserLogin(Author, pwd);        
            TimeSpan ts = DateTime.Now - n1;
            this._logger.LogInformation("调用方法：UserLogin，时间为："+ ts.TotalSeconds+"秒");
            if (!bo)
            {
                this._logger.LogInformation("账号或密码错误，登陆失败");
            }
            return bo ? "登陆成功" : "账号或密码错误，登陆失败";
        }
        /// <summary>
        /// 获取全部用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetAllUser()
        {
            return "GetAllUser";
        }
    }
}
