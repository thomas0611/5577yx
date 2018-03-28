using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Common;

namespace Game.DAL
{
    public class DBHelper
    {
        private static readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();      //从配置文件获取连接字符串
        public LogHelper logHelper = new LogHelper();
        private SqlConnection conn;         //定义私有的连接字段

        public SqlConnection Conn           //定义连接属性
        {
            get
            {
                if (conn == null)
                {
                    conn = new SqlConnection(ConnStr);
                }
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                if (conn.State == ConnectionState.Broken)
                {
                    conn.Close();
                    conn.Open();
                }
                return conn;

            }
        }

        /// <summary>
        /// 定义查询的无参数化方法
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns>返回SqlDataReader</returns>
        public SqlDataReader GetReader(string sql)
        {
            SqlCommand comm = new SqlCommand(sql, Conn);
            SqlDataReader dr = comm.ExecuteReader(CommandBehavior.CloseConnection);
            logHelper.WriteLog(this.GetType(), sql);
            return dr;
        }

        /// <summary>
        /// 定义查询的参数化方法
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="sp">参数化参数</param>
        /// <returns>返回SqlDataReader</returns>
        public SqlDataReader GetReader(string sql, SqlParameter[] sp)
        {
            SqlCommand comm = new SqlCommand(sql, Conn);
            comm.Parameters.AddRange(sp);
            SqlDataReader dr = comm.ExecuteReader(CommandBehavior.CloseConnection);
            logHelper.WriteLog(this.GetType(), comm.CommandText);
            return dr;
            
        }

        /// <summary>
        /// 定义查询的无参数化方法
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns>返回DataTable</returns>
        public DataTable GetTable(string sql)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, Conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Conn.Close();
            logHelper.WriteLog(this.GetType(), sql);
            return dt;

        }


        /// <summary>
        /// 定义查询的参数化方法
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="sp">参数化参数</param>
        /// <returns>返回DataTable</returns>
        public DataTable GetTable(string sql, SqlParameter[] sp)
        {
            DataTable dt = new DataTable();
            SqlCommand comm = new SqlCommand(sql, Conn);
            comm.Parameters.AddRange(sp);
            SqlDataAdapter da = new SqlDataAdapter(comm);
            da.Fill(dt);
            Conn.Close();
            logHelper.WriteLog(this.GetType(), sql);
            return dt;
        }

        /// <summary>
        /// 定义增删改无参数化方法
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns>返回是执行语句是否成功</returns>
        public Boolean ExecuteNonQuery(string sql)
        {
            SqlCommand comm = new SqlCommand(sql, Conn);
            int count = comm.ExecuteNonQuery();
            Conn.Close();
            return count > 0;
        }

        /// <summary>
        /// 定义增删改参数化方法
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="sp">参数化参数</param>
        /// <returns>返回执行语句是否成功</returns>
        public Boolean ExecuteNonQuery(string sql, SqlParameter[] sp)
        {
            SqlCommand comm = new SqlCommand(sql, Conn);
            comm.Parameters.AddRange(sp);
            int count = comm.ExecuteNonQuery();
            Conn.Close();
            return count > 0;
        }

        /// <summary>
        /// 定义执行聚合函数的无参数化方法
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns>返回执行聚会函数结果</returns>
        public Double ExecuteScalar(string sql)
        {
            SqlCommand comm = new SqlCommand(sql, Conn);
            object result = comm.ExecuteScalar();
            Conn.Close();
            return Convert.ToDouble(result);
        }

        /// <summary>
        /// 定义执行聚合函数的参数化方法
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="sp">参数化参数</param>
        /// <returns>返回执行聚合函数结果</returns>
        public Double ExecuteScalar(string sql, SqlParameter[] sp)
        {
            SqlCommand comm = new SqlCommand(sql, Conn);
            comm.Parameters.AddRange(sp);
            object result = comm.ExecuteScalar();
            Conn.Close();
            return Convert.ToDouble(result);
        }

        /// <summary>
        /// 定义执行存储过程的做聚合函数的无参数化方法
        /// </summary>
        /// <param name="proc">存储过程</param>
        /// <returns></returns>
        public Double ExecuteScalarByProc(string proc)
        {
            SqlCommand comm = new SqlCommand(proc, Conn);
            comm.CommandType = CommandType.StoredProcedure;
            object result = comm.ExecuteScalar();
            Conn.Close();
            return Convert.ToDouble(result);
        }

