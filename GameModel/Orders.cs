using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Model
{
    public class Orders
    {
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public int Type { get; set; }
        public int PayTypeId { get; set; }
        public int State { get; set; }
        public string UserName { get; set; }
        public float PMoney { get; set; }
        public float PayMoney { get; set; }
        public DateTime PayTime { get; set; }
        public string Ip { get; set; }
        public int GameId { get; set; }
        public int ServerId { get; set; }
        public string GameName { get; set; }
        public string ServerName { get; set; }
        public string Phone { get; set; }
        public float RebateNum { get; set; }
        public int RebateId { get; set; }
        public int ConvertId { get; set; }
        public string AdminUserName { get; set; }

        public Orders()
        {

        }

        public Orders(int Id, string OrderNo, int Type, int PayTypeId, int State, string UserName, float PMoney, float PayMoney, DateTime PayTime,
            string Ip, int GameId, int ServerId, string GameName, string ServerName, string Phone, float RebateNum, int RebateId, int ConvertId,
            string AdminUserName)
        {
            this.Id = Id;
            this.OrderNo = OrderNo;
            this.Type = Type;
            this.PayTypeId = PayTypeId;
            this.State = State;
            this.UserName = UserName;
            this.PMoney = PMoney;
            this.PayMoney = PayMoney;
            this.PayTime = PayTime;
            this.Ip = Ip;
            this.GameId = GameId;
            this.ServerId = ServerId;
            this.GameName = GameName;
            this.ServerName = ServerName;
            this.Phone = Phone;
            this.RebateNum = RebateNum;
            this.RebateId = RebateId;
            this.ConvertId = ConvertId;
            this.AdminUserName = AdminUserName;
        }
    }
}
