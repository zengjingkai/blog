﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ZjkBlog.WebApi
{
    public class JwtIssuerOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string ValidFor { get; set; }
        public string ValidAudience { get; set; }
        public string SecurityKey { get; set; }
        public string ExpireMinutes { get; set; }

        
    }
}
