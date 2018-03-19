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
        private Timer refreshTimer;
        private int refreshTimer_Interval = 10000;
        private int testServerPID;
        private bool canStart = true;
        public AdminPanel()
        {
            InitializeComponent();
        }

        private void AdminPanel_Load(object sender, EventArgs e)
        {
            BuildServerList();
            BuildMMList();
            BuildProcessList();
            InitRefreshTimer();
        }

        public void refreshServerList()
        {
            serverList_panel1.Controls.Clear();
            mmList_panel.Controls.Clear();
            processList_panel1.Controls.Clear();
            BuildServerList();
            BuildMMList();
            BuildProcessList();
        }

        private void BuildServerList()
        {
            serverList_panel1.Controls.Clear();
            foreach (KeyValuePair<string, string> server in MatchMakingMaster.GetServerList())
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
            foreach (KeyValuePair<string, string> mmserver in MatchMakingMaster.GetMMServerList())
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

        private void BuildProcessList()
        {
            processList_panel1.Controls.Clear();
            foreach (KeyValuePair<int, string> process in MatchMakingMaster.GetProcessList())
            {
                TextBox txtB = new TextBox();
                txtB.ReadOnly = true;
                txtB.Text = "PID: " + process.Key + " | StartDate: " + process.Value;
                txtB.Location = new Point(10, processList_panel1.Controls.Count * 25);
                Size size = TextRenderer.MeasureText(txtB.Text, txtB.Font);
                txtB.Width = size.Width;
                txtB.Height = size.Height;

                processList_panel1.Controls.Add(txtB);
            }
        }

        private void refreshBTN_MouseClick(object sender, MouseEventArgs e)
        {
            refreshServerList();
        }

        public void InitRefreshTimer()
        {
            refreshTimer = new Timer();
            refreshTimer.Tick += new EventHandler(refreshTimer_tick);
            refreshTimer.Interval = refreshTimer_Interval;
            refreshTimer.Start();
        }

        private void refreshTimer_tick(object sender, EventArgs e)
        {
            refreshServerList();
            KillTestServers();
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {

        }

        private void testServerInstance_MouseClick(object sender, MouseEventArgs e)
        {
            if(canStart)
            {
                ServerInstance testServer = new ServerInstance("127.0.0.1", "9999", "12345", "D:\\Troy-Heinicke\\PackagedProjects\\KreavianShooter\\Windows\\WindowsNoEditor\\KreavianShooter\\Binaries\\Win64");
                bool started = testServer.StartServer();
                Console.WriteLine("Server Started: {0} ID: {1}", started, testServer.GetProcessIdOfServer().ToString());
                testServerPID = testServer.GetProcessIdOfServer();
                canStart = false;
            }


            
        }

        private void KillTestServers()
        {
            if(testServerPID != 0)
            {
                MatchMakingMaster.KillServerInstanceByID(testServerPID);
                canStart = true;
                testServerPID = 0;
            }
        }
    }
}
