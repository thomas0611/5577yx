using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.DAL;
using System.Data;

namespace Game.Manager
{
    public class LockManager
    {
        LockServer ls = new LockServer();

        /// <summary>
        /// 检测是否被锁定
        /// </summary>
        /// <param name="Lock">锁定参数</param>
        /// <returns>返回是否锁定</returns>
        public Boolean IsLock(string Lock)
        {
            return ls.IsLock(Lock);
        }

        /// <summary>
        /// 添加一个锁定的
        /// </summary>
        /// <param name="Lock">锁定参数</param>
        /// <param name="Operator">操作者</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddLock(string Lock, string Operator, string LockInfo)
        {
            return ls.AddLock(Lock, Operator, LockInfo);
        }

        /// <summary>
        /// 删除锁定的Action
        /// </summary>
        /// <param name="Lock">锁定参数</param>
        /// <returns>返回是否删除成功</returns>
        public Boolean DelLock(string Lock)
        {
            return ls.DelLock(Lock);
        }

        /// <summary>
        /// 获取锁定信息条数
        /// </summary>
        /// <returns></returns>
        public Double GetAllLockCount(string WhereStr)
        {
            return ls.GetAllLockCount(WhereStr);
        }

        /// <summary>
        /// 通过分页获取锁定信息
        /// </summary>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageNum">页码</param>
        /// <param name="WhereStr">条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns>返回数据集</returns>
        public DataTable GetAllLock(int PageSize, int PageNum, string WhereStr, string OrderBy)
        {
            return ls.GetAllLock(PageSize, PageNum, WhereStr, OrderBy);
        }
    }
}
