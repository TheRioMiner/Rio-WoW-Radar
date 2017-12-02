using System;
using System.Collections;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;


namespace Rio_WoW_Radar
{
    public static class Sounds
    {
        private static SoundEffect high_lvl_Sound;
        private static SoundEffect high_lvl_rogue_Sound;
        private static SoundEffect low_lvl_Sound;
        private static SoundEffect my_player_damaged_Sound;

        private static SoundEffectInstance high_lvl;
        private static SoundEffectInstance high_lvl_rogue;
        private static SoundEffectInstance low_lvl;
        private static SoundEffectInstance my_player_damaged;


        private static uint LastMyHP = 0;

        private const float MaxSoundDistance = 80;



        public static void LoadContent(ContentManager contentManager)
        {
            high_lvl_Sound = contentManager.Load<SoundEffect>("Sounds\\high_lvl");
            high_lvl_rogue_Sound = contentManager.Load<SoundEffect>("Sounds\\high_lvl_rogue");
            low_lvl_Sound = contentManager.Load<SoundEffect>("Sounds\\low_lvl");
            my_player_damaged_Sound = contentManager.Load<SoundEffect>("Sounds\\my_player_damaged");

            high_lvl = high_lvl_Sound.CreateInstance();
            high_lvl_rogue = high_lvl_rogue_Sound.CreateInstance();
            low_lvl = low_lvl_Sound.CreateInstance();
            my_player_damaged = my_player_damaged_Sound.CreateInstance();
            my_player_damaged.IsLooped = false;
        }



        public static void ChecksHighLevels(ArrayList players, Radar.PlayerObject im)
        {
            //Чекаем, теряем ли мы хп?
            if (im.CurrentHealth < LastMyHP)
            {
                if (my_player_damaged.State == SoundState.Stopped)
                {
                    //Нас дамажат, проигрываем эффект удара
                    my_player_damaged.Volume = 0.3f;
                    my_player_damaged.Pitch = Game1.random.Next(4, 6) / 10.0f;
                    my_player_damaged.Play();
                }
            }
            LastMyHP = im.CurrentHealth;


            ArrayList playerToCheckDistance = new ArrayList();

            bool HighLvlFinded = false;
            foreach (Radar.PlayerObject player in players)
            {
                if (player.Guid != im.Guid)
                {
                    if (!player.IsDead & !im.IsDead)
                    {
                        if (Defines.IsEnemy(player.Race, im.Race))
                        {
                            if (player.IsHighLvl)
                            {
                                playerToCheckDistance.Add(player);
                                HighLvlFinded = true;
                            }
                        }
                    }
                }
            }


            //Ищем ближайших высокий левелов
            float nearestDistance = MaxSoundDistance;
            foreach (Radar.PlayerObject player in players)
            {
                float currPlayer_Distance = Vector2.Distance(new Vector2(im.XPos, im.YPos), new Vector2(player.XPos, player.YPos));

                if (currPlayer_Distance < nearestDistance)
                {
                    nearestDistance = currPlayer_Distance;
                }
            }

            //Вычисляем громкость, чем ближе - тем громче
            float procVal = (nearestDistance / MaxSoundDistance) * 100;
            float limit = 70;
            float procents = procVal > limit ? limit : procVal;  //Ограничиваем
            float volume = Math.Abs((100 - procents) / 666);

            high_lvl_rogue.Volume = volume;
            high_lvl.Volume = volume;
            low_lvl.Volume = volume;


            //Проигрование звуки если высокий лвл
            bool findedAny = false;
            foreach (Radar.PlayerObject player in players)
            {
                if (player.Guid != im.Guid)
                {
                    if (!player.IsDead & !im.IsDead)
                    {
                        if (Defines.IsEnemy(player.Race, im.Race))
                        {
                            if (HighLvlFinded)  //Высокий уровень
                            {
                                //Если рога
                                if (player.Class == (byte)Defines.Classes.ROGUE | player.Class == (byte)Defines.Classes.DRUID)
                                {
                                    if (high_lvl_rogue.State != SoundState.Playing)
                                    {
                                        low_lvl.Stop();
                                        high_lvl.Stop();
                                        high_lvl_rogue.Play();
                                    }
                                }
                                else
                                {
                                    if (high_lvl.State != SoundState.Playing)
                                    {
                                        low_lvl.Stop();
                                        high_lvl_rogue.Stop();
                                        high_lvl.Play();
                                    }
                                }

                                findedAny = true;
                                break;
                            }
                            else if (!player.IsHighLvl) //Низкий уровень
                            {
                                if (low_lvl.State != SoundState.Playing & high_lvl_rogue.State != SoundState.Playing)
                                {
                                    high_lvl.Stop();
                                    low_lvl.Play();
                                }
                                findedAny = true;
                                break;
                            }
                        }
                    }
                }
            }


            //Если не нашло ничего, вырубаем (кроме роги)
            if (!findedAny)
            {
                low_lvl.Stop();
                high_lvl.Stop();
            }
        }
    }
}
