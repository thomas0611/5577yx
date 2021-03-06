﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.DAL;
using Game.Model;
using Common;

namespace Game.Manager
{
    public class Game_Jjsg : IGame
    {
        Games game = new Games();                                           //实例化游戏
        GamesServers games = new GamesServers();                            //实例化获取游戏相关数据
        GameConfig gc = new GameConfig();                                   //实例化游戏参数
        GameConfigServers gcs = new GameConfigServers();                    //实例化获取游戏参数相关数据
        GameServer gs = new GameServer();                                   //实例化游戏服务器
        GameServerServers gss = new GameServerServers();                    //实例化获取游戏服务器相关数据
        GameUser gu = new GameUser();                                       //实例化用户
        GameUserServers gus = new GameUserServers();                        //实例化获取用户相关数据
        Orders order = new Orders();                                        //实例化订单
        OrdersServers os = new OrdersServers();                             //实例化获取订单相关数据
        string tstamp;                                                      //定义时间戳
        string Sign;                                                        //定义验证参数

        /// <summary>
        /// 街机三国登录接口
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <param name="IsPC">是否PC端登陆</param>
        /// <returns>返回登陆地址</returns>
        public string Login(int UserId, int ServerId, int IsPC)
        {
            gu = gus.GetGameUser(UserId);                                   //获取当前登录用户
            gs = gss.GetGameServer(ServerId);                              //获取用户要登录的服务器
            tstamp = Utils.GetTimeSpan();                                   //获取时间戳
            Sign = DESEncrypt.Md5(gu.Id + gc.AgentId + tstamp + gs.ServerNo + gc.LoginTicket, 32);
            string LoginUrl = "http://" + gc.LoginCom + "?user=" + gu.Id + "&time=" + tstamp + "&server_id=" + gs.ServerNo + "&platform=" + gc.AgentId + "&sign=" + Sign + "&non_kid=1&source=&regdate=&backurl=";
            return LoginUrl;
        }

        /// <summary>
        /// 街机三国充值接口
        /// </summary>
        /// <param name="OrderNo">订单号</param>
        /// <returns>返回充值结果</returns>
        public string Pay(string OrderNo)
        {
            order = os.GetOrder(OrderNo);                                   //获取用户的充值订单
            gu = gus.GetGameUser(order.UserName);                           //获取充值用户
            gs = gss.GetGameServer(order.ServerId);                        //获取用户要充值的服务器
            string PayGold = (order.PayMoney * game.GameMoneyScale).ToString();     //计算支付的游戏币
            if (gus.IsGameUser(gu.UserName))                                //判断用户是否属于平台
            {
                tstamp = Utils.GetTimeSpan();                               //获取时间戳
                string sign = DESEncrypt.Md5(gu.Id + gc.AgentId + tstamp + gs.ServerNo + PayGold + order.PayMoney * 100 + OrderNo + gc.PayTicket, 32);
                string PayUrl = "http://" + gc.PayCom + "?user=" + gu.Id + "&pay_amount=" + PayGold + "&amount=" + order.PayMoney * 100 + "&time=" + tstamp + "&order_id=" + OrderNo + "&server_id=" + gs.ServerNo + "&platform=" + gc.AgentId + "&sign=" + sign + "";
                GameUserInfo gui = Sel(gu.Id, gs.Id);                       //获取玩家查询信息
                if (gui.Message == "Success")                               //判断玩家是否存在
                {
                    if (order.State == 1)                                       //判断订单状态是否为支付状态
                    {
                        string PayResult = Utils.GetWebPageContent(PayUrl);     //获取充值结果
                        switch (PayResult)                                      //对充值结果进行解析
                        {
                            case "1":
                                if (os.UpdateOrder(order.OrderNo))              //更新订单状态为已完成
                                {
                                    gus.UpdateGameMoney(gu.UserName, order.PayMoney);     //跟新玩家游戏消费情况
                                    return "充值成功！";
                                }
                                else
                                {
                                    return "充值失败！错误原因：更新订单状态失败！";
                                }
                            case "2":
                                return "充值失败！错误原因：充值的服务器不存在！";
                            case "3":
                                return "充值失败！错误原因：充值金额有误！";
                            case "4":
                                return "充值失败！错误原因：验证参数错误！";
                            case "5":
                                return "充值失败！错误原因：无非提交重复订单！";
                            case "6":
                                return "充值失败！错误原因：不存在的用户！";
                            case "7":
                                return "充值失败！错误原因：充值服务器出错！";
                            case "8":
                                return "充值失败！错误原因：请求订单超时！";
                            default:
                                return "充值失败！未知错误！";
                        }
                    }
                    else
                    {
                        return "充值失败！错误原因：无法提交未支付订单！";
                    }
                }
                else
                {
                    return gui.Message;
                }
            }
            else
            {
                return "充值失败！用户不存在！";
            }
        }

        /// <summary>
        /// 街机三国查询接口
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <returns>返回查询结果</returns>
        public GameUserInfo Sel(int UserId, int ServerId)
        {
            gu = gus.GetGameUser(UserId);                                   //获取查询用户
            gs = gss.GetGameServer(ServerId);                              //获取查询用户所在区服
            tstamp = Utils.GetTimeSpan();                                   //获取时间戳
            GameUserInfo gui = new GameUserInfo();                          //定义返回查询结果信息  
            Sign = DESEncrypt.Md5(gu.Id + gc.AgentId + tstamp + gs.ServerNo + gc.LoginTicket, 32);              //获取验证参数
            string SelUrl = "http://" + gc.ExistCom + "?user=" + gu.Id + "&platform=" + gc.AgentId + "&time=" + tstamp + "&server_id=" + gs.ServerNo + "&sign=" + Sign + "";
            try
            {
                string SelResult = Utils.GetWebPageContent(SelUrl);         //获取查询结果
                switch (SelResult)                                          //对查询结果进行解析
                {
                    case "1":
                        gui.Message = "查询失败！用户参数错误！";
                        break;
                    case "2":
                        gui.Message = "查询失败！服务器不存在！";
                        break;
                    case "3":
                        gui.Message = "查询失败！用户不存在！";
                        break;
                    case "4":
                        gui.Message = "查询失败！验证参数错误！";
                        break;
                    case "5":
                        gui.Message = "查询失败！无效的请求！";
                        break;
                    case "6":
                        gui.Message = "查询失败！请求超时！";
                        break;
                    case "7":
                        gui.Message = "查询失败！无效的平台标识！";
                        break;
                    default:
                        SelResult = SelResult.Substring(0, SelResult.IndexOf('}'));                      //处理返回结果
                        SelResult = SelResult.Replace(SelResult.Substring(0, SelResult.LastIndexOf('{') + 1), "");
                        string[] b = SelResult.Split(',');
                        gui = new GameUserInfo(b[0].Substring(7).Replace("\"", ""), gu.UserName, Utils.ConvertUnicodeStringToChinese(b[1].Substring(11).Replace("\"", "")), int.Parse(b[2].Substring(8).Replace("\"", "")), gs.Name, os.GetOrderInfo(gu.UserName), "Success");
                        break;
                }
            }
            catch (Exception)
            {
                gui.Message = "查询失败！查询不到用户信息！";
            }
            return gui;
        }

        /// <summary>
        /// 实例化接口参数
        /// </summary>
        public Game_Jjsg()
        {
            game = games.GetGame("jjsg");                                   //获取游戏
            gc = gcs.GetGameConfig(game.Id);                                //获取游戏参数
        }
    }
}
