using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TMTDentalFPS
{
    public partial class Form1 : Form
    {
        private Button stateNav = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Shown += Form1_Shown;
            //
            stateNav = button1;
            Page1 page1 = new Page1();
            nav(page1, content);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Visible = false;
            Login loginForm = new Login();
            var result = loginForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                Visible = true;
            } else if (result == DialogResult.Cancel) {
                Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Page1 page1 = new Page1();
            nav(page1, content);
            actNav(button1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Page2 page2 = new Page2();
            nav(page2, content);
            actNav(button2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Page3 page3 = new Page3();
            nav(page3, content);
            actNav(button3);
        }

        public void nav(Form form, Panel panel)
        {
            form.TopLevel = false;
            form.Size = panel.Size;
            content.Controls.Clear();
            content.Controls.Add(form);
            form.Show();
        }

        public void actNav(Button button)
        {
            stateNav.BackColor = SystemColors.MenuHighlight;
            stateNav.ForeColor = SystemColors.GradientActiveCaption;
            button.BackColor = SystemColors.ControlLightLight;
            button.ForeColor = SystemColors.ControlText;
            stateNav = button;
        }

    }
}

public static class DataConnect
{
    public static string ip;
    public static string port;
    public static string machineID;
    public static string machineName;
}

public static class DataTablePage1
{
    public static DataTable dataTable;
}
