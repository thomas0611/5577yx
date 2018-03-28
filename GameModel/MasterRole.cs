using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Model
{
    public class MasterRole
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public MasterRole()
        {

        }

        public MasterRole(int RoleId, string RoleName)
        {
            this.RoleId = RoleId;
            this.RoleName = RoleName;
        }
    }
}
