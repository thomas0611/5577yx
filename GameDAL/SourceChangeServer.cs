using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Game.Model;
using System.Data;

namespace Game.DAL
{
    public class SourceChangeServer
    {
        DBHelper db = new DBHelper();
        CommonServer cs = new CommonServer();

        /// <summary>
        /// 添加一条来源变更信息
        /// </summary>
        /// <param name="sc">来源变更</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddSourceChange(SourceChange sc)
        {
            try
            {
                string sql = "insert into sourceChange(username,source,source_change,date_change,operator)"
                           + "values (@UserName,@Source,@SourceChange,@Date_change,@Operators)";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@UserName",sc.UserName),
                    new SqlParameter("@Source",sc.Source),
                    new SqlParameter("@SourceChange",sc.SourceChanged), 
                    new SqlParameter("@Date_change", sc.DateChange),
                    new SqlParameter("@Operators",sc.Operator)
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

        /// <summary>
        /// 获取来源变更统计数据条数
        /// </summary>
        /// <param name="WhereStr">条件</param>
        /// <returns>返回数据条数</returns>
        public Double GetSourceChangeCount(string WhereStr)
        {
            return cs.GetDataCount(WhereStr, "vw_SourceChange");
        }

        /// <summary>
        /// 通过分页获取来源变更统计数据
        /// </summary>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageNum">页码</param>
        /// <param name="WhereStr">条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns>返回来源变更统计数据</returns>
        public DataTable GetAllSourceChange(int PageSize, int PageNum, string WhereStr, string OrderBy)
        {
            return cs.GetAllData(PageSize, PageNum, WhereStr, OrderBy, "vw_SourceChange");
        }

        /// <summary>
        /// 删除来源变更统计
        /// </summary>
        /// <param name="SCId">Id</param>
        /// <returns>返回是否删除成功</returns>
        public Boolean DelSourceChange(int SCId) 
        {
            try
            {
                string sql = "delete from sourceChange where id=@Id";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@Id",SCId)
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
