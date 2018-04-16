namespace Hackney.ConfigEncryptor
{
    partial class FormCompare
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.richTextBoxLocal = new System.Windows.Forms.RichTextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelUseLocalConfig = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelLocalConfigFilename = new System.Windows.Forms.Label();
            this.richTextBoxServer = new System.Windows.Forms.RichTextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.labelUseServerConfig = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelServerConfigFilename = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.richTextBoxLocal);
            this.splitContainer1.Panel1.Controls.Add(this.panel3);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.richTextBoxServer);
            this.splitContainer1.Panel2.Controls.Add(this.panel4);
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Size = new System.Drawing.Size(898, 620);
            this.splitContainer1.SplitterDistance = 434;
            this.splitContainer1.TabIndex = 1;
            // 
            // richTextBoxLocal
            // 
            this.richTextBoxLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxLocal.Location = new System.Drawing.Point(0, 24);
            this.richTextBoxLocal.Name = "richTextBoxLocal";
            this.richTextBoxLocal.Size = new System.Drawing.Size(434, 496);
            this.richTextBoxLocal.TabIndex = 2;
            this.richTextBoxLocal.Text = "";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.labelUseLocalConfig);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 520);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(0, 0, 0, 20);
            this.panel3.Size = new System.Drawing.Size(434, 100);
            this.panel3.TabIndex = 1;
            // 
            // labelUseLocalConfig
            // 
            this.labelUseLocalConfig.AutoSize = true;
            this.labelUseLocalConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelUseLocalConfig.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUseLocalConfig.Location = new System.Drawing.Point(80, 35);
            this.labelUseLocalConfig.Name = "labelUseLocalConfig";
            this.labelUseLocalConfig.Size = new System.Drawing.Size(221, 31);
            this.labelUseLocalConfig.TabIndex = 0;
            this.labelUseLocalConfig.Text = "Use Local Config";
            this.labelUseLocalConfig.Click += new System.EventHandler(this.labelUseLocalConfig_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelLocalConfigFilename);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(434, 24);
            this.panel1.TabIndex = 0;
            // 
            // labelLocalConfigFilename
            // 
            this.labelLocalConfigFilename.AutoSize = true;
            this.labelLocalConfigFilename.Location = new System.Drawing.Point(4, 5);
            this.labelLocalConfigFilename.Name = "labelLocalConfigFilename";
            this.labelLocalConfigFilename.Size = new System.Drawing.Size(66, 13);
            this.labelLocalConfigFilename.TabIndex = 0;
            this.labelLocalConfigFilename.Text = "Local Config";
            // 
            // richTextBoxServer
            // 
            this.richTextBoxServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxServer.Location = new System.Drawing.Point(0, 24);
            this.richTextBoxServer.Name = "richTextBoxServer";
            this.richTextBoxServer.Size = new System.Drawing.Size(460, 496);
            this.richTextBoxServer.TabIndex = 5;
            this.richTextBoxServer.Text = "";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.labelUseServerConfig);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 520);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(0, 0, 0, 20);
            this.panel4.Size = new System.Drawing.Size(460, 100);
            this.panel4.TabIndex = 4;
            // 
            // labelUseServerConfig
            // 
            this.labelUseServerConfig.AutoSize = true;
            this.labelUseServerConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelUseServerConfig.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUseServerConfig.Location = new System.Drawing.Point(114, 35);
            this.labelUseServerConfig.Name = "labelUseServerConfig";
            this.labelUseServerConfig.Size = new System.Drawing.Size(236, 31);
            this.labelUseServerConfig.TabIndex = 1;
            this.labelUseServerConfig.Text = "Use Server Config";
            this.labelUseServerConfig.Click += new System.EventHandler(this.labelUseServerConfig_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labelServerConfigFilename);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(460, 24);
            this.panel2.TabIndex = 2;
            // 
            // labelServerConfigFilename
            // 
            this.labelServerConfigFilename.AutoSize = true;
            this.labelServerConfigFilename.Location = new System.Drawing.Point(3, 5);
            this.labelServerConfigFilename.Name = "labelServerConfigFilename";
            this.labelServerConfigFilename.Size = new System.Drawing.Size(71, 13);
            this.labelServerConfigFilename.TabIndex = 1;
            this.labelServerConfigFilename.Text = "Server Config";
            // 
            // FormCompare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 620);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.Name = "FormCompare";
            this.Text = "Encrypted Configuration Difference ";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RichTextBox richTextBoxLocal;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RichTextBox richTextBoxServer;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label labelLocalConfigFilename;
        private System.Windows.Forms.Label labelServerConfigFilename;
        private System.Windows.Forms.Label labelUseLocalConfig;
        private System.Windows.Forms.Label labelUseServerConfig;
    }
}