using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Model
{
    public class GameServer
    {
        public int Id { get; set; }                             //流水号Id 
        public int GameId { get; set; }                         //游戏Id
        public string ServerNo { get; set; }                    //服务器编号
        public string QuFu { get; set; }                        //服务器区服
        public string Name { get; set; }                        //服务器名称
        public string Img { get; set; }                         //服务器图片
        public string ServerDesc { get; set; }                  //服务器描述
        public string Line { get; set; }                        //服务器线路
        public int State { get; set; }                          //服务器状态
        public DateTime StartTime { get; set; }                 //开服时间
        public int Sort_Id { get; set; }                        //排序

        public GameServer()
        {

        }

        public GameServer(int Id, int GameId, string ServerNo, string QuFu, string Name, string Img, string ServerDesc, string Line, int State,
            DateTime StartTime, int Sort_Id)
        {
            this.Id = Id;
            this.GameId = GameId;
            this.ServerNo = ServerNo;
            this.QuFu = QuFu;
            this.Name = Name;
            this.Img = Img;
            this.ServerDesc = ServerDesc;
            this.Line = Line;
            this.State = State;
            this.StartTime = StartTime;
            this.Sort_Id = Sort_Id;
        }
    }
}
