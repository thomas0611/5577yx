using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Model
{
    public class Competence
    {
        public int CompetenceId { get; set; }
        public int ParentCompetenceId { get; set; }
        public string CompetenceName { get; set; }
        public int CompetenceMenu { get; set; }

        public Competence()
        {

        }
        public Competence(int CompetenceId, int ParentCompetenceId, string CompetenceName, int CompetenceMenu)
        {
            this.CompetenceId = CompetenceId;
            this.ParentCompetenceId = ParentCompetenceId;
            this.CompetenceName = CompetenceName;
            this.CompetenceMenu = CompetenceMenu;
        }
    }
}
