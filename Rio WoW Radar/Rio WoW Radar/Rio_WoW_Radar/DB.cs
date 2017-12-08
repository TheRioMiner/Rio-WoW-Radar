using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Newtonsoft.Json;


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
                //Имя руды
                public string Name { get; set; } = "Неизвестно";

                //Ид
                public uint ID { get; set; }

                //Позиция
                public Vector2 Position { get; set; }

                //Максимальная дистанция обнаружения
                public float MaxSeeDistance { get; set; }

                //Последний раз где руда был замечена
                public string LastSeen { get; set; } = "";
            }

            //Получить максимальную дистанцию отрисовки
            private float GetMaxDistance()
            {
                float MaxDistance = 1;

                try  //Предохраняемся)
                {
                    foreach (var ores in OresDict)
                    {
                        for (int i = 0; i < ores.Value.Count; i++)
                        {
                            float currDist = ores.Value[i].MaxSeeDistance;

                            if (currDist > MaxDistance)
                            {
                                MaxDistance = currDist;
                            }
                        }
                    }
                }
                catch { }

                return (float)Math.Round(MaxDistance, 2);
            }

            //Получить список руды в конкретной зоне
            public bool GetList(uint ZoneID, out List<Ore> list)
            {
                return OresDict.TryGetValue(ZoneID, out list);
            }

            //Есть ли зона в списке?
            public bool ZoneExist(uint ZoneID)
            {
                List<Ore> temp = new List<Ore>();
                return OresDict.TryGetValue(ZoneID, out temp);
            }


            //Максимальная дист. прорисовки из всех трав в списке
            public float MaxSeeDistance { get { return GetMaxDistance(); } }

            //Список руды разбитая по зонам
            public Dictionary<uint, List<Ore>> OresDict = new Dictionary<uint, List<Ore>>();
        }


        public class cHerbs
        {
            public class Herb
            {
                //Имя травы
                public string Name { get; set; } = "Неизвестно";

                //Ид
                public uint ID { get; set; }

                //Позиция
                public Vector2 Position { get; set; }

                //Максимальная дистанция обнаружения
                public float MaxSeeDistance { get; set; }

                //Последний раз где травка был замечена
                public string LastSeen { get; set; } = "";
            }

            //Получить максимальную дистанцию отрисовки
            private float GetMaxDistance()
            {
                float MaxDistance = 1;

                try  //Предохраняемся)
                {
                    foreach (var herbs in HerbDict)
                    {
                        for (int i = 0; i < herbs.Value.Count; i++)
                        {
                            float currDist = herbs.Value[i].MaxSeeDistance;

                            if (currDist > MaxDistance)
                            {
                                MaxDistance = currDist;
                            }
                        }
                    }
                }
                catch { }

                return (float)Math.Round(MaxDistance, 2);
            }

            //Получить список травки в конкретной зоне
            public bool GetList(uint ZoneID, out List<Herb> list)
            {
                return HerbDict.TryGetValue(ZoneID, out list);
            }

            //Есть ли зона в списке?
            public bool ZoneExist(uint ZoneID)
            {
                List<Herb> temp = new List<Herb>();
                return HerbDict.TryGetValue(ZoneID, out temp);
            }


            //Максимальная дист. прорисовки из всех трав в списке
            public float MaxSeeDistance { get { return GetMaxDistance(); } }
            
            //Список травок разбитая по зонам
            public Dictionary<uint, List<Herb>> HerbDict = new Dictionary<uint, List<Herb>>();
        }


        public class cObjects
        {
            public class Object
            {
                //Ид объекта
                public uint ID { get; set; }

                //Позиция
                public Vector2 Position { get; set; }

                //Максимальная дистанция обнаружения
                public float MaxSeeDistance { get; set; }

                //Последний раз где объект был замечен
                public string LastSeen { get; set; } = "";
            }

            //Получить максимальную дистанцию отрисовки
            private float GetMaxDistance()
            {
                float MaxDistance = 1;

                try  //Предохраняемся)
                {
                    foreach (var objects in ObjectsDict)
                    {
                        for (int i = 0; i < objects.Value.Count; i++)
                        {
                            float currDist = objects.Value[i].MaxSeeDistance;

                            if (currDist > MaxDistance)
                            {
                                MaxDistance = currDist;
                            }
                        }
                    }
                }
                catch { }

                return (float)Math.Round(MaxDistance, 2);
            }

            //Получить список объектов в конкретной зоне
            public bool GetList(uint ZoneID, out List<Object> list)
            {
                return ObjectsDict.TryGetValue(ZoneID, out list);
            }

            //Есть ли зона в списке?
            public bool ZoneExist(uint ZoneID)
            {
                List<Object> temp = new List<Object>();
                return ObjectsDict.TryGetValue(ZoneID, out temp);
            }


            //Максимальная дист. прорисовки из всех объектов в списке
            public float MaxSeeDistance { get { return GetMaxDistance(); } }

            //Список объектов разбитые по зонам
            public Dictionary<uint, List<Object>> ObjectsDict = new Dictionary<uint, List<Object>>();
        }


        public cPlayers Players = new cPlayers();
        public cOres Ores = new cOres();
        public cObjects Objects = new cObjects();
        public cHerbs Herbs = new cHerbs();
    }



    public class DB
    {
        public static DataBase database = new DataBase();


        private const string DB_Dir = "DB";

        private const string Players_FileName = "Игроки.json";

        private const string Players_Dir = "Игроки";
        private const string Ores_Dir = "Руды";
        private const string Herbs_Dir = "Травы";
        private const string Objects_Dir = "Объекты";

        public const string PlayersPath = DB_Dir + "\\" + Players_Dir;
        public const string OresPath = DB_Dir + "\\" + Ores_Dir;
        public const string HerbsPath = DB_Dir + "\\" + Herbs_Dir;
        public const string ObjectsPath = DB_Dir + "\\" + Objects_Dir;




        //Сохранить базу данных
        public static void SaveDB()
        {
            try
            {
                const string playersPath = DB_Dir + "\\" + Players_Dir;
                const string oresPath = DB_Dir + "\\" + Ores_Dir;
                const string herbsPath = DB_Dir + "\\" + Herbs_Dir;
                const string objectsPath = DB_Dir + "\\" + Objects_Dir;

                //Проверяем, на месте ли папки?
                if (!Directory.Exists(DB_Dir))
                {
                    //Создаем заново папки
                    Directory.CreateDirectory(DB_Dir);

                    Directory.CreateDirectory(playersPath);
                    Directory.CreateDirectory(oresPath);
                    Directory.CreateDirectory(herbsPath);
                    Directory.CreateDirectory(objectsPath);
                }
                else
                {
                    if (!Directory.Exists(playersPath)) { Directory.CreateDirectory(playersPath); }
                    if (!Directory.Exists(oresPath)) { Directory.CreateDirectory(oresPath); }
                    if (!Directory.Exists(herbsPath)) { Directory.CreateDirectory(herbsPath); }
                    if (!Directory.Exists(objectsPath)) { Directory.CreateDirectory(objectsPath); }
                }
            }
            catch { Tools.MsgBox.Error("Нету доступа для создания папок!" + "\n" + "Невозможно сохранить бд!"); }



            //Первым делом сохраняем игроков
            try
            {
                string serPlayers = JsonConvert.SerializeObject(database.Players, Formatting.Indented);

                File.WriteAllText(DB_Dir + "\\" + Players_Dir + "\\" + Players_FileName, serPlayers);
            }
            catch (Exception ex)
            {
                Tools.MsgBox.Exception(ex, "Ошибка сериализации или записи списка игроков!");
            }


            //Сохраняем руды
            foreach (KeyValuePair<uint, List<DataBase.cOres.Ore>> dictVal in database.Ores.OresDict)
            {
                string zoneName = "";

                try
                {
                    uint ZoneID = dictVal.Key;

                    if (ZoneID != 0)
                    {
                        //Получаем имя зоны
                        zoneName = Enums.ZonesDB.GetTextOfZone(dictVal.Key);

                        //Сериализуем
                        string serZone = JsonConvert.SerializeObject(dictVal.Value, Formatting.Indented);

                        //Записываем в файл
                        File.WriteAllText(OresPath + "\\" + zoneName + ".json", serZone);
                    }
                }
                catch (Exception ex)
                {
                    Tools.MsgBox.Exception(ex, "Ошибка сериализации или записи списка руды для зоны: " + zoneName);
                }
            }


            //Сохраняем травы
            foreach (KeyValuePair<uint, List<DataBase.cHerbs.Herb>> dictVal in database.Herbs.HerbDict)
            {
                string zoneName = "";

                try
                {
                    uint ZoneID = dictVal.Key;

                    if (ZoneID != 0)
                    {
                        //Получаем имя зоны
                        zoneName = Enums.ZonesDB.GetTextOfZone(dictVal.Key);

                        //Сериализуем
                        string serZone = JsonConvert.SerializeObject(dictVal.Value, Formatting.Indented);


                        //Записываем в файл
                        File.WriteAllText(HerbsPath + "\\" + zoneName + ".json", serZone);
                    }
                }
                catch (Exception ex)
                {
                    Tools.MsgBox.Exception(ex, "Ошибка сериализации или записи списка трав для зоны: " + zoneName);
                }
            }


            //Сохраняем объекты
            foreach (KeyValuePair<uint, List<DataBase.cObjects.Object>> dictVal in database.Objects.ObjectsDict)
            {
                string zoneName = "";

                try
                {
                    uint ZoneID = dictVal.Key;

                    if (ZoneID != 0)
                    {
                        //Получаем имя зоны
                        zoneName = Enums.ZonesDB.GetTextOfZone(ZoneID);

                        //Сериализуем
                        string serZone = JsonConvert.SerializeObject(dictVal.Value, Formatting.Indented);


                        //Записываем в файл
                        File.WriteAllText(ObjectsPath + "\\" + zoneName + ".json", serZone);
                    }
                }
                catch (Exception ex)
                {
                    Tools.MsgBox.Exception(ex, "Ошибка сериализации или записи списка объектов для зоны: " + zoneName);
                }
            }
        }


        //Загрузить базу данных
        public static void LoadDB()
        {
            //Если папка с бд есть
            if (Directory.Exists(DB_Dir))
            {
                //Загружаем игроков
                if (Directory.Exists(PlayersPath))
                {
                    if (File.Exists(PlayersPath + "\\" + Players_FileName))
                    {
                        try
                        {
                            string readedPlayers = File.ReadAllText(PlayersPath + "\\" + Players_FileName);
                            database.Players = JsonConvert.DeserializeObject<DataBase.cPlayers>(readedPlayers);
                        }
                        catch (Exception ex) { Tools.MsgBox.Exception(ex, "Ошибка загрузки бд игроков!"); }
                    }
                }



                //Загружаем руды
                {
                    Dictionary<uint, List<DataBase.cOres.Ore>> oresDict = new Dictionary<uint, List<DataBase.cOres.Ore>>();
                    if (LoadListsFromDir(OresPath, ref oresDict))
                    {
                        database.Ores.OresDict = oresDict;
                    }
                }


                //Загружаем травы
                {
                    Dictionary<uint, List<DataBase.cHerbs.Herb>> herbsDict = new Dictionary<uint, List<DataBase.cHerbs.Herb>>();
                    if (LoadListsFromDir(HerbsPath, ref herbsDict))
                    {
                        database.Herbs.HerbDict = herbsDict;
                    }
                }


                //Загружаем объекты
                {
                    Dictionary<uint, List<DataBase.cObjects.Object>> objectsDict = new Dictionary<uint, List<DataBase.cObjects.Object>>();
                    if (LoadListsFromDir(ObjectsPath, ref objectsDict))
                    {
                        database.Objects.ObjectsDict = objectsDict;
                    }
                }
            }
        }


        //Более чем гениальная херня которую я делал!
        static bool LoadListsFromDir<T>(string path, ref Dictionary<uint, List<T>> tempDict)
        {
            //Загружаем травы
            if (Directory.Exists(path))
            {
                //Сквозь все файлы .json в папке с рудой
                FileInfo[] files = new DirectoryInfo(path).GetFiles("*.json");
                foreach (FileInfo file in files)
                {
                    try
                    {
                        //Читаем файл зоны
                        string readedZone = File.ReadAllText(file.FullName);

                        //Получаем ид зоны из имя файла
                        string zoneName = Path.GetFileNameWithoutExtension(file.Name);
                        uint ZoneID = Enums.ZonesDB.GetIdFromText(zoneName);
                        if (ZoneID != 0) //Если такая зона есть
                        {
                            //Десериализуем
                            List<T> oresForZone = JsonConvert.DeserializeObject<List<T>>(readedZone);

                            //Устанавливаем
                            tempDict[ZoneID] = oresForZone;
                        }
                    }
                    catch (Exception ex)
                    {
                        Tools.MsgBox.Exception(ex, "Ошибка загрузки файла для зоны: " + path + "\n" + "Возможно файл поврежден!");
                    }
                }

                return true;
            }

            return false;
        }


        
        //Добавить игрока или обновить его в бд
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
                        database.Players.PlayerList[i].LastPos = Tools.Vec.Round(item.LastPos, 2);

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


        //Добавить руду в бд или обновить её в бд
        public static void AddOrUpdateOre(uint ZoneID, DataBase.cOres.Ore item)
        {
            if (database.Ores.ZoneExist(ZoneID))
            {
                for (int i = 0; i < database.Ores.OresDict[ZoneID].Count; i++)
                {
                    if (database.Ores.OresDict[ZoneID][i].ID == item.ID) //Отсеиваем мусор
                    {
                        bool inRadius = Tools.Vec.InRadius(database.Ores.OresDict[ZoneID][i].Position, item.Position, Game1.settings.nodes.RadiusCheck);
                        if (inRadius)
                        {
                            //Установка позиции
                            database.Ores.OresDict[ZoneID][i].Position = item.Position;

                            //Обновляем максимальную дистанцию отрисовки
                            if (item.MaxSeeDistance > database.Ores.OresDict[ZoneID][i].MaxSeeDistance)
                            { database.Ores.OresDict[ZoneID][i].MaxSeeDistance = item.MaxSeeDistance; }

                            //Устанавливаем текущее время
                            if (item.LastSeen != "")
                            { database.Ores.OresDict[ZoneID][i].LastSeen = item.LastSeen; }


                            return;
                        }
                    }
                }

                //Если прошли цикл, значит это новая руда!
                database.Ores.OresDict[ZoneID].Add(item);
            }
            else
            {
                //Если не нашли эту зону в списке, добавляем 
                database.Ores.OresDict.Add(ZoneID, new List<DataBase.cOres.Ore>() { item });
            }
        }


        //Добавить траву в бд или обновить её в бд
        public static void AddOrUpdateHerb(uint ZoneID, DataBase.cHerbs.Herb item)
        {
            if (database.Herbs.ZoneExist(ZoneID))  //Если такая зона есть в списке зон
            {
                for (int i = 0; i < database.Herbs.HerbDict[ZoneID].Count; i++)
                {
                    if (database.Herbs.HerbDict[ZoneID][i].ID == item.ID) //Отсеиваем мусор
                    {
                        bool inRadius = Tools.Vec.InRadius(database.Herbs.HerbDict[ZoneID][i].Position, item.Position, Game1.settings.nodes.RadiusCheck);
                        if (inRadius)
                        {
                            //Установка позиции
                            database.Herbs.HerbDict[ZoneID][i].Position = item.Position;

                            //Обновляем максимальную дистанцию отрисовки
                            if (item.MaxSeeDistance > database.Herbs.HerbDict[ZoneID][i].MaxSeeDistance)
                            { database.Herbs.HerbDict[ZoneID][i].MaxSeeDistance = item.MaxSeeDistance; }

                            //Устанавливаем текущее время
                            if (item.LastSeen != "")
                            { database.Herbs.HerbDict[ZoneID][i].LastSeen = item.LastSeen; }


                            return;
                        }
                    }
                }

                //Если прошли цикл, значит это новая трава!
                database.Herbs.HerbDict[ZoneID].Add(item);
            }
            else
            {
                //Если не нашли эту зону в списке, добавляем 
                database.Herbs.HerbDict.Add(ZoneID, new List<DataBase.cHerbs.Herb>() { item });
            }
        }


        //Добавить объект в бд или обновить её в бд
        public static void AddOrUpdateObject(uint ZoneID, DataBase.cObjects.Object item)
        {
            //Не добавляем всякую парашу
            if (Enums.ObjDB.HasInBlackList(item.ID)) { return; }


            //Если такая зона есть в списке зон
            if (database.Objects.ZoneExist(ZoneID))
            {
                for (int i = 0; i < database.Objects.ObjectsDict[ZoneID].Count; i++)
                {
                    if (database.Objects.ObjectsDict[ZoneID][i].ID == item.ID) //Отсеиваем мусор
                    {
                        bool inRadius = Tools.Vec.InRadius(database.Objects.ObjectsDict[ZoneID][i].Position, item.Position, Game1.settings.nodes.RadiusCheck);
                        if (inRadius)
                        {
                            //Установка позиции
                            database.Objects.ObjectsDict[ZoneID][i].Position = item.Position;

                            //Обновляем максимальную дистанцию отрисовки
                            if (item.MaxSeeDistance > database.Objects.ObjectsDict[ZoneID][i].MaxSeeDistance)
                            { database.Objects.ObjectsDict[ZoneID][i].MaxSeeDistance = item.MaxSeeDistance; }

                            //Устанавливаем текущее время
                            if (item.LastSeen != "")
                            { database.Objects.ObjectsDict[ZoneID][i].LastSeen = item.LastSeen; }


                            return;
                        }
                    }
                }


                //Если прошли цикл, значит это новая трава!
                database.Objects.ObjectsDict[ZoneID].Add(item);
            }
            else
            {
                //Если не нашли эту зону в списке зон, добавляем 
                database.Objects.ObjectsDict.Add(ZoneID, new List<DataBase.cObjects.Object>() { item });
            }
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
