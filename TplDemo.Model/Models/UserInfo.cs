using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TplDemo.Model.Models
{
    public class UserInfo
    {
        public int Id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public bool Sex { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birth { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        public virtual ICollection<UserRoleRel> UserRoleRel { get; set; }
    }
}
