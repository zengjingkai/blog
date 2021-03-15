using System;
using System.Collections.Generic;
using System.Text;

namespace ZjkBlog.Model
{
    public class ResultModel
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool ISSUCCESS { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string MESSAGE { get; set; }

        /// <summary>
        /// 错误编码
        /// </summary>
        public string CODE { get; set; }
        /// <summary>
        /// XML
        /// </summary>
        public string Xml { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public ResultModel()
        {
            this.ISSUCCESS = true;
            this.CODE = CommonConstModel.Operate_Result_Code_Success;
            this.MESSAGE = "操作成功";
        }
        /// <summary>
        /// 设置成功信息
        /// </summary>
        /// <param name="MESSAGE">成功描述</param>
        public void SetSuccessInfo(string MESSAGE)
        {
            this.ISSUCCESS = true;
            this.CODE = CommonConstModel.Operate_Result_Code_Success;
            this.MESSAGE = MESSAGE;
        }
        /// <summary>
        /// 设置异常信息
        /// </summary>
        /// <param name="MESSAGE">异常描述</param>
        public void SetException(string MESSAGE)
        {
            this.ISSUCCESS = false;
            this.CODE = CommonConstModel.Operate_Result_Code_Error;
            this.MESSAGE = MESSAGE;
        }
        /// <summary>
        /// 设置异常信息
        /// </summary>
        /// <param name="Ex">异常对象</param>
        public void SetException(Exception Ex)
        {
            this.ISSUCCESS = false;
            this.CODE = CommonConstModel.Operate_Result_Code_Error;
            this.MESSAGE = "操作异常:" + Ex.Message;
        }
    }
}
