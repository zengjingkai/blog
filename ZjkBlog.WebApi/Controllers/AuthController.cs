using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ZjkBlog.Model;

namespace ZjkBlog.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IDictionary<string, string> users = new Dictionary<string, string>
            {
            { "admin", "1" },
            { "zjk", "2" },
            { "lxl", "lxl" },
            { "james", "james" }
            };
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public string Gettoken([FromBody] JwtLoginRequest request)
        {
            Claim[] claims;
            var jwtmodel = _configuration.GetSection(nameof(JwtIssuerOptions));
            var iss = jwtmodel[nameof(JwtIssuerOptions.Issuer)];
            var key = jwtmodel[nameof(JwtIssuerOptions.SecurityKey)];
            var audience = jwtmodel[nameof(JwtIssuerOptions.Audience)];
            var claimsIdentity = new ClaimsIdentity(new[]{
                    new Claim(ClaimTypes.Name,request.LoginID)
                    });
            if (!users.Any(u => u.Key == request.LoginID && u.Value == request.LoginPwd))
            {
                return "账号或密码错误";
            }
            if ("admin".Equals(request.LoginID))
            {
                 claims = new[]{
                    new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                    new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddMinutes(30)).ToUnixTimeSeconds()}"),
                    new Claim( "ManageId", "admin"),
                    new Claim(ClaimTypes.Role,"admin") };
            }
            else
            {
                 claims = new[]{
                    new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                    new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddMinutes(30)).ToUnixTimeSeconds()}"),
                    new Claim( "ManageId", "user"),
                    new Claim(ClaimTypes.Role,"user") };
            }
            var m5dkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(m5dkey, SecurityAlgorithms.HmacSha256);//生成签名
            var jwttoken = new JwtSecurityToken(
            //颁发者
            issuer: iss,
            //接收者
            audience: audience,
            //参数
            claims: claims,
            //过期时间
            expires: DateTime.Now.AddMinutes(30),
            //证书签名
            signingCredentials: creds
            );
            var token = new JwtSecurityTokenHandler().WriteToken(jwttoken);//生成token
            return token;
        }
    }
}
