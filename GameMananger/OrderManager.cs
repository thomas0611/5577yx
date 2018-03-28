using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Model;
using Game.DAL;
using Common;
using System.Threading;
using System.Data;

namespace Game.Manager
{
    public class OrderManager
    {
        OrdersServers os = new OrdersServers();

        /// <summary>
        /// 生成一个订单
        /// </summary>
        /// <param name="OrderCode">订单标识码</param>
        /// <param name="GameId">游戏Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <param name="Type">订单类型</param>
        /// <param name="PayTypeId">支付类型</param>
        /// <param name="UserName">充值用户</param>
        /// <param name="Money">充值金额(RMB/元)</param>
        /// <param name="AdminUserName">操作员</param>
        /// <returns></returns>
        public Orders GetOrder(string OrderCode, string GameId, int ServerId, int Type, int PayTypeId, string UserName, float Money, string AdminUserName)
        {
            Thread.Sleep(8);
            Orders o = new Orders();
            if (CheckOrder(GameId, ServerId, Type, PayTypeId, UserName, Money, AdminUserName))
            {
                GameServerServers gss = new GameServerServers();
                GamesServers gs = new GamesServers();
                o.Type = Type;
                o.PayTypeId = PayTypeId;
                o.State = 0;
                o.UserName = UserName;
                o.PayTime = DateTime.Now;
                o.Ip = BBRequest.GetIP();
                if (!string.IsNullOrEmpty(AdminUserName))
                {
                    o.AdminUserName = AdminUserName;
                }
                else
                {
                    o.AdminUserName = "用户充值";
                }
                if (PayTypeId == 7 || PayTypeId == 15)
                {
                    o.PMoney = o.PayMoney = Money / 10;
                }
                else
                {
                    o.PMoney = o.PayMoney = Money;
                }
                if (Type == 1)
                {
                    if (GameId.ToString().Length == 1)
                    {
                        GameId = "00" + GameId;
                    }
                    else if (GameId.ToString().Length == 2)
                    {
                        GameId = "0" + GameId;
                    }
                    o.OrderNo = OrderCode + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond + GameId + gss.GetGameServer(ServerId).QuFu;
                    o.GameId = int.Parse(GameId);
                    o.ServerId = ServerId;
                    o.GameName = gs.GetGame(o.GameId).Name;
                    o.ServerName = gss.GetGameServer(ServerId).Name;
                }
                else
                {
                    o.OrderNo = OrderCode + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond + 0 + 0;
                    o.GameId = 0;
                    o.ServerId = 0;
                    o.GameName = "适用所有游戏";
                    o.ServerName = "适用所有区服";
                }
            }
            return o;
        }

        /// <summary>
        /// 添加一笔订单
        /// </summary>
        /// <param name="o">订单</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddOrder(Orders o)
        {
            return os.AddOrder(o);
        }

        /// <summary>
        /// 根据OrderNo获取订单
        /// </summary>
        /// <param name="OrderNo">订单号</param>
        /// <returns>返回订单信息</returns>
        public Orders GetOrder(string OrderNo)
        {
            return os.GetOrder(OrderNo);
        }

        /// <summary>
        /// 根据Id获取订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Orders GetOrder(int id)
        {
            return os.GetOrder(id);
        }

        /// <summary>
        /// 手动修改订单状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public Boolean ChangeOrderState(int id,int state)
        {
            return os.ChangeOrderState(id, state);
        }
        /// <summary>
        /// 获取订单完整信息
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public DataTable GetComOrder(string orderNo)
        {
            return os.GetComOrder(orderNo);
        }

        /// <summary>
        /// 根据汇付宝返回的支付类型更改订单支付类型
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="PayTypeId"></param>
        /// <returns></returns>
        public Boolean EditOrder(string OrderNo,int PayTypeId)
        {
            return os.EditOrder(OrderNo,PayTypeId);
        }

        /// <summary>
        /// 支付成功之后更新订单
        /// </summary>
        /// <param name="OrderNo">订单号</param>
        /// <returns>返回是否成功</returns>
        public Boolean UpdateOrder(string OrderNo)
        {
            return os.UpdateOrder(OrderNo);
        }

