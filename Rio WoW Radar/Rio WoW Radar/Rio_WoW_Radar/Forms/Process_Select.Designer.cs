namespace Rio_WoW_Radar.Forms
{
    partial class Process_Select
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.PPid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PHexPID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PLogin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PInWorld = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PConnected = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_refresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PPid,
            this.PHexPID,
            this.PLogin,
            this.PInWorld,
            this.PConnected});
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(359, 304);
            this.dataGridView1.TabIndex = 0;
            // 
            // PPid
            // 
            this.PPid.HeaderText = "PID";
            this.PPid.Name = "PPid";
            this.PPid.Width = 50;
            // 
            // PHexPID
            // 
            this.PHexPID.HeaderText = "Hex PID";
            this.PHexPID.Name = "PHexPID";
            this.PHexPID.Width = 60;
            // 
            // PLogin
            // 
            this.PLogin.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PLogin.HeaderText = "Логин";
            this.PLogin.MinimumWidth = 65;
            this.PLogin.Name = "PLogin";
            // 
            // PInWorld
            // 
            this.PInWorld.HeaderText = "В мире";
            this.PInWorld.Name = "PInWorld";
            this.PInWorld.Width = 60;
            // 
            // PConnected
            // 
            this.PConnected.HeaderText = "Коннект";
            this.PConnected.Name = "PConnected";
            this.PConnected.Width = 65;
            // 
            // button_ok
            // 
            this.button_ok.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ok.Location = new System.Drawing.Point(-1, 303);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(274, 30);
            this.button_ok.TabIndex = 1;
            this.button_ok.Text = "Выбрать";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_refresh
            // 
            this.button_refresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_refresh.Location = new System.Drawing.Point(272, 303);
            this.button_refresh.Name = "button_refresh";
            this.button_refresh.Size = new System.Drawing.Size(88, 30);
            this.button_refresh.TabIndex = 2;
            this.button_refresh.Text = "Обновить";
            this.button_refresh.UseVisualStyleBackColor = true;
            this.button_refresh.Click += new System.EventHandler(this.button_refresh_Click);
            // 
            // Process_Select
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 332);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button_refresh);
            this.Controls.Add(this.button_ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Process_Select";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Выберите процесс";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_refresh;
        private System.Windows.Forms.DataGridViewTextBoxColumn PPid;
        private System.Windows.Forms.DataGridViewTextBoxColumn PHexPID;
        private System.Windows.Forms.DataGridViewTextBoxColumn PLogin;
        private System.Windows.Forms.DataGridViewTextBoxColumn PInWorld;
        private System.Windows.Forms.DataGridViewTextBoxColumn PConnected;
    }
}