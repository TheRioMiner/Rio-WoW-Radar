using System;
using System.Diagnostics;
using System.Globalization;

using Microsoft.Xna.Framework;


namespace Rio_WoW_Radar
{
    class Tools
    {
        public static string GetNowTime()
        {
            return DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }

        public static DateTime GetTimeFromString(string Time)
        {
            return DateTime.ParseExact(Time, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        }



        public static Vector2 RoundVector2(Vector2 vector, int decimals)
        {
            return new Vector2((float)Math.Round(vector.X, decimals), (float)Math.Round(vector.Y, decimals));
        }

        public static Vector3 RoundVector3(Vector3 vector, int decimals)
        {
            return new Vector3((float)Math.Round(vector.X, decimals), (float)Math.Round(vector.Y, decimals), (float)Math.Round(vector.Z, decimals));
        }


        public static bool Vector2InRadius(Vector2 vector1, Vector2 vector2, float radius)
        {
            return Vector2.Distance(vector1, vector2) <= radius;
        }

        public static bool Vector3InRadius(Vector3 vector1, Vector3 vector2, float radius)
        {
            return Vector3.Distance(vector1, vector2) <= radius;
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
    }
}
