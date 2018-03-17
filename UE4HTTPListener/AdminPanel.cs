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
            //Dummy Values for Testing
            /*
            Program.registerServer("127.0.0.1", "7777");
            Program.registerServer("127.0.0.1", "7778");
            Program.registerServer("127.0.0.1", "7779");
            Program.registerServer("127.0.0.1", "7780");
            Program.registerServer("127.0.0.1", "7781");
            BuildServerList();
            //Program.removeServer("7777");
            //Program.removeServer("7781");
            */

            BuildServerList();
        }

        public void refreshServerList()
        {
            serverList_panel1.Controls.Clear();
            BuildServerList();
        }

        private void BuildServerList()
        {
            serverList_panel1.Controls.Clear();
            foreach (KeyValuePair<string, string> server in Program.GetServerList())
            {
                CheckBox chb = new CheckBox();
                chb.Text = server.Value + ":" + server.Key;
                chb.Location = new Point(10, serverList_panel1.Controls.Count * 25);

                serverList_panel1.Controls.Add(chb);
            }
        }

        private void refreshBTN_MouseClick(object sender, MouseEventArgs e)
        {
            refreshServerList();
        }
    }
}
