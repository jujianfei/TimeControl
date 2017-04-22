/*
 *==================
 *创建人：琚建飞
 *创建时间：2017/4/19 11:28:50
 *说明：
 *==================
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using Excel = Microsoft.Office.Interop.Excel;

namespace TimeControl
{
    public partial class frmData : Form
    {
        public frmData()
        {
            InitializeComponent();
        }

        //引用配置文件
        string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        private void frmData_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connStr);
            string sql = "select * from dataRecord";
            SqlDataAdapter ad = new SqlDataAdapter(sql, conn);
            conn.Open();
            ad.Fill(dt);
            dataGridView1.DataSource = dt;

        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            string fileName = "";
            string saveFileName = "";
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = "xlsx";
            saveDialog.Filter = "Excel文件|*.xlsx";
            saveDialog.FileName = fileName;
            saveDialog.ShowDialog();
            saveFileName = saveDialog.FileName;
            if (saveFileName.IndexOf(":") < 0) return; //被点了取消
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            if (xlApp == null)
            {
                MessageBox.Show("无法创建Excel对象，您的电脑可能未安装Excel");
                return;
            }
            Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];//取得sheet1 
            //写入标题             
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            { worksheet.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText; }
            //写入数值
            for (int r = 0; r < dataGridView1.Rows.Count; r++)
            {
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    worksheet.Cells[r + 2, i + 1] = dataGridView1.Rows[r].Cells[i].Value;
                }
                System.Windows.Forms.Application.DoEvents();
            }
            worksheet.Columns.EntireColumn.AutoFit();//列宽自适应
            MessageBox.Show(fileName + "资料保存成功", "提示", MessageBoxButtons.OK);
            if (saveFileName != "")
            {
                try
                {
                    workbook.Saved = true;
                    workbook.SaveCopyAs(saveFileName);  //fileSaved = true;                 
                }
                catch (Exception ex)
                {//fileSaved = false;                      
                    MessageBox.Show("导出文件时出错,文件可能正被打开！\n" + ex.Message);
                }
            }
            xlApp.Quit();
            GC.Collect();//强行销毁    
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string connstr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            SqlConnection conn = new SqlConnection(connstr);

            DateTime dts = Convert.ToDateTime(dtpStart.Text);
            DateTime dte = Convert.ToDateTime(dtpEnd.Text);
            string sql = "select * from dataRecord where datediff(day,@dts,StartTime) >=0 and datediff(day,StartTime,@dte) >=0";
            SqlParameter[] spa = new SqlParameter[]{
                new SqlParameter("@dts", dts),
                new SqlParameter("@dte", dte)
            };            
            conn.Open();
            SqlDataAdapter ad = new SqlDataAdapter(sql, conn);
            ad.SelectCommand.Parameters.AddRange(spa);
            ad.Fill(dt);
            dataGridView1.DataSource = dt;
        }
    }
}
