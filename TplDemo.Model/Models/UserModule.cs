using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Model.Models
{
    public class UserModule
    {
        public int Id { get; set; }
        public string MId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string ParentId { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Level { get; set; }
        public string Sequence { get; set; }
        public virtual ICollection<UserModuleRel> UserModuleRel { get; set; }
    }
}
