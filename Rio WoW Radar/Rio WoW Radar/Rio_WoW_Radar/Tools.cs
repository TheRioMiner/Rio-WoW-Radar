using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Globalization;

using Microsoft.Xna.Framework;


namespace Rio_WoW_Radar
{
    public static class Tools
    {
        public static class MsgBox
        {
            public static void Error(string text)
            { MessageBox.Show(text, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }

            public static void Error(string text, string caption)
            { MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error); }

            public static void Warning(string text)
            { MessageBox.Show(text, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

            public static void Warning(string text, string caption)
            { MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning); }

            public static void Info(string text)
            { MessageBox.Show(text, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information); }

            public static void Info(string text, string caption)
            { MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information); }


            public static void Exception(Exception ex)
            {
                MessageBox.Show("Необработаное исключение! - [" + ex.Message + " | " + ex.InnerException + "];", "Необработаное исключение!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            public static void Exception(Exception ex, string text)
            {
                MessageBox.Show(text + " - [" + ex.Message + " | " + ex.InnerException + "];", "Необрабатоное исключение!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        public static string GetNowTime()
        {
            return DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }

        public static DateTime GetTimeFromString(string Time)
        {
            return DateTime.ParseExact(Time, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        }



        public static float Oscillate(float min, float max, float value)
        {
            float range = max - min;

            float multiple = value / range;

            bool ascending = multiple % 2 == 0;
            float modulus = value % range;

            return ascending ? modulus + min : max - modulus;
        }



        public static Color GetRandomColor(Random random)
        {
            return Color.FromNonPremultiplied(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255), 255);
        }

        public static Color GetOpacity(Color color, byte Opacity)
        {
            return Color.FromNonPremultiplied(color.R, color.G, color.B, Opacity);
        }



        public static string MsFromTicks(long ticks)
        {
            return CutValue((((double)ticks / Stopwatch.Frequency) * 1000.0), 2);
        }

        public static string MicroSecondsFromTicks(long ticks)
        {
            return CutValue((((double)ticks / Stopwatch.Frequency) * 1000000.0), 1);
        }



        //Правильная обрезка значений после запятой  (правильней робит чем ToString("#.##");)
        public static string CutValue(double value, int cutCount)
        {
            string s = value.ToString();
            try
            {
                if (cutCount >= 1)
                {
                    if (s.IndexOf(',') >= 0)
                    {
                        int index = s.IndexOf(',') + cutCount + 1;
                        return s.Substring(0, index);
                    }
                    else
                    {
                        string c = "";
                        for (int i = 0; i < cutCount; i++)
                        {
                            c += "0";
                        }

                        s += "," + c;
                        return s;
                    }
                }
                else
                {
                    return s;
                }
            }
            catch { return s; }
        }


        //Радианы в градусы
        public static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        //Градусы в радианы
        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }



        public static class Vec
        {
            public static Vector2 Round(Vector2 vec, int decimals)
            {
                return new Vector2((float)Math.Round(vec.X, decimals), (float)Math.Round(vec.Y, decimals));
            }

            public static Vector3 Round(Vector3 vec, int decimals)
            {
                return new Vector3((float)Math.Round(vec.X, decimals), (float)Math.Round(vec.Y, decimals), (float)Math.Round(vec.Z, decimals));
            }


            public static bool InRadius(Vector2 vec, Vector2 vector, float radius)
            {
                return Vector2.Distance(vec, vector) <= radius;
            }

            public static float Distance(Vector2 vec, Vector2 vector)
            {
                return (float)Math.Round(Vector2.Distance(vec, vector));
            }
        }
    }
}
