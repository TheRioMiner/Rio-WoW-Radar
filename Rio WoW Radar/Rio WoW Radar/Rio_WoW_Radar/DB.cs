using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

using Newtonsoft.Json;
using Microsoft.Xna.Framework;


namespace Rio_WoW_Radar
{
    #region DataBase structs

    public struct LVL
    {
        public byte L;
        public string T;

        public LVL(byte Lvl, string Time)
        {
            L = Lvl;
            T = Time;
        }
    }

    public struct SEEN
    {
        public UInt32 I;
        public string T;

        public SEEN(UInt32 ZoneId, string Time)
        {
            I = ZoneId;
            T = Time;
        }
    }

    #endregion


    public class DataBase
    {
        public class cPlayers
        {
            public class Player
            {
                //Имя и гуид
                public string Name { get; set; } = "Неизвестно";
                public ulong GUID { get; set; }

                //Уровень добавляем вручную
                public byte LvL { get; set; }

                public byte Race { get; set; }
                public byte Gender { get; set; }
                public byte Class { get; set; }

                //Последняя позиция обнаружения
                public Vector3 LastPos { get; set; }

                //Последняя зона где находился игрок
                public UInt32 LastZoneID { get; set; }

                //Последний раз где игрок был замечен
                public string LastSeen { get; set; } = "";

                //Список уровней когда получил уровень
                public List<LVL> Levels { get; set; } = new List<LVL>();

                //Зоны где был игрок
                public List<SEEN> Seens { get; set; } = new List<SEEN>();
            }

            //Получить самый маленький или большой guid из бд
            private ulong GetHighnestOrLowestGuid(bool hightnest)
            {
                if (hightnest)
                {
                    ulong highnestGuid = ulong.MinValue;

                    for (int i = 0; i < PlayersCount; i++)
                    {
                        try  //Предохраняемся
                        {
                            if (PlayerList[i].GUID > highnestGuid)
                            {
                                highnestGuid = PlayerList[i].GUID;
                            }
                        }
                        catch { }
                    }

                    return highnestGuid;
                }
                else
                {
                    ulong lowestGuid = ulong.MaxValue;

                    for (int i = 0; i < PlayersCount; i++)
                    {
                        try  //Предохраняемся
                        {
                            if (PlayerList[i].GUID < lowestGuid)
                            {
                                lowestGuid = PlayerList[i].GUID;
                            }
                        }
                        catch { }
                    }

                    return lowestGuid;
                }
            }


            public ulong HighnestGuid { get { return GetHighnestOrLowestGuid(true); } }
            public ulong LowestGuid { get { return GetHighnestOrLowestGuid(false); } }
            public int PlayersCount { get { return PlayerList.Count; } }

            public List<Player> PlayerList = new List<Player>();
        }


        public class cOres
        {
            public class Ore
            {
                //Имя и гуид
                public string Name { get; set; } = "Неизвестно";

                //Ид
                public uint ID { get; set; }

                //Позиция
                public Vector3 Position { get; set; }

                //Максимальная дистанция обнаружения
                public float MaxSeeDistance { get; set; }

                //Зона где был обнаружена руда
                public string Zone { get; set; } = "";

                //Последний раз где руда был замечен
                public string LastSeen { get; set; } = "";
            }

            //Получить максимальную дистанцию отрисовки
            private float GetMaxDistance()
            {
                float MaxDistance = 1;

                for (int i = 0; i < OreList.Count; i++)
                {
                    try  //Предохраняемся
                    {
                        if (OreList[i].MaxSeeDistance > MaxDistance)
                        {
                            MaxDistance = OreList[i].MaxSeeDistance;
                        }
                    }
                    catch { }
                }

                return (float)Math.Round(MaxDistance, 2);
            }


            public float MaxSeeDistance { get { return GetMaxDistance(); } }
            public int OresCount { get { return OreList.Count; } }

            public List<Ore> OreList = new List<Ore>();
        }


        public class cHerbs
        {
            public class Herb
            {
                //Имя и гуид
                public string Name { get; set; } = "Неизвестно";

