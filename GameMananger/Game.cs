using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Model;

namespace Game.Manager
{
    public interface IGame
    {
        /// <summary>
        /// 游戏登录接口
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <returns>返回游戏登录地址</returns>
        string Login(int UserId, int ServerId,int IsPC);

        /// <summary>
        /// 游戏支付接口
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="ServerId">服务器Id</param>
        /// <param name="Money">支付金额（RMB）</param>
        /// <param name="OrderNo">支付订单</param>
        /// <returns>返回支付结果</returns>
        string Pay(string OrderNo);

        /// <summary>
        /// 游戏查询接口
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <returns>返回查询结果</returns>
        GameUserInfo Sel(int UserId, int ServerId);
    }
}
