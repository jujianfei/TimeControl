using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class DAO
    {
        SQLHelper sqlhelper = new SQLHelper();
        //引用配置文件内容
        string connstr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        #region 往数据库插入一条信息
        /// <summary>
        /// 往数据库插入一条信息
        /// </summary>
        /// <param name="st"></param>
        /// <param name="ct"></param>
        /// <param name="ca"></param>
        /// <param name="ot"></param>
        /// <param name="sum"></param>
        /// <returns></returns>
        public int Insert(DateTime st, string ct, string ca, DateTime ot, int sum)
        {
            SqlParameter[] pa = new SqlParameter[]{
                new SqlParameter("@st", st),
                new SqlParameter("@ct", ct),
                new SqlParameter("@ot", ot),
                new SqlParameter("@sum", sum),
                new SqlParameter("@ca", ca)
            };
            int result = sqlhelper.ExecuteNonQuery("dataRecord_Insert", pa, CommandType.StoredProcedure);
            return result;
        } 
        #endregion

        #region 绑定全部数据
        /// <summary>
        /// 绑定全部信息
        /// </summary>
        /// <returns></returns>
        public DataTable SelectAll()
        {
            return sqlhelper.ExecuteQuery("dataRecord_SelectAll", CommandType.StoredProcedure);
        } 
        #endregion

        #region 根据日期查询相关数据
        /// <summary>
        /// 根据日期查询相关数据
        /// </summary>
        /// <param name="dts">起始日期</param>
        /// <param name="dte">结束日期</param>
        /// <returns></returns>
        public DataTable SelectByDate(DateTime dts, DateTime dte)
        {
            SqlParameter[] spa = new SqlParameter[]{
                new SqlParameter("@dts", dts),
                new SqlParameter("@dte", dte)
            };
            return sqlhelper.ExecuteQuery("dataRecord_SelectByDate",spa, CommandType.StoredProcedure);
        } 
        #endregion

    }
}
