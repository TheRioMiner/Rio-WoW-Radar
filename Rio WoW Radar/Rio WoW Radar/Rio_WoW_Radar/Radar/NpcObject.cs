using System;

namespace Rio_WoW_Radar.Radar
{
    class NpcObject
    {
        public ulong Guid = 0;
        public uint NpcID = 0;
        public ulong SummonedBy = 0;
        public float XPos = 0;
        public float YPos = 0;
        public float ZPos = 0;
        public float Rotation = 0;
        public UIntPtr BaseAddress = UIntPtr.Zero;
        public UIntPtr UnitFieldsAddress = UIntPtr.Zero;
        public short Type = 0;
        public String Name = "";

        public uint CurrentHealth = 0;
        public uint MaxHealth = 0;
        public uint CurrentEnergy = 0; // mana, rage or energy
        public uint MaxEnergy = 0;
        public uint Level = 0;

        public bool IsDead
        {
            get { return CurrentHealth <= 0; }
        }

        public NpcObject()
        {
        }

        public NpcObject(ulong cGuid, uint cNpcID, float cXPos, float cYPos, float cZPos, float cRotation, UIntPtr cBaseAddress, UIntPtr cUnitFieldsAddress, short cType, String cName, uint cCurrentHealth, uint cMaxHealth, uint cCurrentEnergy, uint cMaxEnergy, uint cLevel)
        {
            Guid = cGuid;
            NpcID = cNpcID;
            XPos = cXPos;
            YPos = cYPos;
            ZPos = cZPos;
            Rotation = cRotation;
            BaseAddress = cBaseAddress;
            UnitFieldsAddress = cUnitFieldsAddress;
            Type = cType;
            Name = cName;
            CurrentHealth = cCurrentHealth;
            MaxHealth = cMaxHealth;
            CurrentEnergy = cCurrentEnergy;
            MaxEnergy = cMaxEnergy;
            Level = cLevel;
        }

        public object Clone()
        {
            return new NpcObject(Guid, NpcID, XPos, YPos, ZPos, Rotation, BaseAddress, UnitFieldsAddress, Type, Name, CurrentHealth, MaxHealth, CurrentEnergy, MaxEnergy, Level);
        }
    }
}
