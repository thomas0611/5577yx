using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Model
{
    public class Games
    {
        public int Id { get; set; }                                 //流水号Id
        public string Name { get; set; }                            //游戏名称
        public string GameNo { get; set; }                          //游戏编码
        public string GameListImg { get; set; }                     //游戏图片集合
        public string IndexTjImg { get; set; }                      //首页推荐图片
        public string IndexHbImg { get; set; }                      //首页海报图片
        public string IndexHdImg { get; set; }                      //
        public string HdImg { get; set; }                           //
        public string GameDesc { get; set; }                        //游戏描述
        public string GameCom { get; set; }                         //游戏官网
        public string GameBBS { get; set; }                         //游戏论坛
        public string NewHand { get; set; }                         //新手卡
        public int Is_Top { get; set; }                             //是否制定
        public int Is_Red { get; set; }                             //
        public int Is_Hot { get; set; }                             //是否热门游戏
        public int Is_Slide { get; set; }                           //
        public int Is_Lock { get; set; }                            //是否锁定
        public int Sort_Id { get; set; }                            //
        public DateTime AddTime { get; set; }                       //添加游戏时间
        public int GameMoneyScale { get; set; }                     //游戏币跟人民币的比例
        public string GameMoneyName { get; set; }                   //游戏币名称
        public int IsRole { get; set; }                             //
        public string Pic1 { get; set; }                            //
        public string Pic2 { get; set; }                            //
        public string Pic3 { get; set; }                            //
        public string Pic4 { get; set; }                            //
        public int P1 { get; set; }                                 //
        public int P2 { get; set; }                                 //
        public string GameProperty { get; set; }                    //游戏属性
        public int tjqf { get; set; }                               //推荐区服
        public string game_url_g { get; set; }                      //
        public string game_url_hd { get; set; }                     //
        public string game_url_xzq { get; set; }                    //登录器下载地址

        public Games()
        {

        }

        public Games(int Id, string Name, string GameNo, string GameListImg, string IndexTjImg, string IndexHbImg, string IndexHdImg,
            string HdImg, string GameDesc, string GameCom, string GameBBS, string NewHand, int Is_Top, int Is_Red, int Is_Hot, int Is_Slide,
           int Is_Lock, int Sort_Id, DateTime AddTime, int GameMoneyScale, string GameMoneyName, int IsRole, string Pic1, string Pic2,
            string Pic3, string Pic4, int P1, int P2, string GameProperty, int tjqf, string game_url_g, string game_url_hd, string game_url_xzq)
        {
            this.Id = Id;
            this.Name = Name;
            this.GameNo = GameNo;
            this.GameListImg = GameListImg;
            this.IndexTjImg = IndexTjImg;
            this.IndexHbImg = IndexHbImg;
            this.IndexHdImg = IndexHdImg;
            this.HdImg = HdImg;
            this.GameDesc = GameDesc;
            this.GameCom = GameCom;
            this.GameBBS = GameBBS;
            this.NewHand = NewHand;
            this.Is_Top = Is_Top;
            this.Is_Red = Is_Red;
            this.Is_Hot = Is_Hot;
            this.Is_Slide = Is_Slide;
            this.Is_Lock = Is_Lock;
            this.Sort_Id = Sort_Id;
            this.AddTime = AddTime;
            this.GameMoneyScale = GameMoneyScale;
            this.IsRole = IsRole;
            this.Pic1 = Pic1;
            this.Pic2 = Pic2;
            this.Pic3 = Pic3;
            this.Pic4 = Pic4;
            this.P1 = P1;
            this.P2 = P2;
            this.GameProperty = GameProperty;
            this.tjqf = tjqf;
            this.game_url_g = game_url_g;
            this.game_url_hd = game_url_hd;
            this.game_url_xzq = game_url_xzq;
        }
    }
}
