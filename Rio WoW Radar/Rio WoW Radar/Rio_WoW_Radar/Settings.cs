using Microsoft.Xna.Framework;

namespace Rio_WoW_Radar
{
    public class Settings
    {
        public class Ores
        {
            public bool Draw = true;
            public bool Find = true;
            public int Size = 32;
            public Color Color = Color.DimGray;
            public float FontSize = 12.0f;
        }

        public class Herbs
        {
            public bool Draw = true;
            public bool Find = true;
            public int Size = 32;
            public Color Color = Color.LawnGreen;
            public float FontSize = 12.0f;
        }

        public class RareObjects
        {
            public bool Draw = true;
            public bool Find = true;
            public int Size = 32;
            public float FontSize = 12.0f;
        }

        public class OtherObjects
        {
            public bool Draw = false;
            public int Size = 32;
            public Color Color = Color.LightGray;
            public float FontSize = 12.0f;
        }

        public class Nodes
        {
            public float RadiusCheck = 5.0f;
            public int Size = 24;

            public float NotExist_DivideFactor = 1.16f;
        }


        public int My_Size = 32;
        public int Player_Size = 32;
        public int Npc_Size = 32;

        public byte HighLevel = 75;


        public bool NpcEnabled = true;
        public bool PlayersEnabled = true;
        public bool FriendlyPlayersEnabled = true;
        public bool ObjectsEnabled = true;

        public Nodes nodes = new Nodes();

        public Ores ores = new Ores();
        public Herbs herbs = new Herbs();
        public RareObjects rareobjects = new RareObjects();
        public OtherObjects otherobjects = new OtherObjects();
    }
}
