namespace UE4HTTPListener
{
    partial class AdminPanel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.serverList_groupBox1 = new System.Windows.Forms.GroupBox();
            this.serverList_panel1 = new System.Windows.Forms.Panel();
            this.refreshBTN = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.matchMakingList = new System.Windows.Forms.GroupBox();
            this.mmList_panel = new System.Windows.Forms.Panel();
            this.serverList_groupBox1.SuspendLayout();
            this.matchMakingList.SuspendLayout();
            this.SuspendLayout();
            // 
            // serverList_groupBox1
            // 
            this.serverList_groupBox1.Controls.Add(this.serverList_panel1);
            this.serverList_groupBox1.Location = new System.Drawing.Point(13, 13);
            this.serverList_groupBox1.Name = "serverList_groupBox1";
            this.serverList_groupBox1.Size = new System.Drawing.Size(241, 168);
            this.serverList_groupBox1.TabIndex = 0;
            this.serverList_groupBox1.TabStop = false;
            this.serverList_groupBox1.Text = "Server List";
            // 
            // serverList_panel1
            // 
            this.serverList_panel1.AutoScroll = true;
            this.serverList_panel1.Location = new System.Drawing.Point(6, 19);
            this.serverList_panel1.Name = "serverList_panel1";
            this.serverList_panel1.Size = new System.Drawing.Size(229, 143);
            this.serverList_panel1.TabIndex = 0;
            // 
            // refreshBTN
            // 
            this.refreshBTN.Location = new System.Drawing.Point(19, 187);
            this.refreshBTN.Name = "refreshBTN";
            this.refreshBTN.Size = new System.Drawing.Size(75, 23);
            this.refreshBTN.TabIndex = 2;
            this.refreshBTN.Text = "Refresh List";
            this.refreshBTN.UseVisualStyleBackColor = true;
            this.refreshBTN.MouseClick += new System.Windows.Forms.MouseEventHandler(this.refreshBTN_MouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // matchMakingList
            // 
            this.matchMakingList.Controls.Add(this.mmList_panel);
            this.matchMakingList.Location = new System.Drawing.Point(260, 13);
            this.matchMakingList.Name = "matchMakingList";
            this.matchMakingList.Size = new System.Drawing.Size(340, 168);
            this.matchMakingList.TabIndex = 4;
            this.matchMakingList.TabStop = false;
            this.matchMakingList.Text = "MatchMaking Active Servers";
            // 
            // mmList_panel
            // 
            this.mmList_panel.Location = new System.Drawing.Point(6, 19);
            this.mmList_panel.Name = "mmList_panel";
            this.mmList_panel.Size = new System.Drawing.Size(328, 143);
            this.mmList_panel.TabIndex = 0;
            // 
            // AdminPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 477);
            this.Controls.Add(this.matchMakingList);
            this.Controls.Add(this.refreshBTN);
            this.Controls.Add(this.serverList_groupBox1);
            this.Name = "AdminPanel";
            this.Text = "AdminPanel";
            this.Load += new System.EventHandler(this.AdminPanel_Load);
            this.serverList_groupBox1.ResumeLayout(false);
            this.matchMakingList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private  System.Windows.Forms.GroupBox serverList_groupBox1;
        private  System.Windows.Forms.Panel serverList_panel1;
        private System.Windows.Forms.Button refreshBTN;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.GroupBox matchMakingList;
        private System.Windows.Forms.Panel mmList_panel;
    }
}