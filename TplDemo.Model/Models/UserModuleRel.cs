using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Model.Models
{
    public class UserModuleRel
    {
        public Guid UMID { get; set; }
        public int RId { get; set; }
        public int MId { get; set; }
        public bool Status { get; set; }
        public virtual UserModule UserModule { get; set; }
        public virtual UserRole UserRole { get; set; }
    }
}
