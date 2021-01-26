using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZjkBlog.Model;

namespace ZjkBlog.WebApi
{
    public class JwtAppService: IJwtAppService
    {
        private readonly IConfiguration _configuration;
        private List<JwtAuthorizationDto>  _tokens;
        private IDistributedCache _cache;
        private HttpContextAccessor _httpContextAccessor;
        /// <summary>
        /// 新增 Token
        /// </summary>
        /// <param name="user">用户信息数据传输对象</param>
        /// <returns></returns>
        public JwtAuthorizationDto Create(UserRole user)
        {

            Claim[] claims;
            var jwtmodel = _configuration.GetSection(nameof(JwtIssuerOptions));
            var iss = jwtmodel[nameof(JwtIssuerOptions.Issuer)];
            var key = jwtmodel[nameof(JwtIssuerOptions.SecurityKey)];
            var audience = jwtmodel[nameof(JwtIssuerOptions.Audience)];
            DateTime authTime = DateTime.UtcNow;
            DateTime expiresAt = authTime.AddMinutes(Convert.ToDouble(jwtmodel[nameof(JwtIssuerOptions.ExpireMinutes)]));
            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);          
           
            var claimsIdentity = new ClaimsIdentity(new[]{
                        new Claim(ClaimTypes.Name,user.UserModel.Auditor)
                        });        
          
                claims = new[]{
                        new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(authTime).ToUnixTimeSeconds()}") ,
                        new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(expiresAt).ToUnixTimeSeconds()}"),
                        new Claim( "ManageId", user.UserModel.UserName),
                        new Claim(ClaimTypes.Expiration,expiresAt.ToString()),
                        //申明角色
                        new Claim(ClaimTypes.Role,user.Role.Code) };


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
            expires: expiresAt,
            //证书签名
            signingCredentials: creds
            );
            var token = new JwtSecurityTokenHandler().WriteToken(jwttoken);//生成token
            //存储 Token 信息                                                          
            var jwt = new JwtAuthorizationDto
            {
                UserId = user.UserModel.Auditor,
                Token = token,
                Auths = new DateTimeOffset(authTime).ToUnixTimeSeconds(),
                Expires = new DateTimeOffset(expiresAt).ToUnixTimeSeconds(),
                Success = true
            };
            _tokens.Add(jwt);
            return jwt;
        }

        /// <summary>
        /// 停用 Token
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns></returns>
        public async Task DeactivateAsync(string token)
        => await _cache.SetStringAsync(GetKey(token),
                " ", new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"]))
                });

        /// <summary>
        /// 停用当前 Token
        /// </summary>
        /// <returns></returns>
        public async Task DeactivateCurrentAsync()
        => await DeactivateAsync(GetCurrentAsync());

        /// <summary>
        /// 设置缓存中过期 Token 值的 key
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns></returns>
        private static string GetKey(string token)
            => $"deactivated token:{token}";

        /// <summary>
        /// 获取 HTTP 请求的 Token 值
        /// </summary>
        /// <returns></returns>
        private string GetCurrentAsync()
        {
            //http header
            var authorizationHeader = _httpContextAccessor
                .HttpContext.Request.Headers["authorization"];

            //token
            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(" ").Last();// bearer tokenvalue
        }

        /// <summary>
        /// 刷新 Token
        /// </summary>
        /// <param name="token">Token</param>
        /// <param name="dto">用户信息</param>
        /// <returns></returns>
        public async Task<JwtAuthorizationDto> RefreshAsync(string token, UserRole dto)
        {
            var jwtOld = GetExistenceToken(token);
            if (jwtOld == null)
            {
                return new JwtAuthorizationDto()
                {
                    Token = "未获取到当前 Token 信息",
                    Success = false
                };
            }
            var jwt = Create(dto);
            //停用修改前的 Token 信息
            await DeactivateCurrentAsync();
            return jwt;
        }
      
        /// <summary>
        /// 判断是否存在当前 Token
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns></returns>
        private JwtAuthorizationDto GetExistenceToken(string token)
            => _tokens.SingleOrDefault(x => x.Token == token);

        /// <summary>
        /// 判断当前 Token 是否有效
        /// </summary>
        /// <returns></returns>
        public Task<bool> IsCurrentActiveTokenAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 判断 Token 是否有效
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns></returns>
        public Task<bool> IsActiveAsync(string token)
        {
            throw new NotImplementedException();
        }
    }
}