                //Ид
                public uint ID { get; set; }

                //Позиция
                public Vector3 Position { get; set; }

                //Максимальная дистанция обнаружения
                public float MaxSeeDistance { get; set; }

                //Зона где был обнаружена травка
                public string Zone { get; set; } = "";

                //Последний раз где травка был замечен
                public string LastSeen { get; set; } = "";
            }

            //Получить максимальную дистанцию отрисовки
            private float GetMaxDistance()
            {
                float MaxDistance = 1;

                for (int i = 0; i < HerbList.Count; i++)
                {
                    try  //Предохраняемся
                    {
                        if (HerbList[i].MaxSeeDistance > MaxDistance)
                        {
                            MaxDistance = HerbList[i].MaxSeeDistance;
                        }
                    }
                    catch { }
                }

                return (float)Math.Round(MaxDistance, 2);
            }


            public float MaxSeeDistance { get { return GetMaxDistance(); } }
            public int HerbsCount { get { return HerbList.Count; } }

            public List<Herb> HerbList = new List<Herb>();
        }


        public class cObjects
        {
            public class Object
            {
                //Ид
                public uint ID { get; set; }

                //Позиция
                public Vector3 Position { get; set; }

                //Максимальная дистанция обнаружения
                public float MaxSeeDistance { get; set; }

                //Зона где был обнаружена травка
                public string Zone { get; set; } = "";

                //Последний раз где объект был замечен
                public string LastSeen { get; set; } = "";
            }

            //Получить максимальную дистанцию отрисовки
            private float GetMaxDistance()
            {
                float MaxDistance = 1;

                for (int i = 0; i < ObjectList.Count; i++)
                {
                    if (ObjectList[i].MaxSeeDistance > MaxDistance)
                    {
                        MaxDistance = ObjectList[i].MaxSeeDistance;
                    }
                }

                return (float)Math.Round(MaxDistance, 2);
            }


            public float MaxSeeDistance { get { return GetMaxDistance(); } }
            public int ObjectsCount { get { return ObjectList.Count; } }

            public List<Object> ObjectList = new List<Object>();
        }


        public cPlayers Players = new cPlayers();
        public cOres Ores = new cOres();
        public cObjects Objects = new cObjects();
        public cHerbs Herbs = new cHerbs();
    }



    public class DB
    {
        public static DataBase database = new DataBase();
        private const string Players_FileName = "players.json";
        private const string Ores_FileName = "ores.json";
        private const string Herbs_FileName = "herbs.json";
        private const string Objects_FileName = "objects.json";



        //Сохранить базу данных
        public static void SaveDB()
        {
            string players = JsonConvert.SerializeObject(database.Players, Formatting.Indented);
            string ores = JsonConvert.SerializeObject(database.Ores, Formatting.Indented);
            string herbs = JsonConvert.SerializeObject(database.Herbs, Formatting.Indented);
            string objects = JsonConvert.SerializeObject(database.Objects, Formatting.Indented);

            try { File.WriteAllText(Players_FileName, players); }
            catch (Exception ex) { MessageBox.Show("Ошибка сохранения бд игроков: " + ex.Message + Environment.NewLine + ex.InnerException); }

            try { File.WriteAllText(Ores_FileName, ores); }
            catch (Exception ex) { MessageBox.Show("Ошибка сохранения бд руд: " + ex.Message + Environment.NewLine + ex.InnerException); }

            try { File.WriteAllText(Herbs_FileName, herbs); }
            catch (Exception ex) { MessageBox.Show("Ошибка сохранения бд трав: " + ex.Message + Environment.NewLine + ex.InnerException); }

            try { File.WriteAllText(Objects_FileName, objects); }
            catch (Exception ex) { MessageBox.Show("Ошибка сохранения бд объектов: " + ex.Message + Environment.NewLine + ex.InnerException); }
        }


