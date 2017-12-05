using System;
using System.Collections.Generic;

namespace Rio_WoW_Radar.Enums
{
    class ObjDB
    {
        #region Руды!
        private static IDictionary<UInt32, Name_And_TextureName> OresDB = new Dictionary<UInt32, Name_And_TextureName>()
        {
            {1731,   new Name_And_TextureName("Медная жила",                           "mine_copper")},
            {3763,   new Name_And_TextureName("Медная жила",                           "mine_copper")},
            {2055,   new Name_And_TextureName("Медная жила",                           "mine_copper")},
            {181248, new Name_And_TextureName("Медная жила",                           "mine_copper")},
            {103713, new Name_And_TextureName("Медная жила",                           "mine_copper")},
            {1610,   new Name_And_TextureName("Ароматитовая жила",                     "mine_blood_iron")},
            {1667,   new Name_And_TextureName("Ароматитовая жила",                     "mine_blood_iron")},
            {1732,   new Name_And_TextureName("Оловянная жила",                        "mine_tin")},
            {3764,   new Name_And_TextureName("Оловянная жила",                        "mine_tin")},
            {2054,   new Name_And_TextureName("Оловянная жила",                        "mine_tin")},
            {181249, new Name_And_TextureName("Оловянная жила",                        "mine_tin")},
            {103711, new Name_And_TextureName("Оловянная жила",                        "mine_tin")},
            {2653,   new Name_And_TextureName("Малое месторождение кровавого камня",   "mine_blood_iron")},
            {73940,  new Name_And_TextureName("Покрытая слизью серебряная жила",       "mine_silver")},
            {1733,   new Name_And_TextureName("Серебряная жила",                       "mine_silver")},
            {105569, new Name_And_TextureName("Серебряная жила",                       "mine_silver")},
            {1735,   new Name_And_TextureName("Залежи железа",                         "mine_iron")},
            {19903,  new Name_And_TextureName("Индарилиевая жила",                     "mine_indurium")},
            {1734,   new Name_And_TextureName("Золотая жила",                          "mine_gold")},
            {150080, new Name_And_TextureName("Золотая жила",                          "mine_gold")},
            {181109, new Name_And_TextureName("Золотая жила",                          "mine_gold")},
            {73941,  new Name_And_TextureName("Покрытая слизью золотая жила",          "mine_gold")},
            {2040,   new Name_And_TextureName("Мифриловые залежи",                     "mine_mithril")},
            {150079, new Name_And_TextureName("Мифриловые залежи",                     "mine_mithril")},
            {176645, new Name_And_TextureName("Мифриловые залежи",                     "mine_mithril")},
            {123310, new Name_And_TextureName("Покрытые слизью мифриловые залежи",     "mine_mithril")},
            {2047,   new Name_And_TextureName("Залежи истинного серебра",              "mine_truesilver")},
            {181108, new Name_And_TextureName("Залежи истинного серебра",              "mine_truesilver")},
            {150081, new Name_And_TextureName("Залежи истинного серебра",              "mine_truesilver")},
            {165658, new Name_And_TextureName("Залежи черного железа",                 "mine_darkiron")},
            {123848, new Name_And_TextureName("Покрытая слизью ториевая жила",         "mine_thorium")},
            {324,    new Name_And_TextureName("Малая ториевая жила",                   "mine_thorium")},
            {150082, new Name_And_TextureName("Малая ториевая жила",                   "mine_thorium")},
            {176643, new Name_And_TextureName("Малая ториевая жила",                   "mine_thorium")},
            {180215, new Name_And_TextureName("Ториевая жила Хаккари",                 "mine_thorium")},
            {177388, new Name_And_TextureName("Покрытая слизью богатая ториевая жила", "mine_rich_thorium")},
            {175404, new Name_And_TextureName("Богатая ториевая жила",                 "mine_rich_thorium")},
            {181555, new Name_And_TextureName("Месторождение оскверненного железа",    "mine_feliron")},
            {185877, new Name_And_TextureName("Месторождение хаотита",                 "?")},
            {181069, new Name_And_TextureName("Большая обсидиановая глыба",            "?")},
            {181068, new Name_And_TextureName("Маленький кусочек обсидиана",           "?")},
            {181556, new Name_And_TextureName("Залежи адамантита",                     "mine_adamantium")},
            {189978, new Name_And_TextureName("Залежи кобальта",                       "mine_cobalt")},
            {181569, new Name_And_TextureName("Богатые залежи адамантита",             "mine_rich_adamantium")},
            {181570, new Name_And_TextureName("Богатые залежи адамантита",             "mine_rich_adamantium")},
            {185557, new Name_And_TextureName("Древняя самоцветная жила",              "?")},
            {181557, new Name_And_TextureName("Кориевая жила",                         "mine_khorium")},
            {189979, new Name_And_TextureName("Богатые залежи кобальта",               "mine_cobalt")},
            {189980, new Name_And_TextureName("Месторождение саронита",                "mine_saronite")},
            {189981, new Name_And_TextureName("Богатое месторождение саронита",        "mine_saronite")},
            {195036, new Name_And_TextureName("Месторождение чистого саронита",        "mine_saronite")},
            {191133, new Name_And_TextureName("Залежи титана",                         "mine_titanium")},
        };
        #endregion


