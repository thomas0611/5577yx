using Game.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.DAL
{
    public class LotteryServers
    {
        DBHelper db = new DBHelper();

        /// <summary>
        /// 获取最新获奖记录
        /// </summary>
        /// <returns>返回中奖记录集合</returns>
        public List<LotteryLog> GetLotteryLog()
        {
            List<LotteryLog> list = new List<LotteryLog>();
            try
            {
                string sql = "select top 10 * from LotteryLog where LotteryName !='谢谢参与' order by LotteryTime desc";

                using (SqlDataReader reder = db.GetReader(sql))
                {
                    while (reder.Read())
                    {
                        LotteryLog ll = new LotteryLog((int)reder["Id"], reder["UserName"].ToString(), (int)reder["Points"], (int)reder["LotteryNum"], reder["LotteryName"].ToString(), (DateTime)reder["LotteryTime"]);
                        list.Add(ll);
                    }
                }
                return list;
            }
            catch (SqlException ex)
            {
                throw new Exception("数据库异常！原因：" + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("未知异常！原因：" + ex.Message);
            }
            return list;
        }

        /// <summary>
        /// 添加一条中奖日志
        /// </summary>
        /// <param name="ll">中奖日志</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddLotteryLog(LotteryLog ll)
        {
            try
            {
                string sql = "insert into LotteryLog(UserName,Points,LotteryNum,LotteryName)values (@UserName,@Points,@LotteryNum,@LotterName)";
                SqlParameter[] sp = new SqlParameter[] 
                {
                     new SqlParameter("@UserName",ll.UserName),
                    new SqlParameter("@Points",ll.Points),
                    new SqlParameter("@LotteryNum",ll.LotteryNum), 
                    new SqlParameter("@LotterName",ll.LotterName)
                };
                return db.ExecuteNonQuery(sql, sp);
            }
            catch (SqlException ex)
            {
                throw new Exception("数据库异常！原因：" + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("未知异常！原因：" + ex.Message);
            }
        }
    }
}