        //Загрузить базу данных
        public static void LoadDB()
        {
            if (File.Exists(Players_FileName))
            {
                string players = "";

                try { players = File.ReadAllText(Players_FileName); }
                catch (Exception ex) { MessageBox.Show("Ошибка загрузки бд игроков: " + ex.Message + Environment.NewLine + ex.InnerException); return; }

                database.Players = JsonConvert.DeserializeObject<DataBase.cPlayers>(players);
            }


            if (File.Exists(Ores_FileName))
            {
                string ores = "";

                try { ores = File.ReadAllText(Ores_FileName); }
                catch (Exception ex) { MessageBox.Show("Ошибка загрузки бд руд: " + ex.Message + Environment.NewLine + ex.InnerException); return; }

                database.Ores = JsonConvert.DeserializeObject<DataBase.cOres>(ores);
            }


            if (File.Exists(Herbs_FileName))
            {
                string herbs = "";

                try { herbs = File.ReadAllText(Herbs_FileName); }
                catch (Exception ex) { MessageBox.Show("Ошибка загрузки бд трав: " + ex.Message + Environment.NewLine + ex.InnerException); return; }

                database.Herbs = JsonConvert.DeserializeObject<DataBase.cHerbs>(herbs);
            }


            if (File.Exists(Objects_FileName))
            {
                string objects = "";

                try { objects = File.ReadAllText(Herbs_FileName); }
                catch (Exception ex) { MessageBox.Show("Ошибка загрузки бд объектов: " + ex.Message + Environment.NewLine + ex.InnerException); return; }

                database.Objects = JsonConvert.DeserializeObject<DataBase.cObjects>(objects);
            }
        }



        public static void AddOrUpdatePlayer(DataBase.cPlayers.Player item)
        {
            if (item.GUID != 0)
            {
                for (int i = 0; i < database.Players.PlayersCount; i++)
                {
                    if (database.Players.PlayerList[i].GUID == item.GUID)
                    {
                        //Устанавливаем имя
                        if (item.Name != "")
                        { database.Players.PlayerList[i].Name = item.Name; }

                        //Установка уровня
                        if (item.LvL != 0)
                        { database.Players.PlayerList[i].LvL = item.LvL; }

                        //Устанавливаем последнюю зону
                        if (item.LastZoneID != 0)
                        { database.Players.PlayerList[i].LastZoneID = item.LastZoneID; }

                        //Устанавливаем дату последнего обнаружения
                        if (item.LastSeen != "")
                        { database.Players.PlayerList[i].LastSeen = item.LastSeen; }


                        //Обновляем последнюю позицию
                        database.Players.PlayerList[i].LastPos = Tools.RoundVector3(item.LastPos, 2);

                        //Обновляем уровень в списке уровней
                        UpdateLevel(i, item.LvL);

                        //Обновляем обнаружения
                        UpdatePlayerSeen(i, item.LastZoneID);


                        return;
                    }
                }

                //Если не нашли в базе данных, то значит добавляем новый экземпляр
                if (item.LvL != 0 & item.LvL != 80) { item.Levels.Add(new LVL(item.LvL, Tools.GetNowTime())); }
                if (item.LastZoneID != 0) { item.Seens.Add(new SEEN(item.LastZoneID, Tools.GetNowTime())); }

                database.Players.PlayerList.Add(item);
            }
        }



        public static void AddOrUpdateOre(DataBase.cOres.Ore item)
        {
            for (int i = 0; i < database.Ores.OresCount; i++)
            {
                bool inRadius = Tools.Vector3InRadius(database.Ores.OreList[i].Position, item.Position, Game1.settings.nodes.RadiusCheck);
                if (inRadius)
                {
                    //Установка позиции
                    database.Ores.OreList[i].Position = item.Position;

                    //Обновляем максимальную дистанцию отрисовки
                    if (item.MaxSeeDistance > database.Ores.OreList[i].MaxSeeDistance)
                    { database.Ores.OreList[i].MaxSeeDistance = item.MaxSeeDistance; }

                    //Устанавливаем текущее время
                    if (item.LastSeen != "")
                    { database.Ores.OreList[i].LastSeen = item.LastSeen; }

                    if (item.Zone != "")
                    { database.Ores.OreList[i].Zone = item.Zone; }

                    return;
                }
            }

            //Если не нашли в базе данных, то значит добавляем новый экземпляр
            database.Ores.OreList.Add(item);
        }


