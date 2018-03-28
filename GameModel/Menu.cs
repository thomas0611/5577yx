using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Model
{
    public class Menu
    {
        public int MenuId { get; set; }
        public int ParentMenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuURL { get; set; }

        public Menu()
        {

        }

        public Menu(int MenuId, int ParentMenuId, string MenuName, string MenuURL)
        {
            this.MenuId = MenuId;
            this.ParentMenuId = ParentMenuId;
            this.MenuName = MenuName;
            this.MenuURL = MenuURL;
        }
    }
}
