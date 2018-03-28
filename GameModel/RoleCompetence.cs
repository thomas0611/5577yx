using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Model
{
    public class RoleCompetence
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int CompetenceId { get; set; }

        public RoleCompetence()
        {

        }

        public RoleCompetence(int Id, int RoleId, int CompetenceId)
        {
            this.Id = Id;
            this.RoleId = RoleId;
            this.CompetenceId = CompetenceId;
        }
    }
}
