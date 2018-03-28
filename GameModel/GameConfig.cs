using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Model
{
    public class GameConfig
    {
        public int Id { get; set; }                             //流水号Id
        public int GameId { get; set; }                         //游戏Id
        public string LoginCom { get; set; }                    //登录地址
        public string PayCom { get; set; }                      //充值地址
        public string ExistCom { get; set; }                    //查询地址
        public string LoginTicket { get; set; }                 //登录Key
        public string PayTicket { get; set; }                   //充值Key
        public string SelectTicket { get; set; }                //查询Key
        public string FcmTicket { get; set; }                   //防沉迷Key
        public string AgentId { get; set; }                     //代理商编码

        public GameConfig()
        {

        }

        public GameConfig(int Id, int GameId, string LoginCom, string PayCom, string ExistCom, string LoginTicket, string PayTicket,
            string SelectTicket, string FcmTicket, string AgentId)
        {
            this.Id = Id;
            this.GameId = GameId;
            this.LoginCom = LoginCom;
            this.PayCom = PayCom;
            this.ExistCom = ExistCom;
            this.LoginTicket = LoginTicket;
            this.PayTicket = PayTicket;
            this.SelectTicket = SelectTicket;
            this.FcmTicket = FcmTicket;
            this.AgentId = AgentId;
        }
    }
}