        #region Травы!
        private static IDictionary<UInt32, Name_And_TextureName> HerbsDB = new Dictionary<UInt32, Name_And_TextureName>()
        {
            {181166, new Name_And_TextureName("Кровопийка",               "herb_bloodthistle")},
            {1618,   new Name_And_TextureName("Мироцвет",                 "herb_peacebloom")},
            {3724,   new Name_And_TextureName("Мироцвет",                 "herb_peacebloom")},
            {1617,   new Name_And_TextureName("Сребролист",               "herb_silverleaf")},
            {3725,   new Name_And_TextureName("Сребролист",               "herb_silverleaf")},
            {1619,   new Name_And_TextureName("Земляной корень",          "herb_earthroot")},
            {3726,   new Name_And_TextureName("Земляной корень",          "herb_earthroot")},
            {1620,   new Name_And_TextureName("Магороза",                 "herb_mageroyal")},
            {3727,   new Name_And_TextureName("Магороза",                 "herb_mageroyal")},
            {1621,   new Name_And_TextureName("Остротерн",                "herb_briarthorn")},
            {3729,   new Name_And_TextureName("Остротерн",                "herb_briarthorn")},
            {2045,   new Name_And_TextureName("Удавник",                  "herb_stranglekelp")},
            {1622,   new Name_And_TextureName("Синячник",                 "herb_bruiseweed")},
            {3730,   new Name_And_TextureName("Синячник",                 "herb_bruiseweed")},
            {1623,   new Name_And_TextureName("Дикий сталецвет",          "herb_wild_steelbloom")},
            {1628,   new Name_And_TextureName("Могильный мох",            "herb_grave_moss")},
            {1624,   new Name_And_TextureName("Королевская кровь",        "herb_kingsblood")},
            {2041,   new Name_And_TextureName("Корень жизни",             "herb_liferoot")},
            {2042,   new Name_And_TextureName("Бледнолист",               "herb_fadeleaf")},
            {2046,   new Name_And_TextureName("Златошип",                 "herb_goldthorn")},
            {2043,   new Name_And_TextureName("Кадгаров ус",              "herb_khadgars_whisker")},
            {2044,   new Name_And_TextureName("Морозник",                 "herb_wintersbite")},
            {2866,   new Name_And_TextureName("Огнецвет",                 "herb_firebloom")},
            {142140, new Name_And_TextureName("Лиловый лотос",            "herb_purple_lotus")},
            {180165, new Name_And_TextureName("Лиловый лотос",            "herb_purple_lotus")},
            {142141, new Name_And_TextureName("Слезы Артаса",             "herb_arthas_tears")},
            {176642, new Name_And_TextureName("Слезы Артаса",             "herb_arthas_tears")},
            {142142, new Name_And_TextureName("Солнечник",                "herb_sungrass")},
            {176636, new Name_And_TextureName("Солнечник",                "herb_sungrass")},
            {180164, new Name_And_TextureName("Солнечник",                "herb_sungrass")},
            {142143, new Name_And_TextureName("Пастушья сумка",           "herb_blindweed")},
            {183046, new Name_And_TextureName("Пастушья сумка",           "herb_blindweed")},
            {142144, new Name_And_TextureName("Призрачная поганка",       "herb_ghost_mushroom")},
            {142145, new Name_And_TextureName("Кровь Грома",              "herb_gromsblood")},
            {176637, new Name_And_TextureName("Кровь Грома",              "herb_gromsblood")},
            {176583, new Name_And_TextureName("Золотой сансам",           "herb_golden_sansam")},
            {176638, new Name_And_TextureName("Золотой сансам",           "herb_golden_sansam")},
            {180167, new Name_And_TextureName("Золотой сансам",           "herb_golden_sansam")},
            {176584, new Name_And_TextureName("Снолист",                  "herb_dreamfoil")},
            {176639, new Name_And_TextureName("Снолист",                  "herb_dreamfoil")},
            {180168, new Name_And_TextureName("Снолист",                  "herb_dreamfoil")},
            {176586, new Name_And_TextureName("Горный серебряный шалфей", "herb_mountain_silversage")},
            {176640, new Name_And_TextureName("Горный серебряный шалфей", "herb_mountain_silversage")},
            {180166, new Name_And_TextureName("Горный серебряный шалфей", "herb_mountain_silversage")},
            {176587, new Name_And_TextureName("Чумоцвет",                 "herb_plaguebloom")},
            {176641, new Name_And_TextureName("Чумоцвет",                 "herb_plaguebloom")},
            {176588, new Name_And_TextureName("Ледяной зев",              "herb_icecap")},
            {176589, new Name_And_TextureName("Черный лотос",             "herb_black_lotus")},
            {181270, new Name_And_TextureName("Сквернопля",               "herb_felweed")},
            {183044, new Name_And_TextureName("Сквернопля",               "herb_felweed")},
            {190174, new Name_And_TextureName("Мерзлая трава",            "herb_misc_flower")},
            {181271, new Name_And_TextureName("Сияние грез",              "herb_dreaming_glory")},
            {183045, new Name_And_TextureName("Сияние грез",              "herb_dreaming_glory")},
            {183043, new Name_And_TextureName("Кисейница",                "herb_ragveil")},
            {181275, new Name_And_TextureName("Кисейница",                "herb_ragveil")},
            {181277, new Name_And_TextureName("Терошишка",                "herb_terocone")},
            {181276, new Name_And_TextureName("Огненный зев",             "herb_flame_cap")},
            {181278, new Name_And_TextureName("Древний лишайник",         "herb_ancient_lichen")},
            {189973, new Name_And_TextureName("Золотой клевер",           "herb_goldclover")},
            {181279, new Name_And_TextureName("Пустоцвет",                "herb_netherbloom")},
            {185881, new Name_And_TextureName("Куст пустопраха",          "herb_netherdust")},
            {191303, new Name_And_TextureName("Огница",                   "herb_firethorn")},
            {181280, new Name_And_TextureName("Ползучий кошмарник",       "herb_nightmare_vine")},
            {181281, new Name_And_TextureName("Манаполох",                "herb_mana_thistle")},
            {190169, new Name_And_TextureName("Тигровая лилия",           "herb_tigerlily")},
            {190170, new Name_And_TextureName("Роза Таландры",            "herb_trose")},
            {191019, new Name_And_TextureName("Язык аспида",              "herb_evergreen")},
            {190173, new Name_And_TextureName("Мерзлая трава",            "herb_misc_flower")},
            {190175, new Name_And_TextureName("Мерзлая трава",            "herb_misc_flower")},
            {190171, new Name_And_TextureName("Личецвет",                 "herb_lichbloom")},
            {190172, new Name_And_TextureName("Ледошип",                  "herb_icethorn")},
            {190176, new Name_And_TextureName("Северный лотос",           "herb_frostlotus")},
        };
        #endregion


