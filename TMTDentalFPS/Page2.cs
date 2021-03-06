﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TMTDentalFPS.Info;
using TMTDentalFPS.Utilities;

namespace TMTDentalFPS
{
    public partial class Page2 : Form
    {
        DeviceManipulator manipulator = new DeviceManipulator();
        public ZkemClient objZkeeper;
        private bool isDeviceConnected = false;
        public Page2()
        {
            InitializeComponent();
            ShowStatusBar(string.Empty, true);
        }

        private void Page2_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = new Collection<UserInfo>();
            SetHeaderText();
            MyUniversalStatic.ChangeGridProperties(dataGridView1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            objZkeeper = new ZkemClient(RaiseDeviceEvent);
            IsDeviceConnected = objZkeeper.Connect_Net(DataConnect.ip, int.Parse(DataConnect.port));

            if (IsDeviceConnected)
            {
                try
                {
                    ShowStatusBar(string.Empty, true);

                    ICollection<UserInfo> lstFingerPrintTemplates = manipulator.GetAllUserInfo(objZkeeper, int.Parse(DataConnect.machineID));
                    if (lstFingerPrintTemplates != null && lstFingerPrintTemplates.Count > 0)
                    {
                        BindToGridView(lstFingerPrintTemplates);
                        ShowStatusBar(lstFingerPrintTemplates.Count + " records found !!", true);
                    }
                    else
                    {
                        //DisplayListOutput("No records found");
                    }
                }
                catch (Exception ex)
                {
                    //DisplayListOutput(ex.Message);
                }
            }
        }

        /// <summary>
        /// Your Events will reach here if implemented
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="actionType"></param>
        private void RaiseDeviceEvent(object sender, string actionType)
        {
            switch (actionType)
            {
                case UniversalStatic.acx_Disconnect:
                    {
                        ShowStatusBar("The device is switched off", true);
                        break;
                    }

                default:
                    break;
            }
        }

        public bool IsDeviceConnected
        {
            get { return isDeviceConnected; }
            set
            {
                isDeviceConnected = value;
                if (isDeviceConnected)
                {
                    ShowStatusBar("The device is connected !!", true);
                }
                else
                {
                    ShowStatusBar("The device is disconnected !!", true);
                    objZkeeper.Disconnect();
                }
            }
        }

        public void ShowStatusBar(string message, bool type)
        {
            if (message.Trim() == string.Empty)
            {
                lblStatus.Visible = false;
                return;
            }

            lblStatus.Visible = true;
            lblStatus.Text = message;
            lblStatus.ForeColor = Color.White;

            if (type)
                lblStatus.BackColor = Color.FromArgb(79, 208, 154);
            else
                lblStatus.BackColor = Color.Tomato;
        }

        private void BindToGridView(object list)
        {
            ClearGrid();
            dataGridView1.DataSource = list;
            SetHeaderText();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            MyUniversalStatic.ChangeGridProperties(dataGridView1);
        }

        private void ClearGrid()
        {
            if (dataGridView1.Controls.Count > 2)
            { dataGridView1.Controls.RemoveAt(2); }


            dataGridView1.DataSource = null;
            dataGridView1.Controls.Clear();
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
        }

        private void SetHeaderText()
        {
            dataGridView1.Columns[0].HeaderText = "Mã Máy";
            dataGridView1.Columns[1].HeaderText = "Mã TK";
            dataGridView1.Columns[2].HeaderText = "Tên TK";
            dataGridView1.Columns[3].HeaderText = "Ngón Tay";
            dataGridView1.Columns[4].HeaderText = "Dữ liệu";
            dataGridView1.Columns[5].HeaderText = "Quyền";
            dataGridView1.Columns[6].HeaderText = "Mật Khẩu";
            dataGridView1.Columns[7].HeaderText = "Trạng thái";
            dataGridView1.Columns[8].HeaderText = "iFlag";
        }
    }
}
