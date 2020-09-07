using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TMTDentalFPS.Utilities;

namespace TMTDentalFPS
{
    public partial class Page1_Add : Form
    {
        public Page1 MyParentForm;
        public Dictionary<string, string> deviceInfo { get; set; }
        public Dictionary<string, string> connectInfo { get; set; }
        public Dictionary<string, string> userInputInfo { get; set; }
        public Page1_Add()
        {
            InitializeComponent();
            ShowStatusBar(string.Empty, true);
            this.deviceInfo = new Dictionary<string, string>()
            {
                {"Firmware V", ""},
                {"Vendor", ""},
                {"SDK V", ""},
                {"Serial No", ""},
                {"Device MAC", ""},
            };
            this.connectInfo = new Dictionary<string, string>()
            {
                {"IP", ""},
                {"Port", ""},
            };
            this.userInputInfo = new Dictionary<string, string>()
            {
                {"Machine ID", "" },
                {"Company Name", ""},
                {"Machine Name", ""},
                {"Machine Model", ""}
            };
        }

        #region 'Panel Move'
        private bool Drag;
        private int MouseX;
        private int MouseY;

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        private bool m_aeroEnabled;

        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;

        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]

        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
            );

        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();
                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW; return cp;
            }
        }
        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0; DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 1,
                            leftWidth = 0,
                            rightWidth = 0,
                            topHeight = 0
                        }; DwmExtendFrameIntoClientArea(this.Handle, ref margins);
                    }
                    break;
                default: break;
            }
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT) m.Result = (IntPtr)HTCAPTION;
        }
        private void PanelMove_MouseDown(object sender, MouseEventArgs e)
        {
            Drag = true;
            MouseX = Cursor.Position.X - this.Left;
            MouseY = Cursor.Position.Y - this.Top;
        }
        private void PanelMove_MouseMove(object sender, MouseEventArgs e)
        {
            if (Drag)
            {
                this.Top = Cursor.Position.Y - MouseY;
                this.Left = Cursor.Position.X - MouseX;
            }
        }
        private void PanelMove_MouseUp(object sender, MouseEventArgs e) 
        { 
            Drag = false; 
        }
        #endregion

        private void Page1_Add_Load(object sender, EventArgs e)
        {

        }

        private void Page1_Add_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region 'Manipulator Device'
        DeviceManipulator manipulator = new DeviceManipulator();
        public ZkemClient objZkeeper;
        private bool isDeviceConnected = false;

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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tbxMachineName.Text == string.Empty)
            {
                ShowStatusBar("Tên máy is Empty", false);
            }
            else if (tbxIP.Text == string.Empty)
            {
                ShowStatusBar("Địa chỉ IP is Empty", false);
            }
            else if (tbxPort.Text == string.Empty)
            {
                ShowStatusBar("Cổng liên kết TCP is Empty", false);
            } 
            else
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    ShowStatusBar(string.Empty, true);

                    if (IsDeviceConnected)
                    {
                        IsDeviceConnected = false;
                        this.Cursor = Cursors.Default;

                        return;
                    }

                    var ip = tbxIP.Text.Trim();
                    string port = tbxPort.Text.Trim();
                    this.connectInfo["IP"] = ip;
                    this.connectInfo["Port"] = port;

                    if (ip == string.Empty || port == string.Empty)
                        throw new Exception("The Device IP Address and Port is mandotory !!");

                    int portNumber = 4370;
                    if (!int.TryParse(port, out portNumber))
                        throw new Exception("Not a valid port number");

                    bool isValidIpA = UniversalStatic.ValidateIP(ip);
                    if (!isValidIpA)
                        throw new Exception("The Device IP is invalid !!");

                    isValidIpA = UniversalStatic.PingTheDevice(ip);
                    if (!isValidIpA)
                        throw new Exception("The device at " + ip + ":" + port + " did not respond!!");

                    objZkeeper = new ZkemClient(RaiseDeviceEvent);
                    IsDeviceConnected = objZkeeper.Connect_Net(ip, portNumber);

                    if (IsDeviceConnected)
                    {
                        this.userInputInfo["Machine ID"] = ((Page1)MyParentForm).machineID;
                        string deviceInfoString = manipulator.FetchDeviceInfo(objZkeeper, int.Parse(((Page1)MyParentForm).machineID));
                        //
                        deviceInfoString = deviceInfoString.Substring(0, deviceInfoString.Length - 1);
                        string[] temp = deviceInfoString.Split(',');
                        foreach (string s in temp)
                        {
                            if (s.IndexOf("Serial No: ") != -1)
                                this.deviceInfo["Serial No"] = s.Replace("Serial No: ", "");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowStatusBar(ex.Message, false);
                }
                this.Cursor = Cursors.Default;
                if (tbxMachineSeri.Text.Trim() != string.Empty)
                    this.deviceInfo["Serial No"] = tbxMachineSeri.Text.Trim();
                this.userInputInfo["Machine Name"] = tbxMachineName.Text.Trim();
                this.userInputInfo["Company Name"] = tbxCompanyName.Text.Trim();
                this.userInputInfo["Machine Model"] = tbxCompanyName.Text.Trim();
                this.DialogResult = DialogResult.OK;
                this.Close();
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

        private void btnPing_Click(object sender, EventArgs e)
        {
            ShowStatusBar(string.Empty, true);

            string ipAddress = tbxIP.Text.Trim();

            bool isValidIpA = UniversalStatic.ValidateIP(ipAddress);
            if (!isValidIpA)
                throw new Exception("The Device IP is invalid !!");

            isValidIpA = UniversalStatic.PingTheDevice(ipAddress);
            if (isValidIpA)
                ShowStatusBar("The device is active", true);
            else
                ShowStatusBar("Could not read any response", false);
        }
        #endregion

        private void tbxMachineName_Validating(object sender, CancelEventArgs e)
        {
            if (tbxMachineName.Text == string.Empty)
            {
                ShowStatusBar("Tên máy is Empty", false);
            }
        }

        private void tbxIP_Validating(object sender, CancelEventArgs e)
        {
            if (tbxIP.Text == string.Empty)
            {
                ShowStatusBar("Địa chỉ IP is Empty", false);
            }
        }

        private void tbxPort_Validating(object sender, CancelEventArgs e)
        {
            if (tbxPort.Text == string.Empty)
            {
                ShowStatusBar("Cổng liên kết TCP is Empty", false);
            }
        }
    }
}
