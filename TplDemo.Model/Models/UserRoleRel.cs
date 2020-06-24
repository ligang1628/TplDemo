using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Model.Models
{
    public class UserRoleRel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public virtual UserInfo UserInfo { get; set; }
        public virtual UserRole UserRole { get; set; }
    }
}
