using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace TimeControl
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        //引用配置文件内容

        string connstr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        private void Form1_Load(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            txtStartTime.Text = dt.ToShortTimeString().ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //1.获取开始时间，学习内容，结束时间
            DateTime st = Convert.ToDateTime(txtStartTime.Text);
            string ct = txtContent.Text;
            DateTime ot = Convert.ToDateTime(cmbHour.Text + ":" + cmbMinute.Text);
            //2.计算总时长
            TimeSpan ts = ot - st;
            int sum = Convert.ToInt32(ts.TotalMinutes);
            //3.将数据写入到数据库中
            //3.1连接数据库
            SqlConnection conn = new SqlConnection(connstr);
            //3.2准备命令对象
            //3.3告诉命令对象要做的事情
            string sql = "insert into dataRecord values(@st,@ct,@ot,@sum) ";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@st", st));
            cmd.Parameters.Add(new SqlParameter("@ct", ct));
            cmd.Parameters.Add(new SqlParameter("@ot", ot));
            cmd.Parameters.Add(new SqlParameter("@sum", sum));
            //3.4打开数据库通道
            conn.Open();
            //3.5执行sql语句
            int result=cmd.ExecuteNonQuery();
            if (result!=0)
            {
                MessageBox.Show("执行成功！","温馨提示");
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            frmData f = new frmData();
            f.ShowDialog();
        }
    }
}
