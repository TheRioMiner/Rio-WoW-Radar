using System;

namespace Rio_WoW_Radar.Radar
{
    public class PlayerObject : ICloneable
    {
        // general properties
        public ulong Guid = 0;
        public ulong SummonedBy = 0;
        public float XPos = 0;
        public float YPos = 0;
        public float ZPos = 0;
        public float Rotation = 0;
        public UIntPtr BaseAddress = UIntPtr.Zero;
        public UIntPtr UnitFieldsAddress = UIntPtr.Zero;
        public short Type = 0;
        public String Name = "";


        public byte Race = 0;
        public byte Class = 0;
        public byte Gender = 2;


        // more specialised properties (player or mob)
        public uint CurrentHealth = 0;
        public uint MaxHealth = 0;
        public uint CurrentEnergy = 0; // mana, rage and energy will all fall under energy.
        public uint MaxEnergy = 0;
        public uint Level = 0;

        public bool IsHighLvl
        {
            get { return Level >= Game1.settings.HighLevel; }
        }

        public bool IsDead
        {
            get { return CurrentHealth <= 0; }
        }

        public bool IsEnemy(byte MyPlayerRace)
        {
            return (Defines.IsHorde(Race) ^ Defines.IsHorde(MyPlayerRace));
        }



        public PlayerObject()
        {
        }

        public PlayerObject(ulong cGuid, float cXPos, float cYPos, float cZPos, float cRotation, UIntPtr cBaseAddress, UIntPtr cUnitFieldsAddress, short cType, String cName, byte cRace, byte cClass, byte cGender, uint cCurrentHealth, uint cMaxHealth, uint cCurrentEnergy, uint cMaxEnergy, uint cLevel)
        {
            Guid = cGuid;
            XPos = cXPos;
            YPos = cYPos;
            ZPos = cZPos;
            Rotation = cRotation;
            BaseAddress = cBaseAddress;
            UnitFieldsAddress = cUnitFieldsAddress;
            Type = cType;
            Name = cName;
            Race = cRace;
            Class = cClass;
            Gender = cGender;
            CurrentHealth = cCurrentHealth;
            MaxHealth = cMaxHealth;
            CurrentEnergy = cCurrentEnergy;
            MaxEnergy = cMaxEnergy;
            Level = cLevel;
        }

        public object Clone()
        {
            return new PlayerObject(Guid, XPos, YPos, ZPos, Rotation, BaseAddress, UnitFieldsAddress, Type, Name, Race, Class, Gender, CurrentHealth, MaxHealth, CurrentEnergy, MaxEnergy, Level);
        }
    }
}
