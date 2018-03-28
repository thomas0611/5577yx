using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Game.DAL
{
    public class CommonServer
    {
        DBHelper db = new DBHelper();

        /// <summary>
        /// 获取数据条数
        /// </summary>
        /// <param name="WhereStr">条件</param>
        /// <param name="TableName">表</param>
        /// <returns>返回数据条数</returns>
        public Double GetDataCount(string WhereStr, string TableName)
        {
            try
            {
                string sql = "select count(*) from " + TableName + " " + WhereStr;
                return db.ExecuteScalar(sql);
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
        /// 通过分页获取数据
        /// </summary>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageNum">页码</param>
        /// <param name="WhereStr">条件</param>
        /// <param name="OrderBy">排序</param>
        /// <param name="TableName">表</param>
        /// <returns>返回数据集合</returns>
        public DataTable GetAllData(int PageSize, int PageNum, string WhereStr, string OrderBy, string TableName)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                new SqlParameter("@PageSize",PageSize),
                new SqlParameter("@PageNum",PageNum),
                new SqlParameter("@TableName",TableName),
                new SqlParameter("@WhereStr",WhereStr),
                new SqlParameter("@OrderBy",OrderBy)
                };
                using (dt = db.GetTableByProc("Proc_Page", sp))
                {

                }
            }
            catch (SqlException ex)
            {
                throw new Exception("数据库异常！原因：" + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("未知异常！原因：" + ex.Message);
            }
            return dt;
        }

    }
}
