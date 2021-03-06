﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ZjkBlog.Common
{
 public class SafeHelper
    {
        private const string StrRegex = @"<[^>]+?style=[\w]+?:expression\(|\b(alert|confirm|prompt)\b|^\+/v(8|9)|<[^>]*?=[^>]*?&#[^>]*?>|\b(and|or)\b.{1,6}?(=|>|<|\bin\b|\blike\b)|/\*.+?\*/|<\s*script\b|<\s*img\b|\bEXEC\b|UNION.+?SELECT|UPDATE.+?SET|INSERT\s+INTO.+?VALUES|(SELECT|DELETE).+?FROM|(CREATE|ALTER|DROP|TRUNCATE)\s+(TABLE|DATABASE)";
        //public static bool PostData()
        //{
        //    bool result = false;
        //    for (int i = 0; i < HttpContext.Current.Request.Form.Count; i++)
        //    {
        //        result = CheckData(HttpContext.Current.Request.Form[i].ToString());
        //        if (result)
        //        {
        //            break;
        //        }
        //    }
        //    return result;
        //}

        //public static bool GetData()
        //{
        //    bool result = false;
        //    for (int i = 0; i < HttpContext.Current.Request.QueryString.Count; i++)
        //    {
        //        result = CheckData(HttpContext.Current.Request.QueryString[i].ToString());
        //        if (result)
        //        {
        //            break;
        //        }
        //    }
        //    return result;
        //}
        //public static bool CookieData()
        //{
        //    bool result = false;
        //    for (int i = 0; i < HttpContext.Current.Request.Cookies.Count; i++)
        //    {
        //        result = CheckData(HttpContext.Current.Request.Cookies[i].Value.ToLower());
        //        if (result)
        //        {
        //            break;
        //        }
        //    }
        //    return result;

        //}
        //public static bool referer()
        //{
        //    bool result = false;
        //    return result = CheckData(HttpContext.Current.Request.UrlReferrer.ToString());
        //}
        /// <summary>
        /// 判断字符串是否存在非法字符
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool CheckData(string inputData)
        {
            if (Regex.IsMatch(inputData, StrRegex))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 判断字符串数组是否存在非法字符
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool CheckData(string [] inputData)
        {
            bool bo = false;
            foreach (var item in inputData)
            {
                if (Regex.IsMatch(item, StrRegex))
                {
                    bo = true;
                }
                else
                {
                    return false;
                }
            }

            return bo;
        }

    }
}
