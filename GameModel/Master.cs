using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Model
{
    public class Master
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string RoleType { get; set; }
        public string UserName { get; set; }
        public string UserPWD { get; set; }
        public string UserCzPWD { get; set; }
        public string RealName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int State { get; set; }
        public DateTime AddTime { get; set; }

        public Master()
        {

        }

        public Master(int Id, int RoleId, string RoleType, string UserName, string UserPWD, string UserCzPWD, string RealName, string Phone,
            string Email, int State, DateTime AddTime)
        {
            this.Id = Id;
            this.RoleId = RoleId;
            this.RoleType = RoleType;
            this.UserName = UserName;
            this.UserPWD = UserPWD;
            this.UserCzPWD = UserCzPWD;
            this.RealName = RealName;
            this.Phone = Phone;
            this.Email = Email;
            this.State = State;
            this.AddTime = AddTime;
        }
    }
}
