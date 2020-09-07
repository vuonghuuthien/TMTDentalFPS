using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TMTDentalFPS.Utilities;

namespace TMTDentalFPS
{
    public partial class Page1 : Form
    {
        public DataTable table { get; set; }
        public string machineID { get; set; }
        public Page1()
        {
            InitializeComponent();
        }

        private void Page1_Load(object sender, EventArgs e)
        {
            // populate datagridview from datatable
            this.table = new DataTable();

            // add columns to datatable
            this.table.Columns.Add("Tên Máy", typeof(string));
            this.table.Columns.Add("Model Máy", typeof(string));
            this.table.Columns.Add("Seri Máy", typeof(string));
            this.table.Columns.Add("Địa Chỉ IP", typeof(string));
            this.table.Columns.Add("Chi Nhánh", typeof(string));
            this.table.Columns.Add("Trạng Thái", typeof(bool));

            dataGridView1.DataSource = this.table;

            MyUniversalStatic.ChangeGridProperties(dataGridView1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Page1_Add page1_Add = new Page1_Add();
            page1_Add.MyParentForm = this;
            this.machineID = (this.table.Rows.Count + 1).ToString();
            if (page1_Add.ShowDialog() == DialogResult.OK)
            {
                Dictionary<string, string> deviceInfo = page1_Add.deviceInfo;
                Dictionary<string, string> connectInfo = page1_Add.connectInfo;
                Dictionary<string, string> userInputInfo = page1_Add.userInputInfo;
                this.table.Rows.Add(
                    userInputInfo["Machine Name"],
                    userInputInfo["Machine Model"],
                    deviceInfo["Serial No"],
                    connectInfo["IP"],
                    userInputInfo["Company Name"],
                    true);
                DataConnect.ip = connectInfo["IP"];
                DataConnect.port = connectInfo["Port"];
                DataConnect.machineID = userInputInfo["Machine ID"];
                DataConnect.machineName = userInputInfo["Machine Name"];
            }
        }

    }
}
