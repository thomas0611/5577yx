using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Model
{
    public class SourceChange
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Source { get; set; }
        public int SourceChanged { get; set; }
        public DateTime DateChange { get; set; }
        public string Operator { get; set; }

        public SourceChange()
        {

        }

        public SourceChange(int Id, string UserName, int Source, int SourceChanged, DateTime DateChange, string Operator)
        {
            this.Id = Id;
            this.UserName = UserName;
            this.Source = Source;
            this.SourceChanged = SourceChanged;
            this.DateChange = DateChange;
            this.Operator = Operator;
        }
    }
}
