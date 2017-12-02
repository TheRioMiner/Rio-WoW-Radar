using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Rio_WoW_Radar
{
    public class SerializatorToShow
    {
        public class DBtoShow
        {
            public class Игрок
            {
                //Имя и гуид
                public string Ник { get; set; } = "Неизвестно";
                public string Сторона { get; set; } = "";
                public string GUID { get; set; } = "";

                //Уровень добавляем вручную
                public string ЛвЛ { get; set; } = " 0 ";

                public string Раса { get; set; } = "";
                public string Класс { get; set; } = "";
                public string Пол { get; set; } = "";


                //Последняя зона где находился игрок
                public string ПоследняяЗона { get; set; } = "";

                //Последняя позиция обнаружения
                public string ПоследняяПозиция { get; set; } = "";

                //Последний раз где игрок был замечен
                public string ПоследнееОбнаружение { get; set; } = "";


                //Список уровней когда получил уровень
                public List<Уровень> Уровни { get; set; } = new List<Уровень>();

                //Обнаружения игрока
                public List<Обнаружение> Обнаружения { get; set; } = new List<Обнаружение>();
            }

            public List<Игрок> Игроки = new List<Игрок>();
        }


        #region XmlTypes
        public class Уровень
        {
            [XmlText]
            public string ЛвЛ { get; set; }

            [XmlAttribute("Дата")]
            public string Дата { get; set; }
        }

        public class Обнаружение
        {
            [XmlText]
            public string Зона { get; set; }

            [XmlAttribute("Дата")]
            public string Дата { get; set; }
        }
        #endregion


        public static void SaveDBtoShow()
        {
            DBtoShow DatabaseToShow = new DBtoShow();

            try
            {
                for (int i = 0; i < DB.database.Players.PlayerList.Count; i++)
                {
                    DataBase.cPlayers.Player player = DB.database.Players.PlayerList[i];

                    string Nick = player.Name;
                    string Team = Defines.IsHorde(player.Race) ? "Орда" : "Альянс";
                    string GUID = player.GUID != 0 ? player.GUID.ToString("#,#") : null;

                    byte LvL = player.LvL;

                    string Race = Defines.GetTextOfRace(player.Race);
                    string Class = Defines.GetTextOfClass(player.Class);
                    string Gender = player.Gender == 0 ? "М" : player.Gender == 1 ? "Ж" : null;

                    string LastPos = Tools.RoundVector3(player.LastPos, 2).ToString().Replace("{", "").Replace("}", "");
                    string LastSeen = player.LastSeen;
                    string LastZoneID = Enums.ZonesDB.GetTextOfZone(player.LastZoneID);

                    //Отражение уровней
                    List<Уровень> Levels = new List<Уровень>();
                    for (int l = 0; l < player.Levels.Count; l++)
                    {
                        Уровень lvlChanged = new Уровень() { ЛвЛ = " " + player.Levels[l].L + " ", Дата = player.Levels[l].T };
                        Levels.Add(lvlChanged);
                    }

                    //Отражение зон
                    List<Обнаружение> Seens = new List<Обнаружение>();
                    for (int z = 0; z < player.Seens.Count; z++)
                    {
                        Обнаружение zoneChanged = new Обнаружение() { Зона = Enums.ZonesDB.GetTextOfZone(player.Seens[z].I), Дата = player.Seens[z].T };
                        Seens.Add(zoneChanged);
                    }

                    DatabaseToShow.Игроки.Add(new DBtoShow.Игрок() { Ник = Nick, Сторона = Team, GUID = GUID, ЛвЛ = LvL.ToString(), Раса = Race, Класс = Class, Пол = Gender, ПоследняяПозиция = LastPos, ПоследнееОбнаружение = LastSeen, ПоследняяЗона = LastZoneID, Уровни = Levels, Обнаружения = Seens });
                }
            }
            catch (Exception ex) { MessageBox.Show("Ошибка отражения основной БД в БД для отображения: " + ex.Message); }



            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(DBtoShow));

                string dbName = "players.xml";
                if (!File.Exists(dbName))
                {
                    using (Stream fStream = new FileStream(dbName, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                    {
                        ser.Serialize(fStream, DatabaseToShow);
                        fStream.Flush();
                        fStream.Close();
                    }
                }
                else
                {
                    using (Stream fStream = new FileStream(dbName, FileMode.Truncate, FileAccess.Write, FileShare.None))
                    {
                        ser.Serialize(fStream, DatabaseToShow);
                        fStream.Flush();
                        fStream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения БД для отображения: " + ex.Message);
            }
        }

    }
}
