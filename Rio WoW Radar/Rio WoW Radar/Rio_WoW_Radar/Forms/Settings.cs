using System;
using System.Windows.Forms;

using Microsoft.Xna.Framework;
using System.Diagnostics;


namespace Rio_WoW_Radar.Forms
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();

            try
            {
                //Главное
                {
                    //Руды
                    {
                        checkBox_ore_draw.Checked = Game1.settings.ores.Draw;
                        checkBox_ore_find.Checked = Game1.settings.ores.Find;
                        numericUpDown_ore_icoSize.Value = Game1.settings.ores.Size;
                        numericUpDown_ore_fontSize.Value = (decimal)Game1.settings.ores.FontSize;
                        button_ore_fontColor.BackColor = System.Drawing.Color.FromArgb(Game1.settings.ores.Color.R, Game1.settings.ores.Color.G, Game1.settings.ores.Color.B);
                    }

                    //Травы
                    {
                        checkBox_herb_draw.Checked = Game1.settings.herbs.Draw;
                        checkBox_herb_find.Checked = Game1.settings.herbs.Find;
                        numericUpDown_herb_icoSize.Value = Game1.settings.herbs.Size;
                        numericUpDown_herb_fontSize.Value = (decimal)Game1.settings.herbs.FontSize;
                        button_herb_fontColor.BackColor = System.Drawing.Color.FromArgb(Game1.settings.herbs.Color.R, Game1.settings.herbs.Color.G, Game1.settings.herbs.Color.B);
                    }

                    //Редкие объекты
                    {
                        checkBox_rare_draw.Checked = Game1.settings.rareobjects.Draw;
                        checkBox_rare_find.Checked = Game1.settings.rareobjects.Find;
                        numericUpDown_rare_icoSize.Value = Game1.settings.rareobjects.Size;
                        numericUpDown_rare_fontSize.Value = (decimal)Game1.settings.rareobjects.FontSize;
                    }

                    //Остальные объекты
                    {
                        checkBox_other_draw.Checked = Game1.settings.otherobjects.Draw;
                        checkBox_other_drawLines.Checked = Game1.settings.otherobjects.DrawLines;
                        numericUpDown_other_icoSize.Value = Game1.settings.otherobjects.Size;
                        numericUpDown_other_fontSize.Value = (decimal)Game1.settings.otherobjects.FontSize;
                        button_other_fontColor.BackColor = System.Drawing.Color.FromArgb(Game1.settings.otherobjects.Color.R, Game1.settings.otherobjects.Color.G, Game1.settings.otherobjects.Color.B);
                    }

                    //Размеры
                    {
                        numericUpDown_myPlayerSize.Value = Game1.settings.My_Size;
                        numericUpDown_playersSize.Value = Game1.settings.Player_Size;
                        numericUpDown_npcSize.Value = Game1.settings.Npc_Size;
                    }

                    //Чтение
                    {
                        checkBox_playersEnabled.Checked = Game1.settings.PlayersEnabled;
                        checkBox_friendlyPlayersEnabled.Checked = Game1.settings.FriendlyPlayersEnabled;
                        checkBox_npcPlayersEnabled.Checked = Game1.settings.NpcEnabled;
                        checkBox_objectsEnabled.Checked = Game1.settings.ObjectsEnabled;
                    }

                    //Ноды
                    {
                        numericUpDown_nodeSize.Value = Game1.settings.nodes.Size;
                        numericUpDown_nodeRadiusCheck.Value = (decimal)Game1.settings.nodes.RadiusCheck;
                        numericUpDown_nodeDivideFactor.Value = (decimal)Game1.settings.nodes.NotExist_DivideFactor;
                    }

                    //Остальное
                    {
                        numericUpDown_highLvl.Value = Game1.settings.HighLevel;
                        checkBox_topMost.Checked = Game1.settings.TopMost;
                        numericUpDown_radarZoom.Value = (decimal)Game1.settings.RadarZoom;
                    }
                }



                //Звуки
                {
                    checkBox_play_highLvl.Checked = Game1.settings.sounds.PlayHighLvl;
                    checkBox_play_lowLvl.Checked = Game1.settings.sounds.PlayLowLvl;
                    checkBox_play_invisibles.Checked = Game1.settings.sounds.PlayInvisibles;

                    checkBox_play_meDamaged.Checked = Game1.settings.sounds.PlayMeDamaged;
                }
            }
            catch (Exception ex)
            {
                Tools.MsgBox.Exception(ex, "Ошибка загрузки настроек, возможно настройки были подправлены из файла!");
            }

            button_Save.Enabled = false;
        }


        private void button_Save_Click(object sender, EventArgs e)
        {
            //Главное
            {
                //Руды
                {
                    Game1.settings.ores.Draw = checkBox_ore_draw.Checked;
                    Game1.settings.ores.Find = checkBox_ore_find.Checked;
                    Game1.settings.ores.Size = (int)numericUpDown_ore_icoSize.Value;
                    Game1.settings.ores.FontSize = (float)numericUpDown_ore_fontSize.Value;
                    Game1.settings.ores.Color = new Color(button_ore_fontColor.BackColor.R, button_ore_fontColor.BackColor.G, button_ore_fontColor.BackColor.B);
                }

                //Травы
                {
                    Game1.settings.herbs.Draw = checkBox_herb_draw.Checked;
                    Game1.settings.herbs.Find = checkBox_herb_find.Checked;
                    Game1.settings.herbs.Size = (int)numericUpDown_herb_icoSize.Value;
                    Game1.settings.herbs.FontSize = (float)numericUpDown_herb_fontSize.Value;
                    Game1.settings.herbs.Color = new Color(button_herb_fontColor.BackColor.R, button_herb_fontColor.BackColor.G, button_herb_fontColor.BackColor.B);
                }

                //Редкие объекты
                {
                    Game1.settings.rareobjects.Draw = checkBox_rare_draw.Checked;
                    Game1.settings.rareobjects.Find = checkBox_rare_find.Checked;
                    Game1.settings.rareobjects.Size = (int)numericUpDown_rare_icoSize.Value;
                    Game1.settings.rareobjects.FontSize = (float)numericUpDown_rare_fontSize.Value;
                }

                //Остальные объекты
                {
                    Game1.settings.otherobjects.Draw = checkBox_other_draw.Checked;
                    Game1.settings.otherobjects.DrawLines = checkBox_other_drawLines.Checked;
                    Game1.settings.otherobjects.Size = (int)numericUpDown_other_icoSize.Value;
                    Game1.settings.otherobjects.FontSize = (float)numericUpDown_other_fontSize.Value;
                    Game1.settings.otherobjects.Color = new Color(button_other_fontColor.BackColor.R, button_other_fontColor.BackColor.G, button_other_fontColor.BackColor.B);
                }

                //Размеры
                {
                    Game1.settings.My_Size = (int)numericUpDown_myPlayerSize.Value;
                    Game1.settings.Player_Size = (int)numericUpDown_playersSize.Value;
                    Game1.settings.Npc_Size = (int)numericUpDown_npcSize.Value;
                    Game1.settings.nodes.Size = (int)numericUpDown_nodeSize.Value;
                }

                //Чтение
                {
                    Game1.settings.PlayersEnabled = checkBox_playersEnabled.Checked;
                    Game1.settings.FriendlyPlayersEnabled = checkBox_friendlyPlayersEnabled.Checked;
                    Game1.settings.NpcEnabled = checkBox_npcPlayersEnabled.Checked;
                    Game1.settings.ObjectsEnabled = checkBox_objectsEnabled.Checked;
                }

                //Ноды
                {
                    Game1.settings.nodes.Size = (int)numericUpDown_nodeSize.Value;
                    Game1.settings.nodes.RadiusCheck = (float)numericUpDown_nodeRadiusCheck.Value;
                    Game1.settings.nodes.NotExist_DivideFactor = (float)numericUpDown_nodeDivideFactor.Value;
                }

                //Остальное
                {
                    Game1.settings.HighLevel = (byte)numericUpDown_highLvl.Value;
                    Game1.settings.TopMost = checkBox_topMost.Checked;
                    Game1.settings.RadarZoom = (float)numericUpDown_radarZoom.Value;
                }
            }


            //Звуки
            {
                Game1.settings.sounds.PlayHighLvl = checkBox_play_highLvl.Checked;
                Game1.settings.sounds.PlayLowLvl = checkBox_play_lowLvl.Checked;
                Game1.settings.sounds.PlayInvisibles = checkBox_play_invisibles.Checked;

                Game1.settings.sounds.PlayMeDamaged = checkBox_play_meDamaged.Checked;
            }


            Rio_WoW_Radar.Settings.SaveSettings();
            button_Save.Enabled = false;
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SomeChanged(object sender, EventArgs e)
        {
            button_Save.Enabled = true;
        }


        private void button_ore_fontColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = button_ore_fontColor.BackColor;
                colorDialog.ShowDialog();
                button_ore_fontColor.BackColor = colorDialog.Color;

                SomeChanged(sender, e);
            }
        }


        private void button_herb_fontColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = button_herb_fontColor.BackColor;
                colorDialog.ShowDialog();
                button_herb_fontColor.BackColor = colorDialog.Color;

                SomeChanged(sender, e);
            }
        }


        private void button_other_fontColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = button_other_fontColor.BackColor;
                colorDialog.ShowDialog();
                button_other_fontColor.BackColor = colorDialog.Color;

                SomeChanged(sender, e);
            }
        }

        private void linkLabel_toGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkLabel_toGitHub.Text);
        }
    }
}
