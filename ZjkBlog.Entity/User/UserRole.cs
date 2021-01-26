using SqlSugar;
using System;

namespace ZjkBlog.Model
{
    /// <summary>
    /// 用户跟角色关联表
    /// </summary>
    public class UserRole 
    {
        public UserRole() { }

        public UserRole(string uid, string rid)
        {
            UserId = uid;
            RoleId = rid;
            CreateTime = DateTime.Now;
            IsDeleted = false;
            CreateId = uid;
            CreateTime = DateTime.Now;
        }
        /// <summary>
        /// 用户
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string UserId { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string RoleId { get; set; }

        /// <summary>
        ///获取或设置是否禁用，逻辑上的删除，非物理删除
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool? IsDeleted { get; set; }
       
        /// <summary>
        /// 创建ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string CreateId { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        [SugarColumn(ColumnDataType ="nvarchar",Length = 50, IsNullable = true)]
        public string CreateBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 修改ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? ModifyId { get; set; }
        /// <summary>
        /// 修改者
        /// </summary>
        [SugarColumn(ColumnDataType ="nvarchar",Length = 50, IsNullable = true)]
        public string ModifyBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? ModifyTime { get; set; }
        [SugarColumn(IsIgnore = true)]
        public UserModel UserModel { get; set; }
        [SugarColumn(IsIgnore = true)]
        public Role Role { get; set; }
    }
}
