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
using ZjkBlog.Model;

namespace ZjkBlog.WebApi.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
   // [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // private ILog log;
        UserBusiness bus = new UserBusiness();

        //ILoggerFactory和ILogger都是系统内置的接口，它们两个都可以写日志，随便用哪个都行
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
        [ResponseCache(Duration = 5, Location = ResponseCacheLocation.Any, NoStore = false)]
        public string Login(string Author, string pwd)
        {
            string messgae = string.Empty;
            DateTime n1 = DateTime.Now;
            ResultModel result = new ResultModel();
            result = bus.UserLogin(Author, pwd);
            this._logger.LogInformation("调用方法：UserLogin，时间为：" + (DateTime.Now - n1).TotalSeconds + "秒");
            messgae = result.MESSAGE;
            this._logger.LogInformation(result.MESSAGE);
            return messgae;
        }
        /// <summary>
        /// 获取全部用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ResponseCache(Duration = 5, Location = ResponseCacheLocation.Any, NoStore = false)]
        public string GetAllUser()
        {
            string userList = string.Empty;
            ResultModel result = new ResultModel();
            DateTime n1 = DateTime.Now;
            result = bus.GetAllUser();
            this._logger.LogInformation("调用方法：GetAllUser，时间为：" + (DateTime.Now - n1).TotalSeconds + "秒");
            if (result.ISSUCCESS)
            {
                userList = result.MESSAGE;
            }
            return userList;
        }

        /// <summary>
        /// 新增一个用户
        /// </summary>
        /// <param name="Author"></param>
        /// <returns></returns>     
        [HttpPost]
        [ResponseCache(Duration = 5, Location = ResponseCacheLocation.Any, NoStore = false)]
        public string AddUser(string Author)
        {
            string messgae = string.Empty;
            DateTime n1 = DateTime.Now;
            ResultModel result = new ResultModel();
            result = bus.AddUser(Author);
            this._logger.LogInformation("调用方法：AddUser，时间为：" + (DateTime.Now - n1).TotalSeconds + "秒");
            messgae = result.MESSAGE;
            this._logger.LogInformation(result.MESSAGE);
            return messgae;
        }
    }
}
