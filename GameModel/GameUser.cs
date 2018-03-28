using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Model
{
    public class GameUser
    {
        public int Id { get; set; }                                 //流水号Id
        public string UserName { get; set; }                        //用户名
        public string PWD { get; set; }                             //密码
        public string Nick { get; set; }                            //
        public string Sex { get; set; }                             //性别
        public string Phone { get; set; }                           //手机
        public string Tel { get; set; }                             //电话
        public string RealName { get; set; }                        //真实姓名
        public string Email { get; set; }                           //电子邮件
        public string QQ { get; set; }                              //QQ号
        public string BirthDay { get; set; }                        //出生日期
        public string Cards { get; set; }                           //身份证卡
        public string Photo { get; set; }                           //照片
        public int Source { get; set; }                             //来源Id
        public string UserDesc { get; set; }                        //用户描述
        public int IsValiDate { get; set; }                         //
        public int IsValiPhone { get; set; }                        //
        public int IsValiCard { get; set; }                         //
        public int IsLock { get; set; }                             //是否锁定
        public int IsSpreader { get; set; }                         //
        public int GradeId { get; set; }                            //
        public DateTime AddTime { get; set; }                       //注册时间
        public DateTime LastLoginTime { get; set; }                 //最后一次登录时间
        public float Points { get; set; }                           //金币数
        public float Money { get; set; }                            //金钱数
        public float GameMoney { get; set; }                        //消费游戏币
        public float RebateMoney { get; set; }                      //
        public string Ip { get; set; }                              //注册Ip
        public string From_Url { get; set; }                        //推广来源
        public int RegGame { get; set; }                            //推广来源游戏
        public string SpValue { get; set; }                         //
        public string annalID { get; set; }                         //推广来源用户Id

        public GameUser()
        {

        }

        public GameUser(int Id, string UserName, string PWD, string Nick, string Sex, string Phone, string Tel, string RealName, string Email,
            string QQ, string BirthDay, string Cards, string Photo, int Source, string UserDesc, int IsValiDate, int IsValiPhone, int IsValiCard,
            int IsLock, int IsSpreader, int GradeId, DateTime AddTime, DateTime LastLoginTime, float Points, float Money, float GameMoney,
            float RebateMoney, string Ip, string From_Url, int RegGame, string SpValue, string annalID)
        {
            this.Id = Id;
            this.UserName = UserName;
            this.PWD = PWD;
            this.Nick = Nick;
            this.Sex = Sex;
            this.Phone = Phone;
            this.Tel = Tel;
            this.RealName = RealName;
            this.Email = Email;
            this.QQ = QQ;
            this.BirthDay = BirthDay;
            this.Cards = Cards;
            this.Photo = Photo;
            this.Source = Source;
            this.UserDesc = UserDesc;
            this.IsValiDate = IsValiDate;
            this.IsValiPhone = IsValiPhone;
            this.IsValiCard = IsValiCard;
            this.IsLock = IsLock;
            this.IsSpreader = IsSpreader;
            this.GradeId = GradeId;
            this.AddTime = AddTime;
            this.LastLoginTime = LastLoginTime;
            this.Points = Points;
            this.Money = Money;
            this.GameMoney = GameMoney;
            this.RebateMoney = RebateMoney;
            this.Ip = Ip;
            this.From_Url = From_Url;
            this.RegGame = RegGame;
            this.SpValue = SpValue;
            this.annalID = annalID;
        }
    }
}
