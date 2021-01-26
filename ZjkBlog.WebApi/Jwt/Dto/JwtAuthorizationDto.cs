﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ZjkBlog.WebApi
{
    public class JwtAuthorizationDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Token令牌
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 所經過的秒數
        /// </summary>
        public long Auths { get; set; }
        /// <summary>
        /// 所經過的秒數
        /// </summary>
        public long Expires { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool  Success { get; set; }
    }
}
