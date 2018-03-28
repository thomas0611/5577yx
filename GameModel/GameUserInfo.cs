using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Model
{
    public class GameUserInfo
    {
        public string Id { get; set; }                      //游戏商提供的用户唯一Id
        public string UserId { get; set; }                  //用户在我们平台的唯一Id
        public string UserName { get; set; }                //用户在游戏里面的角色名
        public int Level { get; set; }                      //用户在游戏里面的等级
        public string ServerName { get; set; }              //用户所在的区服
        public string Money { get; set; }                   //玩家消费的金额
        public string Message { get; set; }                 //返回消息

        public GameUserInfo()
        {

        }

        public GameUserInfo(string Id, string UserId, string UserName, int Level, string ServerName, string Money,string Message)
        {
            this.Id = Id;
            this.UserId = UserId;
            this.UserName = UserName;
            this.Level = Level;
            this.ServerName = ServerName;
            this.Money = Money;
            this.Message = Message;
        }
    }
}
