using System;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;


namespace Rio_WoW_Radar
{
    public class Caching
    {
        public const string CacheDir = "Cache";

        public const string Players_FileName = "Кэш имен игроков.json";
        public const string Mobs_FileName = "Кэш имен мобов.json";

        public class Cache
        {
            public class MobsCache
            {
                //Кол. кэшированых имен
                public int NamesCached { get { return MobNames.Count; } }

                //Кэш имен мобов
                public IDictionary<UInt32, string> MobNames = new Dictionary<UInt32, string>();
            }


            //Кэш имен игроков
            public class PlayersCache
            {
                //Кол. кэшированых имен
                public int NamesCached { get { return PlayerNames.Count; } }

                //Кэш имен
                public IDictionary<ulong, string> PlayerNames = new Dictionary<ulong, string>();
            }


            public MobsCache mobs = new MobsCache();
            public PlayersCache players = new PlayersCache();
        }
        public static Cache cache = new Cache();




        //Получить имя моба из кэша
        public static bool GetMobName(uint MobID, ref string Name)
        {
            return cache.mobs.MobNames.TryGetValue(MobID, out Name);
        }


        //Получить имя игрока из кэша
        public static bool GetPlayerName(ulong PlayerGuid, ref string Name)
        {
            return cache.players.PlayerNames.TryGetValue(PlayerGuid, out Name);
        }


        //Добавить имя моба в кэш
        public static void AddMobName(uint NpcID, string Name)
        {
            if ((Name != string.Empty) & (NpcID != 0))
            {
                cache.mobs.MobNames[NpcID] = Name;
            }
        }


        //Добавить имя игрока в кэш
        public static void AddPlayerName(ulong PlayerGuid, string Name)
        {
            if ((Name != string.Empty) & (PlayerGuid != 0))
            {
                cache.players.PlayerNames[PlayerGuid] = Name;
            }
        }

        

        //Сохранить кэш
        public static void SaveCache()
        {
            try
            {
                //Проверяем, на месте ли папка?
                if (!Directory.Exists(CacheDir))
                { Directory.CreateDirectory(CacheDir); }
            }
            catch
            {
                Tools.MsgBox.Error("Ошибка создания папки для кэша." + "\n" + "Вероятно нет доступа для создания папок.");
                return;
            }


            string playersCache = "";
            string mobsCache = "";

            //Сериализируем кэш игроков
            try
            {
                playersCache = JsonConvert.SerializeObject(cache.players, Formatting.Indented);
            }
            catch { playersCache = ""; }

            //Сериализируем кэш мобов
            try
            {
                mobsCache = JsonConvert.SerializeObject(cache.mobs, Formatting.Indented);
            }
            catch { mobsCache = ""; }



            //Записываем в файл кэш игроков
            try
            {
                if (playersCache != "") { File.WriteAllText(CacheDir + "\\" + Players_FileName, playersCache); }
            }
            catch (Exception ex) { Tools.MsgBox.Exception(ex, "Ошибка записи файла кэша игроков!" + "\n" + "Вероятно нету доступа для создания или записи файлов!"); }


            //Записываем в файл кэш мобов
            try
            {
                if (mobsCache != "") { File.WriteAllText(CacheDir + "\\" + Mobs_FileName, mobsCache); }
            }
            catch (Exception ex) { Tools.MsgBox.Exception(ex, "Ошибка записи файла кэша мобов!" + "\n" + "Вероятно нету доступа для создания или записи файлов!"); }
        }

        

        //Загрузить кэш
        public static void LoadCache()
        {
            //Загрузка кэша игроков
            if (File.Exists(CacheDir + "\\" + Players_FileName))
            {
                string playersCache = "";

                //Читаем файл с кэшем
                try
                {
                    playersCache = File.ReadAllText(CacheDir + "\\" + Players_FileName);
                }
                catch (Exception ex)
                {
                    Tools.MsgBox.Exception(ex, "Ошибка чтения файла кэша игрока!" + "\n" + "Вероятно нету доступа к чтению файла!");
                    playersCache = "";
                }


                //Если успешно прочитали, десериализируем
                if (playersCache != "")
                {
                    try
                    {
                        Cache.PlayersCache deserialized = JsonConvert.DeserializeObject<Cache.PlayersCache>(playersCache);
                        cache.players = deserialized;
                    }
                    catch (Exception ex) { Tools.MsgBox.Exception(ex, "Ошибка десериализации кэша игроков!" + "\n" + "Вероятно файл был поврежден."); }
                }
            }




            //Загрузка кэша мобов
            if (File.Exists(CacheDir + "\\" + Mobs_FileName))
            {
                string mobsCache = "";

                //Читаем файл с кэшем
                try
                {
                    mobsCache = File.ReadAllText(CacheDir + "\\" + Mobs_FileName);
                }
                catch (Exception ex)
                {
                    Tools.MsgBox.Exception(ex, "Ошибка чтения файла кэша мобов!" + "\n" + "Вероятно нету доступа к чтению файла!");
                    mobsCache = "";
                }


                //Если успешно прочитали, десериализируем
                if (mobsCache != "")
                {
                    try
                    {
                        Cache.MobsCache deserialized = JsonConvert.DeserializeObject<Cache.MobsCache>(mobsCache);
                        cache.mobs = deserialized;
                    }
                    catch (Exception ex) { Tools.MsgBox.Exception(ex, "Ошибка десериализации кэша мобов!" + "\n" + "Вероятно файл был поврежден."); }
                }
            }
        }
    }
}
