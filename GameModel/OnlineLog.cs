using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Model
{
    public class OnlineLog
    {
        public int Id { get; set; }                 //流水号Id
        public int UserId { get; set; }             //用户Id
        public int GameId { get; set; }             //游戏Id
        public int ServerId { get; set; }           //服务器Id
        public DateTime LogTime { get; set; }         //登陆时间
        public int State { get; set; }              //状态
        public int AState { get; set; }             //A状态

        public OnlineLog()
        {

        }

        public OnlineLog(int Id, int UserId, int GameId, int ServerId, DateTime LogTime, int State, int AState)
        {
            this.Id = Id;
            this.UserId = UserId;
            this.GameId = GameId;
            this.ServerId = ServerId;
            this.LogTime = LogTime;
            this.State = State;
            this.AState = AState;
        }
    }
}
