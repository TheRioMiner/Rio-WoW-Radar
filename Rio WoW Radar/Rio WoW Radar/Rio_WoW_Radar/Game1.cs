using System;
using System.Media;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Rio_WoW_Radar
{
    public class Game1 : Game
    {
        static GraphicsDeviceManager graphics;
        static SpriteBatch spriteBatch;
        public static SpriteFont mainFont;

        public ArrayList ToFind = new ArrayList();

        public static Settings settings = new Settings();

        public static Radar.Scanner scanner;

        public static Thread BdUpdateThread = new Thread(new ThreadStart(BD_Updater));
        public static Random random = new Random();

        public static long PingTime = 0;
        public static long DrawTime = 0;
        public static long DbTime = 0;

        public static float GlobalRotating = 0;

        public bool HasConnected = false;
        public bool HasInWorld = false;

        public int LastGameState = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 450;
            graphics.PreferredBackBufferHeight = 450;

            Content.RootDirectory = "Content";
        }


        public static float RadarZoom = 3.0f;

        public static int RadarHeight
        {
            get { return graphics.GraphicsDevice.Viewport.Height; }
        }

        public static int RadarWidth
        {
            get { return graphics.GraphicsDevice.Viewport.Width; }
        }


        //Инициализация
        protected override void Initialize()
        {
            this.Window.Title = "Rio WoW Radar -";
            this.TargetElapsedTime = TimeSpan.FromMilliseconds(33.3f); //Делаем лок в 30 fps
            this.IsMouseVisible = true;                               //Включаем показ мышьки
            GraphicsDevice.BlendState = BlendState.AlphaBlend;       //Включаем прозрачность у текстур

            try
            {
                scanner = new Radar.Scanner();  //Инициализируем сканнер
            }
            catch
            {
                MessageBox.Show("WoW не может быть открыт, зайдите в мир!", "Ошибка!", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                this.Exit();
            }


            //Инициализируем гуи
            GUI.InitializeComponent();


            //Грузим бд
            DB.LoadDB();

            //Загружаем кэш
            Caching.LoadCache();

            //Запускаем отдельный поток обновления бд
            BdUpdateThread.Start();

            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Грузим звуки
            Sounds.LoadContent(Content);

            if (!Content.LoadContent(GraphicsDevice))//Грузим текстурки
            {
                //Если неудача, выходим
                MessageBox.Show("Ошибка загрузки текстур!" + Environment.NewLine + "Попробуйте заново скачать приложение!", "Ошибка!", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                this.Exit();
            }
        }


        protected override void UnloadContent()
        {
            //Выгружаем все текстурки
            Content.UnloadContent();
        }


        //Поток для считывания всякой поеботы
        public static void BackGround_Other_Reader()
        {
            while (true)
            {
                Stopwatch otherReaderTime = Stopwatch.StartNew();
                {

                }
                otherReaderTime.Stop();

                Thread.Sleep(500);
            }
        }



        //Поток для обновления базы данных
        public static void BD_Updater()
        {
            while (true)
            {
                Stopwatch dbTime = Stopwatch.StartNew();
                {
                    if (scanner.Players.Count > 0)
                    {
                        for (int i = 0; i < scanner.Players.Count; i++)
                        {
                            //Бережно берем из списка
                            Radar.PlayerObject player;
                            try   { player = scanner.Players[i] as Radar.PlayerObject; }
                            catch { SystemSounds.Exclamation.Play(); continue; }

                            if (player != null)  //Если игрок не багнулся
                            {
                                if (player.Name != "")  //Если имя не забагалось
                                {
                                    DataBase.cPlayers.Player pl = new DataBase.cPlayers.Player()
                                    {
                                        Name = player.Name,
                                        GUID = player.Guid,
                                        LvL = (byte)player.Level,
                                        Class = player.Class,
                                        Race = player.Race,
                                        Gender = player.Gender,
                                        LastPos = new Vector3(player.XPos, player.YPos, player.ZPos),
                                        LastSeen = Tools.GetNowTime(),
                                        LastZoneID = scanner.CurrentZoneID,
                                    };
                                    DB.AddOrUpdatePlayer(pl);
                                }
                            }
                        }
                    }



                    if (scanner.Objects.Count > 0)
                    {
                        for (int i = 0; i < scanner.Objects.Count; i++)
                        {
                            //Бережно берем из списка
                            Radar.OtherObject obj;
                            try   { obj = scanner.Objects[i] as Radar.OtherObject; }
                            catch { SystemSounds.Exclamation.Play(); continue; }

                            if (obj != null)
                            {
                                //Временная переменная
                                Enums.Name_And_TextureName temp = new Enums.Name_And_TextureName();

                                if (Enums.ObjDB.GetOre(obj.ObjectId, ref temp))  //Это руда
                                {
                                    DataBase.cOres.Ore oreToUpdate = new DataBase.cOres.Ore()
                                    {
                                        Name = temp.name,
                                        ID = obj.ObjectId,
                                        Position = Tools.RoundVector3(new Vector3(obj.XPos, obj.YPos, obj.ZPos), 2),
                                        MaxSeeDistance = (float)Math.Round(Vector2.Distance(new Vector2(obj.XPos, obj.YPos), new Vector2(scanner.MyPlayer.XPos, scanner.MyPlayer.YPos)), 2),
                                        Zone = scanner.CurrentZone,
                                        LastSeen = Tools.GetNowTime(),
                                    };
                                    DB.AddOrUpdateOre(oreToUpdate);
                                }
                                else if (Enums.ObjDB.GetHerb(obj.ObjectId, ref temp))  //Трава
                                {
                                    DataBase.cHerbs.Herb herbToUpdate = new DataBase.cHerbs.Herb()
                                    {
                                        Name = temp.name,
                                        ID = obj.ObjectId,
                                        Position = Tools.RoundVector3(new Vector3(obj.XPos, obj.YPos, obj.ZPos), 2),
                                        MaxSeeDistance = (float)Math.Round(Vector2.Distance(new Vector2(obj.XPos, obj.YPos), new Vector2(scanner.MyPlayer.XPos, scanner.MyPlayer.YPos)), 2),
                                        Zone = scanner.CurrentZone,
                                        LastSeen = Tools.GetNowTime(),
                                    };
                                    DB.AddOrUpdateHerb(herbToUpdate);
                                }
                                else  //Это не руда и не трава, добавляем в список объектов!
                                {
                                    DataBase.cObjects.Object objectToUpdate = new DataBase.cObjects.Object()
                                    {
                                        ID = obj.ObjectId,
                                        Position = Tools.RoundVector3(new Vector3(obj.XPos, obj.YPos, obj.ZPos), 2),
                                        MaxSeeDistance = (float)Math.Round(Vector2.Distance(new Vector2(obj.XPos, obj.YPos), new Vector2(scanner.MyPlayer.XPos, scanner.MyPlayer.YPos)), 2),
                                        Zone = scanner.CurrentZone,
                                        LastSeen = Tools.GetNowTime(),
                                    };
                                    DB.AddOrUpdateObject(objectToUpdate);
                                }
                            }
                        }
                    }
                }
                dbTime.Stop();
                DbTime = dbTime.ElapsedTicks;

                Thread.Sleep(500);
            }
        }



        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                this.Exit();

            GlobalRotating += Game1.random.Next(4, 6) / 10.0f;


            //Обновляем гуи
            GUI.Update();


            //Если сканнер запустился
            if (scanner != null)
            {
                //Если игрок в мире
                HasConnected = scanner.HasConnected();
                if (HasConnected)
                {
                    HasInWorld = scanner.InWorld();
                    if (HasInWorld)
                    {
                        //Обновляем звуки
                        Sounds.ChecksHighLevels(scanner.Players, scanner.MyPlayer);


                        Stopwatch pingTime = Stopwatch.StartNew();
                        {
                            scanner.Ping(settings.NpcEnabled, settings.PlayersEnabled, settings.FriendlyPlayersEnabled, settings.ObjectsEnabled);
                        }
                        pingTime.Stop();
                        PingTime = pingTime.ElapsedTicks;



                        if (scanner.Target != null)
                        {
                            if (scanner.Target is Radar.PlayerObject)
                            {
                                Radar.PlayerObject player = scanner.Target as Radar.PlayerObject;
                                this.Window.Title = "Rio WoW Radar - Target[" + player.Name + ":" + player.Level + "]";
                            }
                            else if (scanner.Target is Radar.NpcObject)
                            {
                                Radar.NpcObject npc = scanner.Target as Radar.NpcObject;
                                this.Window.Title = "Rio WoW Radar - Target[" + npc.Name + ":" + npc.Level + "]";
                            }
                            else
                            {
                                this.Window.Title = "Rio WoW Radar - target has broken!       (because emlpdr!)";
                            }
                        }
                        else
                        {
                            this.Window.Title = "Rio WoW Radar - [Draw: " + Tools.MsFromTicks(DrawTime) + " ms]";
                        }
                    }
                    else
                    {
                        this.Window.Title = "Rio WoW Radar - Not in world!";
                    }
                }
                else
                {
                    this.Window.Title = "Rio WoW Radar - Not connected!";
                }
            }
            else
            {
                this.Exit();
            }

            base.Update(gameTime);
        }


    
        protected override void Draw(GameTime gameTime)
        {
            Stopwatch drawTime = Stopwatch.StartNew();
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            //Рисуем миникарту
            //spriteBatch.DrawMinimapOverlay(scanner.MyPlayer);

            //Отрисовываем гуи если в мире и мышка под меню
            if (!GUI.MouseOver | !(HasConnected & HasInWorld))
            {
                if (HasConnected)
                {
                    if (HasInWorld)
                    {
                        //Реинициализируем адресса, если что-то пошло не так
                        {
                            if (LastGameState != 3)
                            { scanner.InitAddresses(); }

                            LastGameState = 3;
                        }


                        //Отрисовка руд, трав и объектов из базы данных!!
                        {
                            //Отрисовка руды из бд
                            {
                                if (settings.ores.Find)  //Если ищем с бд
                                {
                                    for (int i = 0; i < DB.database.Ores.OresCount; i++)
                                    {
                                        //Бережно берем из списка
                                        DataBase.cOres.Ore ore;
                                        try { ore = DB.database.Ores.OreList[i] as DataBase.cOres.Ore; } catch { continue; }


                                        //Если руда в радиусе прорисовки
                                        Vector2 orePos = new Vector2(ore.Position.X, ore.Position.Y);
                                        Vector2 imPos = new Vector2(scanner.MyPlayer.XPos, scanner.MyPlayer.YPos);
                                        if (Tools.Vector2InRadius(orePos, imPos, (DB.database.Ores.MaxSeeDistance * 2)))
                                        {
                                            bool breaked = false;
                                            foreach (Radar.OtherObject obj in scanner.Objects)
                                            {
                                                if (Enums.ObjDB.HasOre(obj.ObjectId))
                                                {
                                                    if (Tools.Vector2InRadius(orePos, new Vector2(obj.XPos, obj.YPos), settings.nodes.RadiusCheck))
                                                    {
                                                        breaked = true;
                                                        break;
                                                    }
                                                }
                                            }

                                            if (!breaked)
                                            {
                                                int XPos = (int)((imPos.X - orePos.X) * RadarZoom + RadarWidth / 2);
                                                int YPos = (int)((imPos.Y - orePos.Y) * RadarZoom + RadarHeight / 2);
                                                float DistanceToOre = Vector2.Distance(orePos, imPos);

                                                if (DistanceToOre < (ore.MaxSeeDistance / 1.5f))
                                                {
                                                    float Zoom = RadarZoom / 4;
                                                    float divide_factor = settings.nodes.NotExist_DivideFactor;
                                                    int NodeSize = (int)((settings.nodes.Size * Zoom) / divide_factor);
                                                    Color textureColor = Tools.GetOpacity(Color.Red, 96);
                                                    Color textColor = Tools.GetOpacity(Color.Red, 200);
                                                    Rectangle TextureDest = new Rectangle(XPos, YPos, NodeSize, NodeSize);

                                                    spriteBatch.DrawTexture("other_notexist_node", TextureDest, textureColor);
                                                    spriteBatch.DrawText(ore.Name, new Vector2(XPos, YPos + NodeSize), (settings.ores.FontSize / divide_factor), textColor);
                                                }
                                                else
                                                {
                                                    float Zoom = RadarZoom / 4;
                                                    int NodeSize = (int)(settings.nodes.Size * Zoom);
                                                    Rectangle TextureDest = new Rectangle(XPos, YPos, NodeSize, NodeSize);

                                                    spriteBatch.DrawTexture("other_notsee_node", TextureDest, Color.DarkRed);
                                                    spriteBatch.DrawText(ore.Name, new Vector2(XPos, YPos + NodeSize), settings.ores.FontSize, Color.DarkRed);
                                                }
                                            }
                                        }
                                    }
                                }
                            }


                            //Отрисовка травы из бд
                            {
                                if (settings.herbs.Find)  //Если ищем с бд
                                {
                                    for (int i = 0; i < DB.database.Herbs.HerbsCount; i++)
                                    {
                                        //Бережно берем из списка
                                        DataBase.cHerbs.Herb herb;
                                        try { herb = DB.database.Herbs.HerbList[i] as DataBase.cHerbs.Herb; } catch { continue; }


                                        //Если трава в радиусе прорисовки
                                        Vector2 herbPos = new Vector2(herb.Position.X, herb.Position.Y);
                                        Vector2 imPos = new Vector2(scanner.MyPlayer.XPos, scanner.MyPlayer.YPos);
                                        if (Tools.Vector2InRadius(herbPos, imPos, (DB.database.Herbs.MaxSeeDistance * 2)))
                                        {
                                            bool breaked = false;
                                            foreach (Radar.OtherObject obj in scanner.Objects)
                                            {
                                                if (Enums.ObjDB.HasHerb(obj.ObjectId))
                                                {
                                                    if (Tools.Vector2InRadius(herbPos, new Vector2(obj.XPos, obj.YPos), settings.nodes.RadiusCheck))
                                                    {
                                                        breaked = true;
                                                        break;
                                                    }
                                                }
                                            }

                                            if (!breaked)
                                            {
                                                int XPos = (int)((imPos.X - herbPos.X) * RadarZoom + RadarWidth / 2);
                                                int YPos = (int)((imPos.Y - herbPos.Y) * RadarZoom + RadarHeight / 2);
                                                float DistanceToHerb = Vector2.Distance(herbPos, imPos);

                                                if (DistanceToHerb < (herb.MaxSeeDistance / 1.5f))
                                                {
                                                    float Zoom = RadarZoom / 4;
                                                    float divide_factor = settings.nodes.NotExist_DivideFactor;
                                                    int NodeSize = (int)((settings.nodes.Size * Zoom) / divide_factor);
                                                    Color textureColor = Tools.GetOpacity(Color.Red, 96);
                                                    Color textColor = Tools.GetOpacity(Color.Red, 200);
                                                    Rectangle TextureDest = new Rectangle(XPos, YPos, NodeSize, NodeSize);

                                                    spriteBatch.DrawTexture("other_notexist_node", TextureDest, textureColor);
                                                    spriteBatch.DrawText(herb.Name, new Vector2(XPos, YPos + NodeSize), (settings.herbs.FontSize / divide_factor), textColor);
                                                }
                                                else
                                                {
                                                    float Zoom = RadarZoom / 4;
                                                    int NodeSize = (int)(settings.nodes.Size * Zoom);
                                                    Rectangle TextureDest = new Rectangle(XPos, YPos, NodeSize, NodeSize);

                                                    spriteBatch.DrawTexture("other_notsee_node", TextureDest, Color.DarkRed);
                                                    spriteBatch.DrawText(herb.Name, new Vector2(XPos, YPos + NodeSize), settings.herbs.FontSize, Color.DarkRed);
                                                }
                                            }
                                        }
                                    }
                                }


                                //Отрисовка редких объектов из бд
                                {
                                    if (settings.rareobjects.Find | settings.otherobjects.Draw)  //Если ищем с бд
                                    {
                                        for (int i = 0; i < DB.database.Objects.ObjectsCount; i++)
                                        {
                                            //Бережно берем из списка
                                            DataBase.cObjects.Object abject;
                                            try { abject = DB.database.Objects.ObjectList[i] as DataBase.cObjects.Object; } catch { continue; }


                                            //Если трава в радиусе прорисовки
                                            Vector2 objPos = new Vector2(abject.Position.X, abject.Position.Y);
                                            Vector2 imPos = new Vector2(scanner.MyPlayer.XPos, scanner.MyPlayer.YPos);
                                            if (Tools.Vector2InRadius(objPos, imPos, (DB.database.Herbs.MaxSeeDistance * 2)))
                                            {
                                                bool breaked = false;
                                                foreach (Radar.OtherObject obj in scanner.Objects)
                                                {
                                                    if (Enums.ObjDB.HasRareObject(obj.ObjectId))
                                                    {
                                                        if (Tools.Vector2InRadius(objPos, new Vector2(obj.XPos, obj.YPos), settings.nodes.RadiusCheck))
                                                        {
                                                            breaked = true;
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (!breaked)
                                                {
                                                    Enums.Name_And_TextureName finded = new Enums.Name_And_TextureName();

                                                    //Если отрисовываемый объект не обычный, а редкий
                                                    if (Enums.ObjDB.GetRareObject(abject.ID, ref finded))
                                                    {
                                                        int XPos = (int)((imPos.X - objPos.X) * RadarZoom + RadarWidth / 2);
                                                        int YPos = (int)((imPos.Y - objPos.Y) * RadarZoom + RadarHeight / 2);
                                                        float DistanceToObject = Vector2.Distance(objPos, imPos);
                                                        Color randomColor = Tools.GetRandomColor(random);

                                                        if (DistanceToObject < (abject.MaxSeeDistance / 1.5f))
                                                        {
                                                            float Zoom = RadarZoom / 4;
                                                            float divide_factor = settings.nodes.NotExist_DivideFactor;
                                                            int NodeSize = (int)((settings.nodes.Size * Zoom) / divide_factor);
                                                            Color textureColor = Tools.GetOpacity(Color.Red, 96);
                                                            Color textColor = Tools.GetOpacity(Color.Red, 200);
                                                            Rectangle TextureDest = new Rectangle(XPos, YPos, NodeSize, NodeSize);

                                                            spriteBatch.DrawTexture("other_notexist_node", TextureDest, textureColor);
                                                            spriteBatch.DrawText(finded.name, new Vector2(XPos, YPos + NodeSize), (settings.rareobjects.FontSize / divide_factor), textColor);
                                                        }
                                                        else
                                                        {
                                                            float Zoom = RadarZoom / 4;
                                                            int NodeSize = (int)(settings.nodes.Size * Zoom);
                                                            Rectangle TextureDest = new Rectangle(XPos, YPos, NodeSize, NodeSize);

                                                            spriteBatch.DrawTexture("other_notsee_node", TextureDest, randomColor);
                                                            spriteBatch.DrawText(finded.name, new Vector2(XPos, YPos + NodeSize), settings.rareobjects.FontSize, randomColor);
                                                        }
                                                    }
                                                    else  //Для обычных объектов
                                                    {
                                                        if (settings.otherobjects.Draw)
                                                        {
                                                            int XPos = (int)((imPos.X - objPos.X) * RadarZoom + RadarWidth / 2);
                                                            int YPos = (int)((imPos.Y - objPos.Y) * RadarZoom + RadarHeight / 2);
                                                            float DistanceToObject = Vector2.Distance(objPos, imPos);
                                                            string id = abject.ID.ToString();

                                                            if (DistanceToObject < (abject.MaxSeeDistance / 1.5f))
                                                            {
                                                                float Zoom = RadarZoom / 4;
                                                                float divide_factor = settings.nodes.NotExist_DivideFactor;
                                                                int NodeSize = (int)((settings.nodes.Size * Zoom) / divide_factor);
                                                                Color textureColor = Tools.GetOpacity(settings.otherobjects.Color, 96);
                                                                Color textColor = Tools.GetOpacity(settings.otherobjects.Color, 200);
                                                                Rectangle TextureDest = new Rectangle(XPos, YPos, NodeSize, NodeSize);

                                                                spriteBatch.DrawTexture("other_notexist_node", TextureDest, textureColor);
                                                                spriteBatch.DrawText(id, new Vector2(XPos, YPos + NodeSize), (settings.otherobjects.FontSize / divide_factor), textColor);
                                                            }
                                                            else
                                                            {
                                                                float Zoom = RadarZoom / 4;
                                                                int NodeSize = (int)(settings.nodes.Size * Zoom);
                                                                Rectangle TextureDest = new Rectangle(XPos, YPos, NodeSize, NodeSize);

                                                                spriteBatch.DrawTexture("other_notsee_node", TextureDest, Color.DarkRed);
                                                                spriteBatch.DrawText(id, new Vector2(XPos, YPos + NodeSize), settings.otherobjects.FontSize, Color.DarkRed);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }




                                    //Теперь отрисовка объектов которые видим
                                    foreach (Radar.OtherObject obj in scanner.Objects)
                                    {
                                        //Если отрисовываем травы
                                        if (settings.herbs.Draw)
                                        {
                                            Enums.Name_And_TextureName finded = new Enums.Name_And_TextureName();
                                            if (Enums.ObjDB.GetHerb(obj.ObjectId, ref finded))
                                            {
                                                float XPos = (scanner.MyPlayer.XPos - obj.XPos) * RadarZoom + RadarWidth / 2;
                                                float YPos = (scanner.MyPlayer.YPos - obj.YPos) * RadarZoom + RadarHeight / 2;

                                                int Size = settings.herbs.Size;
                                                Texture2D Texture = Textures.GetTexture(finded.textureName);
                                                Rectangle TextureDest = new Rectangle((int)XPos, (int)YPos, Size, Size);

                                                spriteBatch.DrawTexture(Texture, TextureDest);
                                                spriteBatch.DrawText(finded.name, new Vector2(XPos, YPos + Size), settings.herbs.FontSize, settings.herbs.Color);
                                            }
                                        }


                                        //Если отрисовываем руды
                                        if (settings.ores.Draw)
                                        {
                                            Enums.Name_And_TextureName finded = new Enums.Name_And_TextureName();
                                            if (Enums.ObjDB.GetOre(obj.ObjectId, ref finded))
                                            {
                                                float XPos = (scanner.MyPlayer.XPos - obj.XPos) * RadarZoom + RadarWidth / 2;
                                                float YPos = (scanner.MyPlayer.YPos - obj.YPos) * RadarZoom + RadarHeight / 2;

                                                int Size = settings.ores.Size;
                                                Texture2D Texture = Textures.GetTexture(finded.textureName);
                                                Rectangle TextureDest = new Rectangle((int)XPos, (int)YPos, Size, Size);

                                                spriteBatch.DrawTexture(Texture, TextureDest);
                                                spriteBatch.DrawText(finded.name, new Vector2(XPos, YPos + Size), settings.ores.FontSize, settings.ores.Color);
                                            }
                                        }


                                        //Если отрисовываем остальные объекты
                                        if (settings.rareobjects.Draw)
                                        {
                                            Enums.Name_And_TextureName finded = new Enums.Name_And_TextureName();
                                            if (Enums.ObjDB.GetRareObject(obj.ObjectId, ref finded))
                                            {
                                                float XPos = (scanner.MyPlayer.XPos - obj.XPos) * RadarZoom + RadarWidth / 2;
                                                float YPos = (scanner.MyPlayer.YPos - obj.YPos) * RadarZoom + RadarHeight / 2;

                                                int Size = settings.rareobjects.Size;
                                                Texture2D Texture = Textures.GetTexture(finded.textureName);
                                                Rectangle TextureDest = new Rectangle((int)XPos, (int)YPos, Size, Size);
                                                Color randomColor = Tools.GetRandomColor(random);

                                                spriteBatch.DrawLine(new Vector2(XPos, YPos), new Vector2(Drawing.RadarCenterXPos, Drawing.RadarCenterYPos), randomColor, 2);
                                                spriteBatch.DrawTexture(Texture, TextureDest, GlobalRotating);
                                                spriteBatch.DrawText(finded.name, new Vector2(XPos, YPos + Size), settings.rareobjects.FontSize, randomColor);
                                            }
                                        }
                                    }
                                }
                            }
                        }



                        //Отрисовываем игроков
                        foreach (Radar.PlayerObject player in scanner.Players)
                        {
                            spriteBatch.DrawPlayer(player, scanner.MyPlayer);
                        }



                        //Ищем объект
                        foreach (object obj in scanner.All)
                        {
                            if (obj is Radar.OtherObject)
                            {
                                Radar.OtherObject otherObj = obj as Radar.OtherObject;


                            }
                            else if (obj is Radar.NpcObject)
                            {
                                Radar.NpcObject npc = obj as Radar.NpcObject;

                            }
                            else if (obj is Radar.PlayerObject)
                            {
                                Radar.PlayerObject player = obj as Radar.PlayerObject;

                            }
                        }


                        //Отрисовываем цель
                        if (scanner.Target != null)
                        {
                            float result = Tools.Oscillate(0.32f, 1.5f, (float)(gameTime.TotalGameTime.TotalMilliseconds / 1000.0f));
                            Texture2D target_tex = Textures.GetTexture("other_target");

                            if (scanner.Target is Radar.PlayerObject)
                            {
                                Radar.PlayerObject player = scanner.Target as Radar.PlayerObject;

                                float XPos = (scanner.MyPlayer.XPos - player.XPos) * RadarZoom + RadarWidth / 2;
                                float YPos = (scanner.MyPlayer.YPos - player.YPos) * RadarZoom + RadarHeight / 2;

                                spriteBatch.DrawLine(new Vector2(Drawing.RadarCenterXPos, Drawing.RadarCenterYPos), new Vector2(XPos, YPos), Color.SlateBlue, 4);
                                spriteBatch.DrawTexture(target_tex, new Rectangle((int)XPos, (int)YPos, (int)(target_tex.Width * result), (int)(target_tex.Height * result)), GlobalRotating / 8, Color.Gold);
                            }
                            else if (scanner.Target is Radar.NpcObject)
                            {
                                Radar.NpcObject npc = scanner.Target as Radar.NpcObject;

                                float XPos = (scanner.MyPlayer.XPos - npc.XPos) * RadarZoom + RadarWidth / 2;
                                float YPos = (scanner.MyPlayer.YPos - npc.YPos) * RadarZoom + RadarHeight / 2;

                                spriteBatch.DrawLine(new Vector2(Drawing.RadarCenterXPos, Drawing.RadarCenterYPos), new Vector2(XPos, YPos), Color.SlateBlue, 4);
                                spriteBatch.DrawTexture(target_tex, new Rectangle((int)XPos, (int)YPos, (int)(target_tex.Width * result), (int)(target_tex.Height * result)), GlobalRotating / 8, Color.Gold);
                                spriteBatch.DrawText(npc.Name, new Vector2(XPos, YPos + target_tex.Height), 14.0f, Color.Gold);
                            }
                        }


                        //Рисуем себя по центру
                        spriteBatch.DrawMeAtCenter(scanner.MyPlayer);
                    }
                    else
                    {
                        Texture2D not_in_world_tex = Textures.GetTexture("other_not_in_world");
                        spriteBatch.DrawTextureFromLeft(not_in_world_tex, new Rectangle(0, 0, RadarWidth, RadarHeight));

                        LastGameState = 2;
                    }
                }
                else
                {
                    GraphicsDevice.Clear(Color.White);
                    spriteBatch.DrawText("Not connected!", new Vector2(RadarHeight / 2, RadarWidth / 2), 25.0f, Color.Black);

                    LastGameState = 1;
                }
            }
            else  //Иначе рисуем инфу
            {
                GraphicsDevice.Clear(Color.AntiqueWhite);
                spriteBatch.DrawInfo();
            }



            spriteBatch.End();

            drawTime.Stop();  //Устанавливаем таймер и обновляем значение
            DrawTime = drawTime.ElapsedTicks;

            base.Draw(gameTime);
        }



        //При выходе, сохраняем базу данных!
        protected override void OnExiting(object sender, EventArgs args)
        {
            //Сохранить основную базу данных
            DB.SaveDB();

            //Сохраняем кэш
            Caching.SaveCache();

            //Сохранить базу данных для просмотра
            SerializatorToShow.SaveDBtoShow();


            //Делаем аборт потоку))  (главное чтобы закон о абортах потокам не запретили!)
            if (BdUpdateThread != null)
            { BdUpdateThread.Abort(); }



            base.OnExiting(sender, args);
        }
    }
}
