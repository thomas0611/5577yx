using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.DAL;

namespace Game.Manager
{
    public class ActionLockManager
    {
        ActionLockServer als = new ActionLockServer();

        /// <summary>
        /// 检测Action是否被锁定
        /// </summary>
        /// <param name="Action">推广参数</param>
        /// <returns>返回是否锁定</returns>
        public Boolean IsLock(string Action)
        {
            return als.IsLock(Action);
        }

        /// <summary>
        /// 添加一个锁定的Action
        /// </summary>
        /// <param name="Action">推广参数</param>
        /// <param name="Operator">操作者</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddAction(string Action, string Operator)
        {
            return als.AddAction(Action, Operator);
        }

        /// <summary>
        /// 删除锁定的Action
        /// </summary>
        /// <param name="Action">推广参数</param>
        /// <returns>返回是否删除成功</returns>
        public Boolean DelAction(string Action)
        {
            return als.DelAction(Action);
        }
        /// <summary>
        /// 添加一个锁定的Ip
        /// </summary>
        /// <param name="Ip">Ip</param>
        /// <param name="Operator">操作者</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddIp(string Action, string Operator)
        {
            return als.AddIp(Action, Operator);
        }

        /// <summary>
        /// 删除锁定的Action
        /// </summary>
        /// <param name="Ip">Ip</param>
        /// <returns>返回是否删除成功</returns>
        public Boolean DelIp(string Action)
        {
            return als.DelIp(Action);
        }
    }
}
