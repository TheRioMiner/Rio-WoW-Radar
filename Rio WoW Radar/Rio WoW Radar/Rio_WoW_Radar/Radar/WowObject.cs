using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rio_WoW_Radar.Radar
{
    public class WowObject
    {
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

        public uint CurrentHealth = 0;
        public uint MaxHealth = 0;
        public uint CurrentEnergy = 0; // mana, rage or energy
        public uint MaxEnergy = 0;
        public uint Level = 0;
    }
}
