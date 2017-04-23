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

        private void Form1_Load(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            txtStartTime.Text = dt.ToShortTimeString().ToString();
        }
        //点击添加按钮
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtContent.Text=="" || txtStartTime.Text=="" || cmbCategory.Text=="" || cmbHour.Text=="" || cmbMinute.Text=="")
            {
                MessageBox.Show("都不能为空哦！");
            }
            else
            {
                //1.获取开始时间，学习内容，结束时间
                DateTime st = Convert.ToDateTime(txtStartTime.Text);
                string ct = txtContent.Text;
                string ca = cmbCategory.Text;
                DateTime ot = Convert.ToDateTime(cmbHour.Text + ":" + cmbMinute.Text);
                //2.计算总时长
                TimeSpan ts = ot - st;
                int sum = Convert.ToInt32(ts.TotalMinutes);
                //3.将数据写入到数据库中
                DAL.DAO dao = new DAL.DAO();
                int result = dao.Insert(st,ct,ca,ot,sum);
                if (result != 0)
                {
                    MessageBox.Show("执行成功！", "温馨提示");
                }
            }
        }
        //查询数据按钮
        private void btnCheck_Click(object sender, EventArgs e)
        {
            frmData f = new frmData();
            f.ShowDialog();
        }
    }
}
