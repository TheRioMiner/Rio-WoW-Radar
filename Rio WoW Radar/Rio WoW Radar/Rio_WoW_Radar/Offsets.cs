using System;

namespace Rio_WoW_Radar
{
    public class Offsets
    {
        public class Client
        {
            public const int StaticClientConnection = 0x00C79CE0;

            public const int ObjectManagerOffset = 0x2ED0;
            public const int FirstObjectOffset = 0xAC;
            public const int LocalGuidOffset = 0xC0;
            public const int NextObjectOffset = 0x3C;

            public const int HasConnectedOffset = 0x534;

            public const int MouseOverGuid = 0x00BD0798;

            public const int StaticLocalPlayerGUID = 0x00BD07A8;
            public const int StaticLocalTargetGUID = 0x00BD07B0;
            public const int StaticLastTargetGUID = 0x00BD07B0;

            public const int StaticCurrentZoneID = 0x00BCEFF0;
            public const int StaticInWorld = 0x00BD0792;

            public class Other
            {
                public const int StaticKilledMobsPointer = 0x0CCD4D34;  //0CCD4BC8 + 16C = 0CCD4D34
                public const int KilledMobsOffset1 = 0x22C;
                public const int KilledMobsOffset2 = 0x2E0;  //KilledMobsOffset1 + KilledMobsOffset2 = Address of uint

                public const int StaticLoginString = 0x00C79620;
                public const int StaticServerNameString = 0x00C79B9E;
            }
        }

        public class Name
        {
            public const int nameStore = 0x00C5D938 + 0x8;
            public const int nameMask = 0x24;
            public const int nameBase = 0x1C;
            public const int nameString = 0x20;

            public const int mobName = 0x964;
            public const int mobNameEx = 0x05C;
        }

        public class Object
        {
            public const int Type = 0x14;

            public const int Rot = 0x7A8;
            public const int Guid = 0x30;
            public const int UnitFields = 0x8;

            public const int ID = 0xC;
            public const int Pos_X = 0xEC;
            public const int Pos_Y = 0xE8;
            public const int Pos_Z = 0xF0;
        }

        public class Unit
        {
            public const int UnitFields = 0x8;

            public const int Pos_X = 0x79C;
            public const int Pos_Y = 0x798;
            public const int Pos_Z = 0x7A0;

            public const int Level = 0x36 * 4;
            public const int Health = 0x18 * 4;
            public const int Energy = 0x19 * 4;
            public const int MaxHealth = 0x20 * 4;
            public const int SummonedBy = 0xE * 4;
            public const int MaxEnergy = 0x21 * 4;

            public const int NpcID = 0x0C;

            public const int Race = 92;
            public const int Class = 93;
            public const int Gender = 94;

            public const int CurrentXP = 0x9E8;
            public const int MaxXP = 0x9EC;
        }

        public class Quest
        {
            public const int StaticQuest1 = 0x00C23680 + 0;
            public const int StaticQuest2 = 0x00C23680 + 12;
            public const int StaticQuest3 = 0x00C23680 + 24;
            public const int StaticQuest4 = 0x00C23680 + 36;
            public const int StaticQuest5 = 0x00C23680 + 48;
            public const int StaticQuest6 = 0x00C23680 + 60;
            public const int StaticQuest7 = 0x00C23680 + 72;
            public const int StaticQuest8 = 0x00C23680 + 84;
            public const int StaticQuest9 = 0x00C23680 + 96;
            public const int StaticQuest10 = 0x00C23680 + 108;
            public const int StaticQuest11 = 0x00C23680 + 120;
            public const int StaticQuest12 = 0x00C23680 + 132;
            public const int StaticQuest13 = 0x00C23680 + 144;
            public const int StaticQuest14 = 0x00C23680 + 156;
            public const int StaticQuest15 = 0x00C23680 + 168;
            public const int StaticQuest16 = 0x00C23680 + 180;
            public const int StaticQuest17 = 0x00C23680 + 192;
            public const int StaticQuest18 = 0x00C23680 + 204;
            public const int StaticQuest19 = 0x00C23680 + 216;
            public const int StaticQuest20 = 0x00C23680 + 228;
            public const int StaticQuest21 = 0x00C23680 + 240;
            public const int StaticQuest22 = 0x00C23680 + 252;
            public const int StaticQuest23 = 0x00C23680 + 264;
            public const int StaticQuest24 = 0x00C23680 + 276;
            public const int StaticQuest25 = 0x00C23680 + 288;
        }

    }
}
