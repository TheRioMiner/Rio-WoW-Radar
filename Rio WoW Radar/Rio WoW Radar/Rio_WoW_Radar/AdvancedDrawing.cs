using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Rio_WoW_Radar
{
    public static class AdvancedDrawing
    {
        #region Base Texture Drawing

        //Draw texture with rotation
        public static void DrawTexture(this SpriteBatch sb, Texture2D texture, Rectangle destination, float rotation)
        {
            sb.Draw(texture, destination, null, Color.White, rotation, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);
        }

        //Draw texture with rotation and color
        public static void DrawTexture(this SpriteBatch sb, Texture2D texture, Rectangle destination, float rotation, Color color)
        {
            sb.Draw(texture, destination, null, color, rotation, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);
        }


        //Draw texture without rotation
        public static void DrawTexture(this SpriteBatch sb, Texture2D texture, Rectangle destination)
        {
            sb.Draw(texture, destination, null, Color.White, 0, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);
        }

        //Draw texture without color
        public static void DrawTexture(this SpriteBatch sb, Texture2D texture, Rectangle destination, Color color)
        {
            sb.Draw(texture, destination, null, color, 0, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);
        }


        //Draw texture without rotation
        public static void DrawOpacityTexture(this SpriteBatch sb, Texture2D texture, Rectangle destination, byte opacity)
        {
            sb.Draw(texture, destination, null, Tools.GetOpacity(Color.White, opacity), 0, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);
        }

        //Draw texture with rotation
        public static void DrawOpacityTexture(this SpriteBatch sb, Texture2D texture, Rectangle destination, float rotation, byte opacity)
        {
            sb.Draw(texture, destination, null, Tools.GetOpacity(Color.White, opacity), rotation, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);
        }

        #endregion


        #region LeftTextureDrawing

        //Draw texture without rotation
        public static void DrawTextureFromLeft(this SpriteBatch sb, Texture2D texture, Rectangle destination)
        {
            sb.Draw(texture, destination, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
        }


        //Draw texture with rotation
        public static void DrawTextureFromLeft(this SpriteBatch sb, Texture2D texture, Rectangle destination, float rotation)
        {
            sb.Draw(texture, destination, null, Color.White, rotation, Vector2.Zero, SpriteEffects.None, 0);
        }


        //Draw texture without color
        public static void DrawTextureFromLeft(this SpriteBatch sb, Texture2D texture, Rectangle destination, Color color)
        {
            sb.Draw(texture, destination, null, color, 0, Vector2.Zero, SpriteEffects.None, 0);
        }

        #endregion



        #region Advanced texture drawing

        public static void DrawTexture(this SpriteBatch sb, string textureName, Rectangle destination)
        {
            Texture2D texture = Textures.GetTexture(textureName);
            sb.Draw(texture, destination, null, Color.White, 0, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);
        }

        public static void DrawTexture(this SpriteBatch sb, string textureName, Rectangle destination, Color color)
        {
            Texture2D texture = Textures.GetTexture(textureName);
            sb.Draw(texture, destination, null, color, 0, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);
        }

        public static void DrawTexture(this SpriteBatch sb, string textureName, Rectangle destination, float rotation)
        {
            Texture2D texture = Textures.GetTexture(textureName);
            sb.Draw(texture, destination, null, Color.White, rotation, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);
        }

        #endregion



        #region Line Drawing

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, int lineWidth)
        {

            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            spriteBatch.Draw(Textures.BlankTexture, point1, null, color, angle, Vector2.Zero, new Vector2(length, lineWidth), SpriteEffects.None, 0);
        }


        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, Vector2 origin, float length, float angle, Color color, int lineWidth)
        {
            spriteBatch.Draw(Textures.BlankTexture, point1, null, color, angle, origin, new Vector2(length, lineWidth), SpriteEffects.None, 0);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, float length, float angle, Color color, int lineWidth)
        {
            spriteBatch.Draw(Textures.BlankTexture, point1, null, color, angle, Vector2.Zero, new Vector2(length, lineWidth), SpriteEffects.None, 0);
        }


        public static void DrawPolygon(this SpriteBatch spriteBatch, Vector2[] vertex, int count, Color color, int lineWidth)
        {
            if (count > 0)
            {
                for (int i = 0; i < count - 1; i++)
                {
                    DrawLine(spriteBatch, vertex[i], vertex[i + 1], color, lineWidth);
                }
                DrawLine(spriteBatch, vertex[count - 1], vertex[0], color, lineWidth);
            }
        }

        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color, int lineWidth)
        {
            Vector2[] vertex = new Vector2[4];
            vertex[0] = new Vector2(rectangle.Left, rectangle.Top);
            vertex[1] = new Vector2(rectangle.Right, rectangle.Top);
            vertex[2] = new Vector2(rectangle.Right, rectangle.Bottom);
            vertex[3] = new Vector2(rectangle.Left, rectangle.Bottom);

            DrawPolygon(spriteBatch, vertex, 4, color, lineWidth);
        }

        public static void DrawCircle(this SpriteBatch spritbatch, Vector2 center, float radius, Color color, int lineWidth, int segments = 16)
        {

            Vector2[] vertex = new Vector2[segments];

            double increment = Math.PI * 2.0 / segments;
            double theta = 0.0;

            for (int i = 0; i < segments; i++)
            {
                vertex[i] = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
                theta += increment;
            }

            DrawPolygon(spritbatch, vertex, segments, color, lineWidth);
        }
        #endregion
    }
}
