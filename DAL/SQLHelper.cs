using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public class SQLHelper
    {
        public SqlConnection conn = null;
        DataTable dt = new DataTable();
        int res;

        public SQLHelper()
        {
            //引用配置文件内容
            string connstr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            conn = new SqlConnection(connstr);
        }

        private SqlConnection GetConn()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            return conn;
        }

        #region 该方法执行带参数的增删改语句或存储过程
        /// <summary>
        /// 该方法执行带参数的增删改语句或存储过程
        /// </summary>
        /// <param name="sql">增删改语句或存储过程</param>
        /// <param name="sp">参数集合</param>
        /// <param name="ct">命令类型</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string cmdText, SqlParameter[] sp, CommandType ct)
        {
            using (SqlCommand cmd = new SqlCommand(cmdText, GetConn()))
            {
                cmd.CommandType = ct;
                cmd.Parameters.AddRange(sp);
                res = cmd.ExecuteNonQuery();
            }
            return res;
        }
        #endregion

        #region 该方法执行不带参数的增删改语句或存储过程
        /// <summary>
        /// 该方法执行不带参数的增删改语句或存储过程
        /// </summary>
        /// <param name="cmdText">sql语句或存储过程</param>
        /// <param name="ct">命令类型</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string cmdText, CommandType ct)
        {
            using (SqlCommand cmd = new SqlCommand(cmdText, GetConn()))
            {
                cmd.CommandType = ct;
                res = cmd.ExecuteNonQuery();
            }
            return res;
        } 
        #endregion

        #region 该方法执行不带参数的SQL查询语句或存储过程
        /// <summary>
        /// 该方法执行不带参数的SQL查询语句或存储过程
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="ct">命令类型</param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string cmdText,CommandType ct)
        { 
            SqlCommand cmd = new SqlCommand(cmdText, GetConn());
            cmd.CommandType = ct;   
            using (SqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                dt.Load(sdr);
            }
            return dt;
        }
        #endregion

        #region 该方法执行带参数的SQL查询语句
        /// <summary>
       /// 该方法执行带参数的SQL查询语句
       /// </summary>
       /// <param name="sql">SQL语句</param>
       /// <param name="sp">参数数组</param>
        /// <param name="ct">命令类型</param>
       /// <returns></returns>
        public DataTable ExecuteQuery(string cmdText, SqlParameter[] sp, CommandType ct)
        {
            SqlCommand cmd = new SqlCommand(cmdText, GetConn());
            cmd.Parameters.AddRange(sp);
            cmd.CommandType = ct;  
            using (SqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                dt.Load(sdr);
            }
            return dt;
        }
        #endregion

    }
}
