using System;

namespace Rio_WoW_Radar
{

#if WINDOWS || XBOX
    static class Program
    {
        public static Game1 game;

        public const int WOW_UNIT = 3;
        public const int WOW_PLAYER = 4;
        public const int WOW_OBJ = 5;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            game = new Game1();
            game.Run();
            game.Dispose();
        }
    }
#endif
}

