using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ZjkBlog.Common
{
    public static class UtilsHelper
    {
        /// <summary>
        /// 行转化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static T DataRowToObject<T>(DataRow dr) where T : class
        {
            T obj = Activator.CreateInstance<T>();
            string columnName = "";
            foreach (DataColumn dc in dr.Table.Columns)
            {
                columnName = dc.ColumnName;
                try
                {
                    System.Reflection.PropertyInfo pinfo = obj.GetType().GetProperty(columnName);
                    if (pinfo != null)
                    {
                        switch (pinfo.PropertyType.Name.ToLower())
                        {
                            case "string":
                                pinfo.SetValue(obj, dr[columnName].ToString(), null);
                                break;
                            case "double":
                                if (dr[columnName].ToString().Trim() == "")
                                    pinfo.SetValue(obj, 0, null);
                                else
                                    pinfo.SetValue(obj, double.Parse(dr[columnName].ToString()), null);
                                break;
                            case "decimal":
                                if (dr[columnName].ToString() == "")
                                    pinfo.SetValue(obj, 0m, null);
                                else
                                    pinfo.SetValue(obj, decimal.Parse(dr[columnName].ToString()), null);
                                break;
                            case "nullable`1":
                                if (dr[columnName].ToString() == "")
                                    pinfo.SetValue(obj, null, null);
                                else
                                    pinfo.SetValue(obj, dr[columnName], null);
                                break;
                            case "int32":
                                if (dr[columnName].ToString() == "")
                                    pinfo.SetValue(obj, 0, null);
                                else
                                    pinfo.SetValue(obj, int.Parse(dr[columnName].ToString()), null);
                                break;
                            default:
                                pinfo.SetValue(obj, dr[columnName], null);
                                break;
                        }
                    }
                }
                catch
                { }
                columnName = null;
            }
            return obj;
        }
        /// <summary>
        /// DataTable转换为实体类型集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> DataTableToList<T>(DataTable dt) where T : class
        {
            List<T> list = new List<T>();
            if (dt == null || dt.Rows.Count == 0)
                return list;
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(DataRowToObject<T>(dr));
            }
            return list;
        }
        /// <summary>
        /// 字符串转double
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defvalue"></param>
        /// <param name="decimas"></param>
        /// <returns></returns>
        public static double StringToDouble(string value, double defvalue, int decimas)
        {
            if (string.IsNullOrEmpty(value))
                return defvalue;
            double tmp;
            bool flag = double.TryParse(value, out tmp);
            if (flag)
            {
                if (decimas > -1)
                {
                    return (double)Math.Round((Decimal)tmp, decimas);
                }
                else
                {
                    return tmp;
                }
            }
            else
            {
                return defvalue;
            }
        }
        /// <summary>
        /// 字符串转double
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double StringToDouble(string value)
        {
            return StringToDouble(value, 0, -1);
        }
        /// <summary>
        /// 字符串转double
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decimas"></param>
        /// <returns></returns>
        public static double StringToDouble(string value, int decimas)
        {
            return StringToDouble(value, 0, decimas);
        }
        /// <summary>
        /// 字符串转double
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defvalue"></param>
        /// <returns></returns>
        public static double StringToDouble(string value, double defvalue)
        {
            return StringToDouble(value, defvalue, -1);
        }
        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime? StringToDateTime(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            DateTime tmp;
            bool flag = DateTime.TryParse(value, out tmp);
            if (flag)
                return tmp;
            else
                return null;
        }
    }
}