        #region Редкие объекты!
        private static IDictionary<UInt32, Name_And_TextureName> RareObjectsDB = new Dictionary<UInt32, Name_And_TextureName>()
        {
            {202082, new Name_And_TextureName("Гнездо равазавра-матриарха",  "other_rare_object")},
            {202083, new Name_And_TextureName("Гнездо острозуба-матриарха",  "other_rare_object")},
            {202080, new Name_And_TextureName("Гнездо Дарта",                "other_rare_object")},
            {202081, new Name_And_TextureName("Гнездо Такка",                "other_rare_object")},
            {179697, new Name_And_TextureName("Сундук с сокровищами арены",  "other_rare_object")},
            //{, new Name_And_TextureName("",     "other_rare_object")},
        };
        #endregion



        
        //Получить руду если она есть
        public static bool GetOre(UInt32 ObjectID, ref Name_And_TextureName Ore)
        {
            return OresDB.TryGetValue(ObjectID, out Ore);
        }

        //Этот объект руда?
        public static bool HasOre(UInt32 ObjectID)
        {
            Name_And_TextureName temp = new Name_And_TextureName();
            return OresDB.TryGetValue(ObjectID, out temp);
        }



        //Получить траву если она есть
        public static bool GetHerb(UInt32 ObjectID, ref Name_And_TextureName Herb)
        {
            return HerbsDB.TryGetValue(ObjectID, out Herb);
        }

        //Этот объект травка?
        public static bool HasHerb(UInt32 ObjectID)
        {
            Name_And_TextureName temp = new Name_And_TextureName();
            return HerbsDB.TryGetValue(ObjectID, out temp);
        }



        //Получить редкий объект если он есть
        public static bool GetRareObject(UInt32 ObjectID, ref Name_And_TextureName RareObject)
        {
            return RareObjectsDB.TryGetValue(ObjectID, out RareObject);
        }

        //Это редкий объект?
        public static bool HasRareObject(UInt32 ObjectID)
        {
            Name_And_TextureName temp = new Name_And_TextureName();
            return RareObjectsDB.TryGetValue(ObjectID, out temp);
        }


        //Это ненужный объект?
        public static bool HasInBlackList(uint ObjectID)
        {
            //Всякая параша типа кораблей и т.п
            //У них прорисовка через весь континент!
            switch (ObjectID)
            {
                case 176495:  return true;  //Дирижабль (Лиловая Принцесса)
                case 181689:  return true;  //Дирижабль Орды ("Поцелуй небес")
                case 175080:  return true;  //Дирижабль  //Ага))
                case 164871:  return true;  //Дирижабль  //ыыгыы))0
                case 20808:   return true;  //Корабль ("Девичий каприз")
                case 190536:  return true;  //Корабль, ледокол (Гордость Штормграда)
                case 181688:  return true;  //Корабль, ледокол (Северное копье)
                case 176231:  return true;  //Корабль "Леди Мели"

                case 176310:  return true;  //Безмятежный берег?!

                default:
                    return false;
            }
        }
    }
}
