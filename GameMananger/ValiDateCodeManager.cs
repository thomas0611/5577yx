using Game.DAL;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Manager
{
    public class ValiDateCodeManager
    {
        ValiDateCodeServer vdcs = new ValiDateCodeServer();
        /// <summary>
        /// 删除验证码
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="Type">类别</param>
        /// <returns>返回是否删除成功</returns>
        public Boolean DelValiDateCode(int UserId, int Type)
        {
            return vdcs.DelValiDateCode(UserId, Type);
        }

        /// <summary>
        /// 添加验证码
        /// </summary>
        /// <param name="vdc">验证码</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddValiDateCode(validatecode vdc)
        {
            return vdcs.AddValiDateCode(vdc);
        }

        /// <summary>
        /// 获取是否存在验证码
        /// </summary>
        /// <param name="UserId">用Id</param>
        /// <param name="Type">类型</param>
        /// <param name="SendTime">发送时间</param>
        /// <returns>返回是否存在</returns>
        public Boolean ExitValiDateCode(int UserId, int Type, DateTime SendTime)
        {
            return vdcs.ExitValiDateCode(UserId, Type, SendTime);
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="Type">类型</param>
        /// <returns>返回验证码</returns>
        public validatecode GetValiDateCode(int UserId, int Type)
        {
            return vdcs.GetValiDateCode(UserId, Type);
        }
    }
}
