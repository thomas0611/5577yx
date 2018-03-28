using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Model
{
    public class LotteryLog
    {
        public int Id { get; set; }                          //日志Id
        public string UserName { get; set; }                 //抽奖用户
        public int Points { get; set; }                      //消耗积分
        public int LotteryNum { get; set; }                  //中奖奖品号码
        public string LotterName { get; set; }               //奖品名称
        public DateTime LotteryTime { get; set; }            //中奖时间

        public LotteryLog(int Id, string UserName, int Points, int LotteryNum, string LotterName, DateTime LotteryTime)
        {
            this.Id = Id;
            this.UserName = UserName;
            this.Points = Points;
            this.LotterName = LotterName;
            this.LotteryNum = LotteryNum;
            this.LotteryTime = LotteryTime;
        }

        public LotteryLog()
        {

        }
    }
}
