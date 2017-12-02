using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

using Newtonsoft.Json;


namespace Rio_WoW_Radar
{
    public class Caching
    {
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


        const string mobs_FileName = "Mobs_Cache.json";
        const string players_FileName = "Players_Cache.json";




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
            string tempStr = "";
            bool existInCache = GetMobName(NpcID, ref tempStr);

            if (!existInCache)  //Если нету в кэше, добавляем
            {
                if ((Name != string.Empty) & (NpcID != 0))
                {
                   cache.mobs.MobNames.Add(NpcID, Name);
                }
            }
        }


        //Добавить имя игрока в кэш
        public static void AddPlayerName(ulong PlayerGuid, string Name)
        {
            string tempStr = "";
            bool existInCache = GetPlayerName(PlayerGuid, ref tempStr);

            if (!existInCache)  //Если нету в кэше, добавляем
            {
                if ((Name != string.Empty) & (PlayerGuid != 0))
                {
                    cache.players.PlayerNames.Add(PlayerGuid, Name);
                }
            }
        }
        

        //Сохранить кэш в файл
        public static void SaveCache()
        {
            try
            {
                //Сохраняем кэш мобов
                string mobsCache = JsonConvert.SerializeObject(cache.mobs, Formatting.Indented);
                File.WriteAllText(mobs_FileName, mobsCache);


                //Сохраняем кэш игроков
                string playersCache = JsonConvert.SerializeObject(cache.players, Formatting.Indented);
                File.WriteAllText(players_FileName, playersCache);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения кэша названий мобов: " + ex.Message);
            }
        }
        

        //Загрузить кэш
        public static void LoadCache()
        {
            try
            {
                //Загрузка кэша мобов
                if (File.Exists(mobs_FileName))
                {
                    string mobCache = File.ReadAllText(mobs_FileName);
                    Cache.MobsCache deserialized = JsonConvert.DeserializeObject<Cache.MobsCache>(mobCache);
                    cache.mobs = deserialized;
                }


                //Загрузка кэша игроков
                if (File.Exists(players_FileName))
                {
                    string playersCache = File.ReadAllText(players_FileName);
                    Cache.PlayersCache deserialized = JsonConvert.DeserializeObject<Cache.PlayersCache>(playersCache);
                    cache.players = deserialized;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки кэша названий мобов: " + ex.Message);
            }
        }
    }
}
