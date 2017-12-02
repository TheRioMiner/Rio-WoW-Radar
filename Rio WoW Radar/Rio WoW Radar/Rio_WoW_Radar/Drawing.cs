using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Rio_WoW_Radar
{
    public static class Drawing
    {
        public static float RadarCenterXPos = (((1)) * Game1.RadarZoom + Game1.RadarWidth / 2) - (Game1.RadarZoom);
        public static float RadarCenterYPos = (((1)) * Game1.RadarZoom + Game1.RadarWidth / 2) - (Game1.RadarZoom);

        public static float MainFontSize
        {
            get { return 16.0f; }
        }



        public static void DrawText(this SpriteBatch sb, string text, Vector2 pos)
        {
            try { Vector2 FontOrigin = Game1.mainFont.MeasureString(text) / 2; sb.DrawString(Game1.mainFont, text, pos, Color.White, 0, FontOrigin, MainFontSize, SpriteEffects.None, 0); }
            catch { Vector2 FontOrigin = Game1.mainFont.MeasureString("DrawText error!") / 2; sb.DrawString(Game1.mainFont, "DrawText error!", pos, Color.White, 0, FontOrigin, MainFontSize, SpriteEffects.None, 0); }
        }

        public static void DrawText(this SpriteBatch sb, string text, Vector2 pos, Color color)
        {
            try { Vector2 FontOrigin = Game1.mainFont.MeasureString(text) / 2; sb.DrawString(Game1.mainFont, text, pos, color, 0, FontOrigin, MainFontSize, SpriteEffects.None, 0); }
            catch { Vector2 FontOrigin = Game1.mainFont.MeasureString("DrawText error!") / 2; sb.DrawString(Game1.mainFont, "DrawText error!", pos, color, 0, FontOrigin, MainFontSize, SpriteEffects.None, 0); }
        }

        public static void DrawText(this SpriteBatch sb, string text, Vector2 pos, float size, Color color)
        {
            try { Vector2 FontOrigin = Game1.mainFont.MeasureString(text) / 2; sb.DrawString(Game1.mainFont, text, pos, color, 0, FontOrigin, size / MainFontSize, SpriteEffects.None, 0); }
            catch { Vector2 FontOrigin = Game1.mainFont.MeasureString("DrawText error!") / 2; sb.DrawString(Game1.mainFont, "DrawText error!", pos, color, 0, FontOrigin, size / MainFontSize, SpriteEffects.None, 0); }
        }

        public static void DrawText(this SpriteBatch sb, string text, Vector2 pos, float size)
        {
            try { Vector2 FontOrigin = Game1.mainFont.MeasureString(text) / 2; sb.DrawString(Game1.mainFont, text, pos, Color.White, 0, FontOrigin, size / MainFontSize, SpriteEffects.None, 0); }
            catch { Vector2 FontOrigin = Game1.mainFont.MeasureString("DrawText error!") / 2; sb.DrawString(Game1.mainFont, "DrawText error!", pos, Color.White, 0, FontOrigin, size / MainFontSize, SpriteEffects.None, 0); }
        }

        public static void DrawTextForInfo(this SpriteBatch sb, string text, Vector2 pos, float size)
        {
            try { sb.DrawString(Game1.mainFont, text, pos, Color.Black, 0, Vector2.Zero, size / MainFontSize, SpriteEffects.None, 0); }
            catch { sb.DrawString(Game1.mainFont, "DrawText error!", pos, Color.Black, 0, Vector2.Zero, size / MainFontSize, SpriteEffects.None, 0); }
        }



        public static void DrawInfo(this SpriteBatch sb)
        {
            string players = Game1.settings.PlayersEnabled ? Game1.scanner.Players.Count.ToString() : "Disabled";
            string npcs = Game1.settings.NpcEnabled ? Game1.scanner.Npcs.Count.ToString() : "Disabled";
            string objects = Game1.settings.ObjectsEnabled ? Game1.scanner.Objects.Count.ToString() : "Disabled";
            string currZone = Game1.scanner.CurrentZone;

            string XPos = Math.Round(Game1.scanner.MyPlayer.XPos, 2).ToString();
            string YPos = Math.Round(Game1.scanner.MyPlayer.YPos, 2).ToString();
            string ZPos = Math.Round(Game1.scanner.MyPlayer.ZPos, 2).ToString();

            string targetGuid = "";
            string npcTargetID = "";
            if (Game1.scanner.Target != null)
            {
                if (Game1.scanner.Target is Radar.PlayerObject)
                {
                    Radar.PlayerObject pl = Game1.scanner.Target as Radar.PlayerObject;
                    targetGuid = pl.Guid.ToString();
                    npcTargetID = "Player Selected!";
                }
                else if (Game1.scanner.Target is Radar.NpcObject)
                {
                    Radar.NpcObject npc = Game1.scanner.Target as Radar.NpcObject;
                    targetGuid = npc.Guid.ToString();
                    npcTargetID = npc.NpcID.ToString();
                }
            }

            sb.DrawTextForInfo("PingTime: " + Tools.MsFromTicks(Game1.PingTime) + " ms", new Vector2(8,32), 14.0f);
            sb.DrawTextForInfo("DbTime: " + Tools.MsFromTicks(Game1.DbTime) + " ms", new Vector2(8, 48), 14.0f);
            sb.DrawTextForInfo("-", new Vector2(8, 64), 14.0f);
            sb.DrawTextForInfo("TotalObjects: " + Game1.scanner.TotalWowObjects, new Vector2(8, 80), 14.0f);
            sb.DrawTextForInfo("ReadedObjects: " + Game1.scanner.ReadedWowObjects, new Vector2(8, 96), 14.0f);
            sb.DrawTextForInfo("-", new Vector2(8, 112), 14.0f);
            sb.DrawTextForInfo("Players: " + Game1.scanner.Players.Count, new Vector2(8, 128), 14.0f);
            sb.DrawTextForInfo("Npcs: " + npcs, new Vector2(8, 144), 14.0f);
            sb.DrawTextForInfo("Objects: " + objects, new Vector2(8, 160), 14.0f);
            sb.DrawTextForInfo("-", new Vector2(8, 176), 14.0f);
            sb.DrawTextForInfo("My GUID: " + Game1.scanner.MyPlayer.Guid, new Vector2(8, 192), 14.0f);
            sb.DrawTextForInfo("XPos: " + XPos, new Vector2(8, 208), 14.0f);
            sb.DrawTextForInfo("YPos: " + YPos, new Vector2(8, 224), 14.0f);
            sb.DrawTextForInfo("ZPos: " + ZPos, new Vector2(8, 240), 14.0f);
            sb.DrawTextForInfo("Current Zone: " + currZone, new Vector2(8, 256), 14.0f);
            sb.DrawTextForInfo("Target GUID: " + targetGuid, new Vector2(8, 270), 14.0f);
            sb.DrawTextForInfo("Target Npc ID: " + npcTargetID, new Vector2(8, 284), 14.0f);
        }


        //public static void DrawObject(this SpriteBatch sb, ArrayList Objects, Radar.OtherObject currObject, Radar.PlayerObject im)
        //{
        //    float XPos = (im.XPos - currObject.XPos) * Game1.RadarZoom + Game1.RadarWidth / 2;
        //    float YPos = (im.YPos - currObject.YPos) * Game1.RadarZoom + Game1.RadarHeight / 2;

        //    //Временная переменная
        //    Enums.Name_And_TextureName finded = new Enums.Name_And_TextureName();


        //    if (Enums.ObjDB.GetOre(currObject.ObjectId, ref finded))  //Это руда
        //    {
        //        //Если отрисовываем руды
        //        if (Game1.settings.ores.Draw)
        //        {
        //            Texture2D Texture = Textures.GetTexture(finded.textureName);
        //            int Size = Game1.settings.ores.Size;
        //            Rectangle TextureDest = new Rectangle((int)XPos, (int)YPos, Size, Size);

        //            sb.DrawCircle(new Vector2(XPos, YPos), (4 * Game1.RadarZoom), Color.Gold, 2, 8);
        //            sb.DrawTexture(Texture, TextureDest);
        //            sb.DrawText(finded.name, new Vector2(XPos, YPos + Size), Game1.settings.ores.FontSize, Game1.settings.ores.Color);
        //        }
        //    }
        //    else if (Enums.ObjDB.GetHerb(currObject.ObjectId, ref finded))  //Трава
        //    {
        //        //Если отрисовываем травы
        //        if (Game1.settings.herbs.Draw)
        //        {
        //            int Size = Game1.settings.herbs.Size;
        //            Texture2D Texture = Textures.GetTexture(finded.textureName);
        //            Rectangle TextureDest = new Rectangle((int)XPos, (int)YPos, Size, Size);

        //            sb.DrawCircle(new Vector2(XPos, YPos), (4 * Game1.RadarZoom), Color.Gold, 2, 8);
        //            sb.DrawTexture(Texture, TextureDest);
        //            sb.DrawText(finded.name, new Vector2(XPos, YPos + Size), Game1.settings.herbs.FontSize, Game1.settings.herbs.Color);
        //        }
        //    }
        //    else if (Enums.ObjDB.GetRareObject(currObject.ObjectId, ref finded))  //Редкий объект
        //    {
        //        //Если отрисовываем редкие объекты
        //        if (Game1.settings.rareobjects.Draw)
        //        {
        //            int Size = Game1.settings.rareobjects.Size;
        //            Color randomColor = Tools.GetRandomColor(Game1.random);
        //            Texture2D Texture = Textures.GetTexture(finded.textureName);
        //            Rectangle TextureDest = new Rectangle((int)XPos, (int)YPos, Size, Size);

        //            sb.DrawLine(new Vector2(RadarCenterXPos, RadarCenterYPos), new Vector2(XPos, YPos), randomColor, 2);

        //            sb.DrawTexture(Texture, TextureDest, Game1.GlobalRotating);
        //            sb.DrawText(finded.name, new Vector2(XPos, YPos + Size), Game1.settings.rareobjects.FontSize, randomColor);
        //        }
        //    }
        //    else  //Остальные объекты
        //    {
        //        //Если отрисовываем остальные объекты
        //        if (Game1.settings.otherobjects.Draw)
        //        {
        //            sb.DrawLine(new Vector2(RadarCenterXPos, RadarCenterYPos), new Vector2(XPos, YPos), Game1.settings.otherobjects.Color, 1);
        //            sb.DrawText(currObject.ObjectId.ToString(), new Vector2(XPos, YPos), 12.0f, Color.White);
        //        }
        //    }
        //}


        public static void DrawPlayer(this SpriteBatch sb, Radar.PlayerObject player, Radar.PlayerObject im)
        {
            bool IsEnemy = Defines.IsEnemy(player.Race, im.Race);
            float XPos = (im.XPos - player.XPos) * Game1.RadarZoom + Game1.RadarWidth / 2;
            float YPos = (im.YPos - player.YPos) * Game1.RadarZoom + Game1.RadarHeight / 2;
            float Rotation = -player.Rotation;
            float Zoom = Game1.RadarZoom / 3.2f;
            Color color = Tools.GetRandomColor(Game1.random);
            int Size = Game1.settings.Player_Size;

            Texture2D class_tex = Textures.GetTextureByClass(player.Class);
            Texture2D arrow_tex = Textures.GetTexture("other_arrow");

            //Calc destinations for textures
            Rectangle ClassDest = new Rectangle((int)XPos, (int)YPos, (int)(class_tex.Width * Zoom), (int)(class_tex.Height * Zoom));
            Rectangle ArrowDest = new Rectangle((int)XPos, (int)YPos, (int)(arrow_tex.Width * (Zoom / 1.4f)), (int)(arrow_tex.Height * (Zoom / 1.4f)));


            //Рисуем линию взгляда если высокий лвл
            if (IsEnemy)
            {
                if (player.IsHighLvl)
                {
                    sb.DrawLine(new Vector2(XPos, YPos), Game1.RadarWidth * 1.5f, (Rotation - 1.577f), Color.Red, 2);
                    sb.DrawText(player.Name + ":" + player.Level.ToString(), new Vector2(XPos, YPos + Size), 15.0f, color);
                }
                else
                {
                    float medium_lenght = (class_tex.Width / 2) + (class_tex.Height / 2);
                    sb.DrawLine(new Vector2(XPos, YPos), (medium_lenght) * Zoom + (medium_lenght / 2), (Rotation - 1.577f), Color.Red, 2);
                    sb.DrawText(player.Name + ":" + player.Level.ToString(), new Vector2(XPos, YPos + Size), 14.0f, Color.Red);
                }
            }
            else
            {
                float medium_lenght = (class_tex.Width / 2) + (class_tex.Height / 2);
                sb.DrawLine(new Vector2(XPos, YPos), (medium_lenght) * Zoom, (Rotation - 1.577f), Color.LightSkyBlue, 2);
                sb.DrawText(player.Name + ":" + player.Level.ToString(), new Vector2(XPos, YPos + Size), 14.0f, Color.Gold);
            }



            if (player.IsDead)
            {
                sb.DrawOpacityTexture(class_tex, ClassDest, 120);
            }
            else
            {
                //Draw class
                sb.DrawTexture(class_tex, ClassDest);


                //And finally arrow of sight
                if (IsEnemy)
                {
                    sb.DrawTexture(arrow_tex, ArrowDest, Rotation, Color.Red);
                }
                else
                {
                    sb.DrawTexture(arrow_tex, ArrowDest, Rotation, Color.LightSkyBlue);
                }
            }
        }


        public static void DrawMeAtCenter(this SpriteBatch sb, Radar.PlayerObject im)
        {
            float Rotation = -im.Rotation;
            float Zoom = Game1.RadarZoom / 4;

            Texture2D main_tex = Textures.GetTextureByRaceAndGender(im.Race, im.Gender);
            Texture2D arrow_tex = Textures.GetTexture("other_arrow");

            //Calc destinations for textures
            Rectangle ClassDest = new Rectangle((int)RadarCenterXPos, (int)RadarCenterYPos, (int)(main_tex.Width * Zoom), (int)(main_tex.Height * Zoom));
            Rectangle ArrowDest = new Rectangle((int)RadarCenterXPos, (int)RadarCenterYPos, (int)(arrow_tex.Width * Zoom), (int)(arrow_tex.Height * Zoom));

            float medium_lenght = (main_tex.Width / 2) + (main_tex.Height / 2);
            sb.DrawLine(new Vector2(RadarCenterXPos, RadarCenterYPos), (medium_lenght) * Zoom, (Rotation - 1.577f), Color.Black, 2);

            //Draw class
            sb.DrawTexture(main_tex, ClassDest);

            //And finally arrow of sight
            sb.DrawTexture(arrow_tex, ArrowDest, Rotation, Color.Black);
        }
    }




    //Класс для отрисовки миникарты  (кто поможет с отрисовкой миникарты посоны? у меня чет не получается)
    public static class Minimap_Drawing
    {
        const float TILE_SCALE_FACTOR = 533.5F;
        const int TILE_HEIGHT = 256;

        public static int ZoomFactor
        {
            get { return (int)(TILE_HEIGHT * (Game1.RadarZoom / 0.5f)); }
        }
        

    }
}
