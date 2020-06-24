using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TplDemo.Model.Models
{
    public class UserRole
    {
        public int Id { get; set; }
        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool Enabled { get; set; }
        public virtual ICollection<UserRoleRel> UserRoleRel { get; set; }
    }
}