        /// <summary>
        /// 验证订单
        /// </summary>
        /// <param name="GameId">游戏Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <param name="Type">充值类型</param>
        /// <param name="PayTypeId">充值方式</param>
        /// <param name="UserName">充值目标账户</param>
        /// <param name="Money">充值金额</param>
        /// <param name="AdminUserName">充值操作用户</param>
        /// <returns></returns>
        public Boolean CheckOrder(string GameId, int ServerId, int Type, int PayTypeId, string UserName, float Money, string AdminUserName)
        {
            GameUserManager gum = new GameUserManager();
            GamesManager gm = new GamesManager();
            if (!string.IsNullOrEmpty(UserName))                                    //验证用户名不能为空
            {
                if (gum.IsGameUser(UserName))                                       //验证用户是否存在
                {
                    if (Type == 1 && ServerId > 0)                                   //验证区是否选择区服
                    {
                        GameUserInfo gui = gm.GetGameUserInfo(int.Parse(GameId), gum.GetGameUser(UserName).Id, ServerId);
                        if (gui.Message == "Success") //验证用户是否在所选区服中存在角色
                        {
                            if (PayTypeId == 6)                                         //验证手动充值必须满足的一些条件
                            {
                                if (Money >= 1 && Money % 1 == 0)                       //订单金额必须大于等于1且必须为1的整数
                                {
                                    return true;
                                }
                                else
                                {
                                    throw new Exception("您输入的金额有误！");
                                }
                            }
                            else if (PayTypeId == 7 )                             //验证平台币充值必须满足的一些条件
                            {
                                if (Money >= 10 && Money % 10 == 0)                     //订单金额必须大于等于10且必须为10的整数
                                {
                                    if (gum.GetGameUser(AdminUserName).Money >= Money)   //玩家平台币余额必须大于等于充值金额
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        throw new Exception("您的余额不足！");
                                    }
                                }
                                else
                                {
                                    throw new Exception("您输入的金额有误！");
                                }
                            }
                            else if (PayTypeId == 15)                             //验证返利币充值必须满足的一些条件
                            {
                                if (Money >= 10 && Money % 10 == 0)                     //订单金额必须大于等于10且必须为10的整数
                                {
                                    if (gum.GetGameUser(AdminUserName).RebateMoney >= Money)   //玩家返利币余额必须大于等于充值金额
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        throw new Exception("您的余额不足！");
                                    }
                                }
                                else
                                {
                                    throw new Exception("您输入的金额有误！");
                                }
                            }
                            else                                                        //验证其他充值方式必须满的一些条件
                            {
                                if (Money >= 5 && Money % 1 == 0)                       //订单金额必须大于等于5且必须为1的整数
                                {
                                    return true;
                                }
                                else
                                {
                                    throw new Exception("您输入的金额有误！");
                                }
                            }
                        }
                        else
                        {
                            throw new Exception(gui.Message);
                        }
                    }
                    else if (Type == 2)
                    {
                        if (PayTypeId == 6)                                         //验证手动充值必须满足的一些条件
                        {
                            if (Money >= 1 && Money % 1 == 0)                       //订单金额必须大于等于1且必须为1的整数
                            {
                                return true;
                            }
                            else
                            {
                                throw new Exception("您输入的金额有误！");
                            }
                        }
                        else                                                        //验证其他充值方式必须满的一些条件
                        {
                            if (Money >= 5 && Money % 1 == 0)                       //订单金额必须大于等于5且必须为1的整数
                            {
                                return true;
                            }
                            else
                            {
                                throw new Exception("您输入的金额有误！");
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("请选择游戏服务器！");
                    }
                }
                else
                {
                    throw new Exception("用户不存在！");
                }
            }
            else
            {
                throw new Exception("用户名不能为空！");
            }
        }

        /// <summary>
        /// 获取用户首次充值
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <returns>返回订单信息</returns>
        public Orders GetFOrder(string UserName)
        {
            return os.GetFOrder(UserName);
        }

        /// <summary>
        /// 获取订单条数
        /// </summary>
        /// <param name="WhereStr">条件</param>
        /// <returns>返回条数</returns>
        public Double GetOrderCount(string WhereStr)
        {
            return os.GetOrderCount(WhereStr);
        }

        /// <summary>
        /// 获取订单分页数据
        /// </summary>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageNum">页码</param>
        /// <param name="WhereStr">条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns>返回订单数据集合</returns>
        public DataTable GetAllOrders(int PageSize, int PageNum, string WhereStr, string OrderBy)
        {
            DataTable dt = new DataTable();
            dt = os.GetAllOrders(PageSize, PageNum, WhereStr, OrderBy);
            foreach (DataRow row in dt.Rows)
            {
                if (string.IsNullOrEmpty(row["Source"].ToString()))
                {
                    row["Source"] = "用户注册";
                }
            }
            return dt;
        }

        /// <summary>
        /// 获取推广员的推广总额
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <returns>返回总金额</returns>
        public Double GetSumMoney(int UserId, string WhereStr)
        {
            return os.GetSumMoney(UserId, WhereStr);
        }