        /// <summary>
        /// 定义支持存储过程做聚合函数的参数化方法
        /// </summary>
        /// <param name="proc">聚合函数</param>
        /// <param name="sp">参数化参数</param>
        /// <returns></returns>
        public Double ExecuteScalarByProc(string proc, SqlParameter[] sp)
        {
            SqlCommand comm = new SqlCommand(proc, Conn);
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.AddRange(sp);
            object result = comm.ExecuteScalar();
            Conn.Close();
            return Convert.ToDouble(result);
        }

        /// <summary>
        /// 定义执行存储过程做查询无参数化的方法
        /// </summary>
        /// <param name="proc">存储过程方法</param>
        /// <returns>返回SqlDataReader</returns>
        public SqlDataReader GetReaderByProc(string proc)
        {
            SqlCommand comm = new SqlCommand(proc, Conn);
            comm.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = comm.ExecuteReader(CommandBehavior.CloseConnection);
            return dr;
        }

        /// <summary>
        /// 定义执行存储过程做查询参数化的方法
        /// </summary>
        /// <param name="proc">存储过程方法</param>
        /// <param name="sp">参数化参数</param>
        /// <returns>返回SqlDataReader</returns>
        public SqlDataReader GetReaderByProc(string proc, SqlParameter[] sp)
        {
            SqlCommand comm = new SqlCommand(proc, Conn);
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.AddRange(sp);
            SqlDataReader dr = comm.ExecuteReader(CommandBehavior.CloseConnection);
            return dr;
        }

        /// <summary>
        /// 定义执行存储过程做查询无参数化的方法
        /// </summary>
        /// <param name="proc"></param>
        /// <returns>返回DataTable</returns>
        public DataTable GetTableByProc(string proc)
        {
            SqlCommand comm = new SqlCommand(proc, Conn);
            comm.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(comm);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;
        }

        /// <summary>
        /// 定义执行存储过程做查询参数化的方法
        /// </summary>
        /// <param name="proc">存储过程方法</param>
        /// <param name="sp">参数化参数</param>
        /// <returns>返回DataTable</returns>
        public DataTable GetTableByProc(string proc, SqlParameter[] sp)
        {
            SqlCommand comm = new SqlCommand(proc, Conn);
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.AddRange(sp);
            SqlDataAdapter da = new SqlDataAdapter(comm);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;
        }

        /// <summary>
        /// 定义执行存储过程做增删改无参数化方法
        /// </summary>
        /// <param name="proc">存储过程方法</param>
        /// <returns>放回执行存储过程是否成功</returns>
        public Boolean ExecuteNonQueryByProc(string proc)
        {
            SqlCommand comm = new SqlCommand(proc, Conn);
            comm.CommandType = CommandType.StoredProcedure;
            int count = comm.ExecuteNonQuery();
            Conn.Close();
            return count > 0;
        }

        /// <summary>
        /// 定义执行存储过程做增删改参数化方法
        /// </summary>
        /// <param name="proc">存储过程方法</param>
        /// <param name="sp">参数化参数</param>
        /// <returns>返回执行存储过程是否成功</returns>
        public Boolean ExecuteNonQueryByProc(string proc, SqlParameter[] sp)
        {
            SqlCommand comm = new SqlCommand(proc, Conn);
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.AddRange(sp);
            int count = comm.ExecuteNonQuery();
            Conn.Close();
            return count > 0;
        }

        /// <summary>
        /// 定义执行事物的无参数化方法  
        /// </summary>
        /// <param name="transName">事物名称</param>
        /// <param name="sql">Slq语句</param>
        /// <returns>返回执行事物是否成功</returns>
        public Boolean Transaction(string transName, string sql)
        {
            SqlTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction(transName);
                SqlCommand comm = new SqlCommand(sql, conn, trans);
                comm.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch (Exception)
            {
                trans.Rollback(transName);
                return false;
            }
        }

        /// <summary>
        /// 定义执行事物的参数化方法  
        /// </summary>
        /// <param name="transName">事物名称</param>
        /// <param name="sql">Sql语句</param>
        /// <param name="sp">参数化参数</param>
        /// <returns>返回执行事物是否成功</returns>
        public Boolean Transaction(string transName, string sql, SqlParameter[] sp)
        {
            SqlTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction(transName);
                SqlCommand comm = new SqlCommand(sql, conn, trans);
                comm.Parameters.AddRange(sp);
                comm.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch (Exception)
            {
                trans.Rollback(transName);
                return false;
            }
        }
    }
}
