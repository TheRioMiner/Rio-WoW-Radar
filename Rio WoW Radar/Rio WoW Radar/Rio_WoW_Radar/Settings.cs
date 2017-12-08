using System;
using System.IO;

using Microsoft.Xna.Framework;
using Newtonsoft.Json;


namespace Rio_WoW_Radar
{
    public class Settings
    {
        public class Ores
        {
            public bool Draw = true;
            public bool Find = true;
            public int Size = 32;
            public Color Color = Color.DimGray;
            public float FontSize = 12.0f;
        }

        public class Herbs
        {
            public bool Draw = true;
            public bool Find = true;
            public int Size = 32;
            public Color Color = Color.LawnGreen;
            public float FontSize = 12.0f;
        }

        public class RareObjects
        {
            public bool Draw = true;
            public bool Find = true;
            public int Size = 32;
            public float FontSize = 12.0f;
        }

        public class OtherObjects
        {
            public bool Draw = false;
            public bool DrawLines = true;
            public int Size = 16;
            public Color Color = Color.LightGray;
            public float FontSize = 12.0f;
        }

        public class Nodes
        {
            public float RadiusCheck = 5.0f;
            public int Size = 24;

            public float NotExist_DivideFactor = 1.16f;
        }

        public class Sounds
        {
            public bool PlayHighLvl = true;
            public bool PlayLowLvl = true;
            public bool PlayInvisibles = true;

            public bool PlayMeDamaged = true;
        }

        public int My_Size = 32;
        public int Player_Size = 32;
        public int Npc_Size = 32;

        public byte HighLevel = 75;
        public float RadarZoom = 3.0f;


        public bool NpcEnabled = true;
        public bool PlayersEnabled = true;
        public bool FriendlyPlayersEnabled = true;
        public bool ObjectsEnabled = true;

        public bool TopMost = false;

        public Nodes nodes = new Nodes();

        public Ores ores = new Ores();
        public Herbs herbs = new Herbs();
        public RareObjects rareobjects = new RareObjects();
        public OtherObjects otherobjects = new OtherObjects();

        public Sounds sounds = new Sounds();


        private const string SettingsDir = "Settings";
        private const string Settings_FileName = "Настройки.json";

        private const string SettingsPath = SettingsDir + "\\" + Settings_FileName;

        //Загрузить настройки
        public static void LoadSettings()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    string readedSettings = File.ReadAllText(SettingsPath);
                    Settings desSettings = JsonConvert.DeserializeObject<Settings>(readedSettings);
                    
                    Game1.settings = desSettings;
                }
            }
            catch (Exception ex) { Tools.MsgBox.Exception(ex, "Ошибка загрузки настроек! Возможно файл был поврежден!"); }
        }


        //Сохранить настройки
        public static void SaveSettings()
        {
            try
            {
                //Создаем папку, если её нету
                if (!Directory.Exists(SettingsDir)) { Directory.CreateDirectory(SettingsDir); }


                string serSettings = JsonConvert.SerializeObject(Game1.settings, Formatting.Indented);

                File.WriteAllText(SettingsPath, serSettings);
            }
            catch (Exception ex) { Tools.MsgBox.Exception(ex, "Ошибка сохранения настроек! Возможно нету доступа к записи или создания файлов!"); }
        }
    }
}
