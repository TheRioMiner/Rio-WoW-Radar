﻿namespace Rio_WoW_Radar.Forms
{
    partial class Players_Guids
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Players_Guids));
            this.button_LoadPlayers = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.PlayerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlayerGuid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlayerIntGuid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button_LoadPlayers
            // 
            this.button_LoadPlayers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_LoadPlayers.Location = new System.Drawing.Point(1, 431);
            this.button_LoadPlayers.Name = "button_LoadPlayers";
            this.button_LoadPlayers.Size = new System.Drawing.Size(351, 25);
            this.button_LoadPlayers.TabIndex = 3;
            this.button_LoadPlayers.Text = "Загрузить игроков";
            this.button_LoadPlayers.UseVisualStyleBackColor = true;
            this.button_LoadPlayers.Click += new System.EventHandler(this.button_LoadPlayers_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PlayerName,
            this.PlayerGuid,
            this.PlayerIntGuid});
            this.dataGridView1.Location = new System.Drawing.Point(1, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(351, 432);
            this.dataGridView1.TabIndex = 2;
            // 
            // PlayerName
            // 
            this.PlayerName.HeaderText = "Имя";
            this.PlayerName.Name = "PlayerName";
            this.PlayerName.Width = 105;
            // 
            // PlayerGuid
            // 
            this.PlayerGuid.HeaderText = "Hex GUID";
            this.PlayerGuid.MinimumWidth = 30;
            this.PlayerGuid.Name = "PlayerGuid";
            // 
            // PlayerIntGuid
            // 
            this.PlayerIntGuid.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PlayerIntGuid.HeaderText = "Int GUID";
            this.PlayerIntGuid.Name = "PlayerIntGuid";
            // 
            // Players_Guids
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 455);
            this.Controls.Add(this.button_LoadPlayers);
            this.Controls.Add(this.dataGridView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 1000);
            this.MinimumSize = new System.Drawing.Size(123, 166);
            this.Name = "Players_Guids";
            this.Text = "GUID\'ы игроков";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_LoadPlayers;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlayerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlayerGuid;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlayerIntGuid;
    }
}