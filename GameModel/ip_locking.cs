using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Model
{
    public class ip_locking
    {
        private string ip;
        /// <summary>
        /// IP
        /// </summary>
        public string Ip
        {
            get { return ip; }
            set { ip = value; }
        }

        private DateTime add_datetime;
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime Add_datetime
        {
            get { return add_datetime; }
            set { add_datetime = value; }
        }
        private string operators;
        /// <summary>
        /// 操作员
        /// </summary>
        public string Operators
        {
            get { return operators; }
            set { operators = value; }
        }
    }
}
