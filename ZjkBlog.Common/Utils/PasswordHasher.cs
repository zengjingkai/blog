using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ZjkBlog.Common
{
    /// <summary>
    /// 密码加密
    /// </summary>
    public class PasswordHasher
    {
        #region 私有方法

        /// <summary>
        /// 加密密码
        /// </summary>
        /// <param name="value">需要加密密码</param>
        /// <param name="salt">盐</param>
        /// <returns></returns>
        private static string HashPassword(string value, string salt)
        {
            var valueBytes = KeyDerivation.Pbkdf2(
                password: value,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);
            return Convert.ToBase64String(valueBytes);
        }       

        /// <summary>
        /// 判断密码是否正确
        /// </summary>
        /// <param name="password">密码</param>
        /// <param name="salt">盐</param>
        /// <param name="hash">加密好的hash值</param>
        /// <returns></returns>
        private static bool Validate(string password, string salt, string hash)
            => HashPassword(password, salt) == hash;

        /// <summary>
        /// 生成随机盐
        /// </summary>
        /// <returns></returns>
        private static string GenerateSalt()
        {
            byte[] randomBytes = new byte[128 / 8];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }
        #endregion

        #region 对外提供，公有方法

        /// <summary>
        /// 对外的密码校验
        /// </summary>
        /// <param name="password">密码</param>
        /// <param name="storePassword">已经加密的密码</param>
        /// <returns></returns>
        public static bool VerifyHashedPassword(string password, string storePassword)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (string.IsNullOrEmpty(storePassword))
            {
                throw new ArgumentNullException(nameof(storePassword));
            }

            var parts = storePassword.Split('.');
            var salt = parts[0];
            var hash = parts[1];

            return Validate(password, salt, hash); ;
        }

        /// <summary>
        /// 加密密码
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string HashPassword(string password)
        {
            var salt = GenerateSalt();
            var hash = HashPassword(password, salt);
            var result = $"{salt}.{hash}";
            Console.WriteLine("hash result:{0}", result);
            return result;
        }

        #endregion

    }
}
