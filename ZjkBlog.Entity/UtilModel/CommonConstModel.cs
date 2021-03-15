using System;
using System.Collections.Generic;
using System.Text;

namespace ZjkBlog.Model
{
    public static class CommonConstModel
    {
        #region 处理结果编码、描述
        /// <summary>
        /// 处理结果编码-处理失败 Operate_Result_Code_Error = "999";
        /// </summary>
        public static readonly string Operate_Result_Code_Error = "999";
        /// <summary>
        /// 处理结果编码-处理成功 Operate_Result_Code_Success = "000";
        /// </summary>
        public static readonly string Operate_Result_Code_Success = "000";

        /// <summary>
        /// 处理结果描述-处理失败 Operate_Result_Desc_Error = "操作失败!";
        /// </summary>
        public static readonly string Operate_Result_Desc_Error = "操作失败!";
        /// <summary>
        /// 处理结果描述-处理成功 Operate_Result_Desc_Success = "操作成功!";
        /// </summary>
        public static readonly string Operate_Result_Desc_Success = "操作成功!";
        #endregion

    }
}
