using System;
using System.Collections;
using System.Windows.Forms;

namespace Rio_WoW_Radar
{
    public partial class PlayersGuid_Form : Form
    {
        public PlayersGuid_Form()
        {
            InitializeComponent();
        }

        private void button_LoadPlayers_Click(object sender, EventArgs e)
        {
            ArrayList players = Game1.scanner.Players;

            ulong smallestGuid = ulong.MaxValue;
            ulong longerGuid = ulong.MinValue;

            dataGridView1.Rows.Clear();

            foreach (Radar.PlayerObject player in players)
            {
                if (player.Guid < smallestGuid)
                {
                    smallestGuid = player.Guid;
                }

                if (player.Guid > longerGuid)
                {
                    longerGuid = player.Guid;
                }
            }

            foreach (Radar.PlayerObject player in players)
            {
                string name = player.Name;
                string guid = player.Guid.ToString("X");
                string intGuid = player.Guid.ToString("#,#");

                DataGridViewCellStyle rowColor = new DataGridViewCellStyle();

                if (player.Guid == smallestGuid)
                {
                    rowColor.BackColor = System.Drawing.Color.Red;
                }
                if (player.Guid == longerGuid)
                {
                    rowColor.BackColor = System.Drawing.Color.Gold;
                }

                int index = dataGridView1.Rows.Add(name, guid, intGuid);
                dataGridView1.Rows[index].DefaultCellStyle = rowColor;
            }
        }
    }
}
