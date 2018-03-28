using Game.DAL;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Manager
{
    public class SysMsgManager
    {
        SysMsgServer sms = new SysMsgServer();

        /// <summary>
        /// 添加一条系统消息
        /// </summary>
        /// <param name="sm">消息</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddSysMsg(sysmsg sm)
        {
            return sms.AddSysMsg(sm);
        }

        /// <summary>
        /// 获取系统消息数量
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <returns>返回消息数量</returns>
        public Double GetSysMsgCount(int UserId)
        {
            return sms.GetSysMsgCount(UserId);
        }

        /// <summary>
        /// 通过分页获取系统消息
        /// </summary>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageNum">页码</param>
        /// <param name="WhereStr">条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns></returns>
        public List<sysmsg> GetAllSysMsg(int PageSize, int PageNum, string WhereStr, string OrderBy)
        {
            return sms.GetAllSysMsg(PageSize, PageNum, WhereStr, OrderBy);
        }

        /// <summary>
        /// 删除系统消息
        /// </summary>
        /// <param name="SysMsgId">消息Id</param>
        /// <returns>返回是否删除成功</returns>
        public Boolean DelSysMsg(int SysMsgId)
        {
            return sms.DelSysMsg(SysMsgId);
        }

    }
}
