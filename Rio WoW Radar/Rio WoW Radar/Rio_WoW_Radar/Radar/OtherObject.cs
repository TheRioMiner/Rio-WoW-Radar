using System;

namespace Rio_WoW_Radar.Radar
{
    public class OtherObject
    {
        public uint ObjectId = 0;
        public float XPos = 0;
        public float YPos = 0;
        public float ZPos = 0;
        //public float Rotation = 0;
        public UIntPtr BaseAddress = UIntPtr.Zero;
        public UIntPtr UnitFieldsAddress = UIntPtr.Zero;


        public OtherObject()
        {
        }

        public OtherObject(uint cObjectId, float cXPos, float cYPos, float cZPos, UIntPtr cBaseAddress, UIntPtr cUnitFieldsAddress)
        {
            ObjectId = cObjectId;
            XPos = cXPos;
            YPos = cYPos;
            ZPos = cZPos;
            //Rotation = cRotation;
            BaseAddress = cBaseAddress;
            UnitFieldsAddress = cUnitFieldsAddress;
        }
    }
}
