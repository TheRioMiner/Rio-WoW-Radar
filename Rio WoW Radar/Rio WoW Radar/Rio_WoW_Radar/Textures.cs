using System.IO;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Rio_WoW_Radar
{
    public static class Textures
    {
        //Список текстурок
        private static IDictionary<string, Texture2D> Texture_List = new Dictionary<string, Texture2D>();

        //Просто белая текстура 1x1
        private static Texture2D _blankTexture;
        public static Texture2D BlankTexture
        {
            get { return _blankTexture; }
        }


        //Получить текстуру, если она есть в списке, иначе возвращяет пустую текстуру
        public static Texture2D GetTexture(string textureName)
        {
            try
            {
                Texture2D outTexture;
                if (Texture_List.TryGetValue(textureName, out outTexture))
                {
                    return outTexture;  //Текстура есть в списке, возвращяем текстурку
                }
                else
                {
                    return BlankTexture; //Текстуры нету в списке, возвращяем пустую текстурку
                }
            }
            catch
            {
                //Что-то пошло не так, возвращяем пустую текстуру
                return BlankTexture;
            }

        }



        //Загрузка текстурок
        public static bool LoadContent(this ContentManager contentManager, GraphicsDevice gd)
        {
            try
            {
                //Устанавливаем пустую текстуру
                _blankTexture = new Texture2D(gd, 1, 1, false, SurfaceFormat.Color);
                _blankTexture.SetData(new[] { Color.White });


                //Во всех папках ищем файлы
                foreach (string directory in Directory.GetDirectories(contentManager.RootDirectory))
                {
                    FileInfo[] files = new DirectoryInfo(directory).GetFiles("*.xnb");  //Получаем все xnb файлы из этой директории

                    //Теперь загружаем по файлу
                    foreach (FileInfo file in files)
                    {
                        try
                        {
                            string key = Path.GetFileNameWithoutExtension(file.Name);

                            string directoryToLoad = directory.Replace(contentManager.RootDirectory + "\\", "");
                            Texture2D tempTexture = contentManager.Load<Texture2D>(directoryToLoad + "\\" + key);

                            Texture_List[key] = tempTexture;
                        }
                        catch { }
                    }
                }

                //Загрузка основного шрифта
                Game1.mainFont = contentManager.Load<SpriteFont>("Fonts\\font_main");

                return true;
            }
            catch
            {
                return false;
            }
        }



        //Выгрузка текстурок
        public static void UnloadContent(this ContentManager contentManager)
        {
            foreach (KeyValuePair<string, Texture2D> tex in Texture_List)
            {
                //Выгружаем все текстуры из памяти
                tex.Value.Dispose();
            }
        }



        //Получить текстуру по рассе и полу
        public static Texture2D GetTextureByRaceAndGender(byte Race, byte Gender)
        {
            if (Gender == 0)
            {
                switch (Race)
                {
                    case 1:
                        return GetTexture("race_human_male");
                    case 2:
                        return GetTexture("race_orc_male");
                    case 3:
                        return GetTexture("race_dwarf_male");
                    case 4:
                        return GetTexture("race_nightelf_male");
                    case 5:
                        return GetTexture("race_undead_male");
                    case 6:
                        return GetTexture("race_tauren_male");
                    case 7:
                        return GetTexture("race_gnome_male");
                    case 8:
                        return GetTexture("race_troll_male");
                    case 10:
                        return GetTexture("race_bloodelf_male");
                    case 11:
                        return GetTexture("race_dranei_male");

                    default:
                        return BlankTexture;
                }
            }
            else
            {
                switch (Race)
                {
                    case 1:
                        return GetTexture("race_human_female");
                    case 2:
                        return GetTexture("race_orc_female");
                    case 3:
                        return GetTexture("race_dwarf_female");
                    case 4:
                        return GetTexture("race_nightelf_female");
                    case 5:
                        return GetTexture("race_undead_female");
                    case 6:
                        return GetTexture("race_tauren_female");
                    case 7:
                        return GetTexture("race_gnome_female");
                    case 8:
                        return GetTexture("race_troll_female");
                    case 10:
                        return GetTexture("race_bloodelf_female");
                    case 11:
                        return GetTexture("race_dranei_female");

                    default:
                        return BlankTexture;
                }
            }
        }


        //Получить текстуру класса
        public static Texture2D GetTextureByClass(byte Class)
        {
            switch (Class)
            {
                case 1:
                    return GetTexture("class_warrior");
                case 2:
                    return GetTexture("class_paladin");
                case 3:
                    return GetTexture("class_hunter");
                case 4:
                    return GetTexture("class_rogue");
                case 5:
                    return GetTexture("class_priest");
                case 6:
                    return GetTexture("class_dk");
                case 7:
                    return GetTexture("class_shaman");
                case 8:
                    return GetTexture("class_mage");
                case 9:
                    return GetTexture("class_warlock");
                case 11:
                    return GetTexture("class_druid");

                default:
                    return BlankTexture;
            }
        }
    }
}