        /// <summary>
        /// 获取推广分析数据条数
        /// </summary>
        /// <param name="WhereStr">条件</param>
        /// <returns>返回数据条数</returns>
        public Double GetPromoCount(string WhereStr)
        {
            return os.GetPromoCount(WhereStr);
        }

        /// <summary>
        /// 获取推广分析分页数据
        /// </summary>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageNum">页码</param>
        /// <param name="WhereStr">条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns>返回数据集</returns>
        public DataTable GetAllPromo(int PageSize, int PageNum, string WhereStr, string OrderBy)
        {
            return os.GetAllPromo(PageSize, PageNum, WhereStr, OrderBy);
        }

        /// <summary>
        /// 获取订单总额
        /// </summary>
        /// <param name="WhereStr">条件</param>
        /// <returns>返回订单总额</returns>
        public Double GetSumMoney(string WhereStr)
        {
            return os.GetSumMoney(WhereStr);
        }

        /// <summary>
        /// 获取推广人数
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <returns>返回推广人数</returns>
        public Double GetSpreadCount(int UserId, Boolean IsUnder)
        {
            return os.GetSpreadCount(UserId, IsUnder);
        }

        /// <summary>
        /// 获取全部推广人数
        /// </summary>
        /// <param name="UserId">用Id</param>
        /// <returns>返回推广人数</returns>
        public Double GetAllSpreadCount(int UserId)
        {
            return os.GetAllSpreadCount(UserId);
        }

        /// <summary>
        /// 获取有效推广人数
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public Double GetAllSpreadCountS(int UserId)
        {
            return os.GetAllSpreadCountS(UserId);
        }

        /// <summary>
        /// 获取推广用户的充值详情总条数
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <returns>返回总条数</returns>
        public Double GetSpreadPayCount(int UserId, string WhereStr)
        {
            string[] time = WhereStr.Split('|');
            return os.GetSpreadPayCount(UserId, time[0], time[1], time[2]);
        }

        /// <summary>
        /// 获取推广用户的角色信息
        /// </summary>
        /// <returns></returns>
        public GameUserInfo GetSpreadUserInfo(int UserId,string WhereStr)
        {
            GameUserInfo gui = new GameUserInfo();
            string[] re = WhereStr.Split('|');
            
            if (!string.IsNullOrEmpty(re[0]))
            {
                int GameId = Convert.ToInt32(re[0]);
                if (!string.IsNullOrEmpty(re[1]))
                {
                    int ServerId = Convert.ToInt32(re[1]);
                    GamesManager gm=new GamesManager();
                    gui=gm.GetGameUserInfo(GameId,UserId ,ServerId);
                    return gui;
                }
                else
                {
                    return gui;
                }
            }
            else
            {
                return gui;
            }
        }

        /// <summary>
        /// 通过分页获取推广付费详情
        /// </summary>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageNum">页码</param>
        /// <param name="UserId">用户Id</param>
        /// <returns>返回订单集合</returns>
        public List<Orders> GetAllSpreadPay(int PageSize, int PageNum, int UserId, string WhereStr)
        {
            string[] time = WhereStr.Split('|');
            return os.GetAllSpreadPay(PageSize, PageNum, UserId, time[0], time[1], time[2]);
        }

        /// <summary>
        /// 获取推广游戏条数
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <returns>返回总条数</returns>
        public Double GetSpreadGameCount(int UserId, string WhereStr)
        {
            string[] re = WhereStr.Split('|');
            WhereStr = "";
            if (!string.IsNullOrEmpty(re[0]))
            {
                WhereStr += " and o.gameid = " + re[0];
            }
            else
            {
                WhereStr += " and 1=1";
            }
            if (string.IsNullOrEmpty(re[1]) || string.IsNullOrEmpty(re[2]))
            {
                WhereStr += " and 1=1";
            }
            else
            {
                WhereStr += " and o.paytime>='" + re[1] + "' and o.paytime<='" + re[2] + "'";
            }
            return os.GetSpreadGameCount(UserId, WhereStr);
        }

        /// <summary>
        /// 获取所有推广游戏
        /// </summary>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageNum">页码</param>
        /// <param name="UserId">用户Id</param>
        /// <returns>返回所有推广游戏集合</returns>
        public Dictionary<string, string> GetAllSpreadGame(int PageSize, int PageNum, int UserId, string WhereStr)
        {
            string[] re = WhereStr.Split('|');
            return os.GetAllSpreadGame(PageSize, PageNum, UserId, re[0], re[1], re[2]);
        }
        /// <summary>
        /// 获取指定服的充值排行
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public DataTable GetSumMoneyOrder(int gameId,int serverId)
        {
            return os.GetSumMoneyOrder(gameId,serverId);
        }
    }
}
