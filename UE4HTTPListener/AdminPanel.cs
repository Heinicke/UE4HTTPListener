using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UE4HTTPListener
{
    public partial class AdminPanel : Form
    {
        public AdminPanel()
        {
            InitializeComponent();
        }

        private void AdminPanel_Load(object sender, EventArgs e)
        {
            BuildServerList();
            BuildMMList();
        }

        public void refreshServerList()
        {
            serverList_panel1.Controls.Clear();
            mmList_panel.Controls.Clear();
            BuildServerList();
            BuildMMList();
        }

        private void BuildServerList()
        {
            serverList_panel1.Controls.Clear();
            foreach (KeyValuePair<string, string> server in Program.GetServerList())
            {
                TextBox txtB = new TextBox();
                txtB.ReadOnly = true;
                txtB.Text = server.Value + ":" + server.Key;
                txtB.Location = new Point(10, serverList_panel1.Controls.Count * 25);
                Size size = TextRenderer.MeasureText(txtB.Text, txtB.Font);
                txtB.Width = size.Width;
                txtB.Height = size.Height;

                serverList_panel1.Controls.Add(txtB);
            }
        }

        private void BuildMMList()
        {
            mmList_panel.Controls.Clear();
            foreach (KeyValuePair<string, string> mmserver in Program.GetMMServerList())
            {
                TextBox txtB = new TextBox();
                txtB.ReadOnly = true;
                txtB.Text = "ID: " + mmserver.Key + " | Port: " + mmserver.Value;
                txtB.Location = new Point(10, mmList_panel.Controls.Count * 25);
                Size size = TextRenderer.MeasureText(txtB.Text, txtB.Font);
                txtB.Width = size.Width;
                txtB.Height = size.Height;

                mmList_panel.Controls.Add(txtB);
            }
        }

        private void refreshBTN_MouseClick(object sender, MouseEventArgs e)
        {
            refreshServerList();
        }
    }
}
