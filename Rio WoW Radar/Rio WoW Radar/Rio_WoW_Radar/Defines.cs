namespace Rio_WoW_Radar
{
    class Defines
    {
        //Rio WoW Radar defines for 3.3.5a



        //Получить из значения имя расы
        public static string GetTextOfRace(byte Race)
        {
            switch (Race)
            {
                case 1:  return "Человек";
                case 2:  return "Орк";
                case 3:  return "Дворф";
                case 4:  return "Ночной эльф";
                case 5:  return "Нежить";
                case 6:  return "Таурен";
                case 7:  return "Гном";
                case 8:  return "Тролль";
                case 10: return "Эльф крови";
                case 11: return "Дреней";

                default:
                    return "";
            }
        }


        //Получить из значения имя класса
        public static string GetTextOfClass(byte Class)
        {
            switch (Class)
            {
                case 1:  return "Воин";
                case 2:  return "Паладин";
                case 3:  return "Охотник";
                case 4:  return "Разбойник";
                case 5:  return "Жрец";
                case 6:  return "Рыцарь Смерти";
                case 7:  return "Шаман";
                case 8:  return "Маг";
                case 9:  return "Чернокнижник";
                case 11: return "Друид";

                default:
                    return "";
            }
        }


        public enum Gender
        {
            MALE = 0,        //Мужик
            FEMALE = 1,      //Баба
            NONE = 2         //Нету пола
        };


        //Расы игроков
        public enum Races
        {
            HUMAN = 1,        //Человек
            ORC = 2,          //Орк
            DWARF = 3,        //Дворф
            NIGHTELF = 4,     //Ночной эльф
            UNDEAD = 5,       //Нежить
            TAUREN = 6,       //Таурен
            GNOME = 7,        //Гном
            TROLL = 8,        //Тролль
            BLOODELF = 10,    //Эльф крови
            DRAENEI = 11,     //Дреней
        };


        //Все классы игроков
        public enum Classes
        {
            WARRIOR = 1,       //Воин
            PALADIN = 2,       //Паладин
            HUNTER = 3,        //Охотник
            ROGUE = 4,         //Разбойник
            PRIEST = 5,        //Жрец
            DEATH_KNIGHT = 6,  //Рыцарь смерти
            SHAMAN = 7,        //Шаман
            MAGE = 8,          //Маг
            WARLOCK = 9,       //Чернокнижник
            DRUID = 11,        //Друид
        };


        //При помощи расы возвращяет орда ли это
        public static bool IsHorde(byte i)
        {
            switch (i)
            {
                case 1: return false; //Человек
                case 2: return true;  //Орк
                case 3: return false; //Дворф
                case 4: return false; //Ночной эльф
                case 5: return true;  //Нежить
                case 6: return true;  //Таурен
                case 7: return false; //Гном
                case 8: return true;  //Тролль
                case 9: return true;  //Гоблин?!
                case 10: return true;  //Эльф крови
                case 11: return false; //Дреней

                default:  //Вне диапазона??
                    return false;
            }
        }

        //Определить, противник ли этот игрок?
        public static bool IsEnemy(byte playerRace, byte myRace)
        {
            return (IsHorde(playerRace) ^ IsHorde(myRace));
        }


        //Все типы объектов
        public enum ObjectType
        {
            ITEM = 1,
            CONTAINER = 2,
            UNIT = 3,
            PLAYER = 4,
            GAMEOBJ = 5,
            DYNOBJ = 6,
            CORPSE = 7,
        };
    }
}