        public static void AddOrUpdateHerb(DataBase.cHerbs.Herb item)
        {
            for (int i = 0; i < database.Herbs.HerbsCount; i++)
            {
                bool inRadius = Tools.Vector3InRadius(database.Herbs.HerbList[i].Position, item.Position, Game1.settings.nodes.RadiusCheck);
                if (inRadius)
                {
                    //Установка позиции
                    database.Herbs.HerbList[i].Position = item.Position;

                    //Обновляем максимальную дистанцию отрисовки
                    if (item.MaxSeeDistance > database.Herbs.HerbList[i].MaxSeeDistance)
                    { database.Herbs.HerbList[i].MaxSeeDistance = item.MaxSeeDistance; }

                    //Устанавливаем текущее время
                    if (item.LastSeen != "")
                    { database.Herbs.HerbList[i].LastSeen = item.LastSeen; }

                    if (item.Zone != "")
                    { database.Herbs.HerbList[i].Zone = item.Zone; }

                    return;
                }
            }

            //Если не нашли в базе данных, то значит добавляем новый экземпляр
            database.Herbs.HerbList.Add(item);
        }


        public static void AddOrUpdateObject(DataBase.cObjects.Object item)
        {
            for (int i = 0; i < database.Objects.ObjectsCount; i++)
            {
                bool inRadius = Tools.Vector3InRadius(database.Objects.ObjectList[i].Position, item.Position, Game1.settings.nodes.RadiusCheck);
                if (inRadius)
                {
                    //Установка позиции
                    database.Objects.ObjectList[i].Position = item.Position;

                    //Обновляем максимальную дистанцию отрисовки
                    if (item.MaxSeeDistance > database.Objects.ObjectList[i].MaxSeeDistance)
                    { database.Objects.ObjectList[i].MaxSeeDistance = item.MaxSeeDistance; }

                    //Устанавливаем текущее время
                    if (item.LastSeen != "")
                    { database.Objects.ObjectList[i].LastSeen = item.LastSeen; }

                    if (item.Zone != "")
                    { database.Objects.ObjectList[i].Zone = item.Zone; }

                    return;
                }
            }

            //Если не нашли в базе данных, то значит добавляем новый экземпляр
            database.Objects.ObjectList.Add(item);
        }


        //Обновление уровня для игрока
        public static void UpdateLevel(int i, byte Level)
        {
            if (Level > 0 & Level <= 80)
            {
                int MaxLevel = 0;
                for (int j = 0; j < database.Players.PlayerList[i].Levels.Count; j++)
                {
                    if (database.Players.PlayerList[i].Levels[j].L > MaxLevel)
                    {
                        MaxLevel = database.Players.PlayerList[i].Levels[j].L;
                    }
                }

                //Если веденный уровень больше из списка, добавляем!
                if (Level > MaxLevel)
                {
                    //Зачем записывать изменения уровней для уже 80 лвл? Поэтому не добавляем
                    if ((database.Players.PlayerList[i].Levels.Count == 0) & (Level == 80))
                    {
                        database.Players.PlayerList[i].Levels = new List<LVL>();
                    }
                    else
                    {
                        database.Players.PlayerList[i].Levels.Add(new LVL(Level, Tools.GetNowTime()));
                    }
                }

                database.Players.PlayerList[i].LvL = Level; //Устанавливаем обычный уровень

                return;
            }
        }


        //Обновление зоны игрока
        public static void UpdatePlayerSeen(int i, UInt32 ZoneID)
        {
            if (ZoneID != 0)
            {
                //Избегаем эксепшена
                if (database.Players.PlayerList[i].Seens.Count > 0)
                {
                    //Если последняя зона не равна указанной
                    if (database.Players.PlayerList[i].Seens[database.Players.PlayerList[i].Seens.Count - 1].I != ZoneID)
                    {
                        //Добавляем
                        database.Players.PlayerList[i].Seens.Add(new SEEN(ZoneID, Tools.GetNowTime()));
                    }
                }
            }
        }

        
    }
}
