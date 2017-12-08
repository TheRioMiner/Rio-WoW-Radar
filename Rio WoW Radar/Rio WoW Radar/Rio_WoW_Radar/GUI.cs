using System;
using System.Windows.Forms;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Rio_WoW_Radar
{
    class GUI
    {
        public static Form general_form = (Form)Control.FromHandle(Program.game.Window.Handle);

        private static MenuStrip menuStrip = new MenuStrip();
        private static ToolStripMenuItem menuStrip_settings = new System.Windows.Forms.ToolStripMenuItem();
        private static ToolStripMenuItem menuStrip_playersGuid = new System.Windows.Forms.ToolStripMenuItem();

        public static bool MouseOver = false;

        public static void InitializeComponent()
        {
            // 
            // menuStrip1
            // 
            menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {menuStrip_settings, menuStrip_playersGuid});
            menuStrip.Location = new System.Drawing.Point(0, 0);
            menuStrip.Name = "menuStrip1";
            menuStrip.Size = new System.Drawing.Size(282, 28);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip1";
            // 
            // menuStrip_settings
            // 
            menuStrip_settings.Name = "menuStrip_settings";
            menuStrip_settings.Size = new System.Drawing.Size(96, 24);
            menuStrip_settings.Text = "Настройки";
            menuStrip_settings.Click += new System.EventHandler(menuStrip_settings_Click);
            // 
            // menuStrip_playersGuid
            // 
            menuStrip_playersGuid.Name = "menuStrip_playersGuid";
            menuStrip_playersGuid.Size = new System.Drawing.Size(96, 24);
            menuStrip_playersGuid.Text = "GUIDы игроков";
            menuStrip_playersGuid.Click += new System.EventHandler(menuStrip_playersGuid_Click);

            general_form.Controls.Add(menuStrip);
        }

        private static void menuStrip_settings_Click(object sender, EventArgs e)
        {
            using (Forms.Settings settingsForm = new Forms.Settings())
            {
                settingsForm.ShowDialog();
            }
        }


        private static void menuStrip_playersGuid_Click(object sender, EventArgs e)
        {
            using (Forms.Players_Guids playerGuidsForm = new Forms.Players_Guids())
            {
                playerGuidsForm.ShowDialog();
            }
        }


        public static void Update()
        {
            var mouseState = Mouse.GetState();
            var mousePosition = new Point(mouseState.X, mouseState.Y);

            Rectangle area = new Rectangle(menuStrip.Location.X, menuStrip.Location.Y, menuStrip.Size.Width, menuStrip.Size.Height);
            if (area.Contains(mousePosition))
            {
                menuStrip.Visible = true;
                MouseOver = true;
            }
            else
            {
                menuStrip.Visible = false;
                MouseOver = false;
            }

            //Делаем радар поверх всех окон, если включено в настройках
            if (general_form.TopMost != Game1.settings.TopMost) { general_form.TopMost = Game1.settings.TopMost; }
        }
    }
}
