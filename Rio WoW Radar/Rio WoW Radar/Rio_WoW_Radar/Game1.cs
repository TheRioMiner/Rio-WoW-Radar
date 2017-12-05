using System;
using System.Media;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Rio_WoW_Radar
{
    public class Game1 : Game
    {
        public static Radar.Scanner scanner;

        static GraphicsDeviceManager graphics;
        static SpriteBatch spriteBatch;
        public static SpriteFont mainFont;

        public static float GlobalRotating = 0;

        public static Settings settings = new Settings();

        public static Thread BdUpdateThread = new Thread(new ThreadStart(DB_Updater));
        public static Random random = new Random();

        public static long PingTime = 0;
        public static long DrawTime = 0;
        public static long DbUpdateTime = 0;

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



        //�������������
        protected override void Initialize()
        {
            this.Window.Title = "Rio WoW Radar -";
            this.TargetElapsedTime = TimeSpan.FromMilliseconds(33.3f); //������ ��� � 30 fps
            this.IsMouseVisible = true;                               //�������� ����� ������
            GraphicsDevice.BlendState = BlendState.AlphaBlend;       //�������� ������������ � �������


            try
            {
                scanner = new Radar.Scanner();  //�������������� �������
            }
            catch
            {
                MessageBox.Show("WoW �� ����� ���� ������, ������� � ���!", "������!", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                this.Exit();
            }


            //�������������� ���
            GUI.InitializeComponent();


            //������ ��
            DB.LoadDB();

            //��������� ���
            Caching.LoadCache();

            //��������� ��������� ����� ���������� ��
            BdUpdateThread.Start();

            base.Initialize();
        }



        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //������ �����
            Sounds.LoadContent(Content);

            if (!Content.LoadContent(GraphicsDevice))//������ ���������
            {
                //���� �������, �������
                MessageBox.Show("������ �������� �������!" + Environment.NewLine + "���������� ������ ������� ����������!", "������!", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                this.Exit();
            }
        }


        protected override void UnloadContent()
        {
            //��������� ��� ���������
            Content.UnloadContent();
        }



        //��������� ����� ��� ���������� ���� ������
        public static void DB_Updater()
        {
            while (true)
            {
                Stopwatch dbTime = Stopwatch.StartNew();
                {
                    //�������� � ��������� �������
                    for (int i = 0; i < scanner.Players.Count; i++)
                    {
                        //������� ����� �� ������
                        Radar.PlayerObject player;
                        try   { player = scanner.Players[i] as Radar.PlayerObject; }
                        catch { SystemSounds.Exclamation.Play(); continue; }


                        if (player != null)  //���� ����� �� ��������
                        {
                            if (player.Name != "")  //���� ��� �����������
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


                    //�������� � ��������� ������ ������� � �.�
                    for (int i = 0; i < scanner.Objects.Count; i++)
                    {
                        //������� ����� �� ������
                        Radar.OtherObject obj;
                        try   { obj = scanner.Objects[i] as Radar.OtherObject; }
                        catch { SystemSounds.Exclamation.Play(); continue; }


                        if (obj != null)  //���� ������ �� ��������
                        {
                            //��������� ����������
                            Enums.Name_And_TextureName temp = new Enums.Name_And_TextureName();

                            if (Enums.ObjDB.GetOre(obj.ObjectId, ref temp))  //��� ����
                            {
                                DataBase.cOres.Ore oreToUpdate = new DataBase.cOres.Ore()
                                {
                                    Name = temp.name,
                                    ID = obj.ObjectId,
                                    Position = Tools.Vec.Round(new Vector2(obj.XPos, obj.YPos), 2),
                                    MaxSeeDistance = (float)Math.Round(Vector2.Distance(new Vector2(obj.XPos, obj.YPos), new Vector2(scanner.MyPlayer.XPos, scanner.MyPlayer.YPos)), 2),
                                    LastSeen = Tools.GetNowTime(),
                                };
                                DB.AddOrUpdateOre(scanner.CurrentZoneID, oreToUpdate);
                            }
                            else if (Enums.ObjDB.GetHerb(obj.ObjectId, ref temp))  //�����
                            {
                                DataBase.cHerbs.Herb herbToUpdate = new DataBase.cHerbs.Herb()
                                {
                                    Name = temp.name,
                                    ID = obj.ObjectId,
                                    Position = Tools.Vec.Round(new Vector2(obj.XPos, obj.YPos), 2),
                                    MaxSeeDistance = (float)Math.Round(Vector2.Distance(new Vector2(obj.XPos, obj.YPos), new Vector2(scanner.MyPlayer.XPos, scanner.MyPlayer.YPos)), 2),
                                    LastSeen = Tools.GetNowTime(),
                                };
                                DB.AddOrUpdateHerb(scanner.CurrentZoneID, herbToUpdate);
                            }
                            else  //��� �� ���� � �� �����, ��������� � ������ ��������!
                            {
                                DataBase.cObjects.Object objectToUpdate = new DataBase.cObjects.Object()
                                {
                                    ID = obj.ObjectId,
                                    Position = Tools.Vec.Round(new Vector2(obj.XPos, obj.YPos), 2),
                                    MaxSeeDistance = (float)Math.Round(Vector2.Distance(new Vector2(obj.XPos, obj.YPos), new Vector2(scanner.MyPlayer.XPos, scanner.MyPlayer.YPos)), 2),
                                    LastSeen = Tools.GetNowTime(),
                                };
                                DB.AddOrUpdateObject(scanner.CurrentZoneID, objectToUpdate);
                            }
                        }
                    }
                }
                dbTime.Stop();
                DbUpdateTime = dbTime.ElapsedTicks;

                Thread.Sleep(500);
            }
        }



        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                this.Exit();

            GlobalRotating += Game1.random.Next(4, 6) / 10.0f;


            //��������� ���
            GUI.Update();


            //���� ������� ����������
            if (scanner != null)
            {
                //���� ����� � ����
                HasConnected = scanner.HasConnected();
                if (HasConnected)
                {
                    HasInWorld = scanner.InWorld();
                    if (HasInWorld)
                    {
                        //��������� �����
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

            //������ ���������
            //spriteBatch.DrawMinimapOverlay(scanner.MyPlayer);

            //������������ ��� ���� � ���� � ����� ��� ����
            if (!GUI.MouseOver | !(HasConnected & HasInWorld))
            {
                if (HasConnected)
                {
                    if (HasInWorld)
                    {
                        //���������������� �������, ���� ���-�� ����� �� ���
                        {
                            if (LastGameState != 3)
                            { scanner.InitAddresses(); }

                            LastGameState = 3;
                        }



                        #region  ��������� ���, ���� � �������� �� ���� ������!!
                        {
                            #region  ��������� ���� �� ��
                            {
                                if (settings.ores.Find)  //���� ���� � ��
                                {
                                    if (DB.database.Ores.ZoneExist(scanner.CurrentZoneID)) //���� ���� ���� ���-�� � ���� ����
                                    {
                                        for (int i = 0; i < DB.database.Ores.OresDict[scanner.CurrentZoneID].Count; i++)
                                        {
                                            //������� ����� �� ������
                                            DataBase.cOres.Ore ore;
                                            try { ore = DB.database.Ores.OresDict[scanner.CurrentZoneID][i] as DataBase.cOres.Ore; }
                                            catch { continue; }


                                            //���� ���� � ������� ����������
                                            Vector2 orePos = new Vector2(ore.Position.X, ore.Position.Y);
                                            Vector2 imPos = new Vector2(scanner.MyPlayer.XPos, scanner.MyPlayer.YPos);
                                            if (Tools.Vec.InRadius(orePos, imPos, (DB.database.Ores.MaxSeeDistance * 2)))
                                            {
                                                bool breaked = false;
                                                foreach (Radar.OtherObject obj in scanner.Objects)
                                                {
                                                    if (Enums.ObjDB.HasOre(obj.ObjectId))
                                                    {
                                                        if (Tools.Vec.InRadius(orePos, new Vector2(obj.XPos, obj.YPos), settings.nodes.RadiusCheck))
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
                            }
                            #endregion


                            #region  ��������� ����� �� ��
                            {
                                if (settings.herbs.Find)  //���� ���� � ��
                                {
                                    if (DB.database.Herbs.ZoneExist(scanner.CurrentZoneID))  //���� ���� ���� ���-�� � ���� ����
                                    {
                                        for (int i = 0; i < DB.database.Herbs.HerbDict[scanner.CurrentZoneID].Count; i++)
                                        {
                                            //������� ����� �� ������
                                            DataBase.cHerbs.Herb herb;
                                            try { herb = DB.database.Herbs.HerbDict[scanner.CurrentZoneID][i] as DataBase.cHerbs.Herb; }
                                            catch { continue; }


                                            //���� ����� � ������� ����������
                                            Vector2 herbPos = new Vector2(herb.Position.X, herb.Position.Y);
                                            Vector2 imPos = new Vector2(scanner.MyPlayer.XPos, scanner.MyPlayer.YPos);
                                            if (Tools.Vec.InRadius(herbPos, imPos, (DB.database.Herbs.MaxSeeDistance * 2)))
                                            {
                                                bool breaked = false;
                                                foreach (Radar.OtherObject obj in scanner.Objects)
                                                {
                                                    if (Enums.ObjDB.HasHerb(obj.ObjectId))
                                                    {
                                                        if (Tools.Vec.InRadius(herbPos, new Vector2(obj.XPos, obj.YPos), settings.nodes.RadiusCheck))
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

                                                    if (DistanceToHerb < 5)
                                                    {

                                                    }

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
                                }
                            }
                            #endregion


                            #region ��������� ������ �������� �� ��
                            {
                                if (settings.rareobjects.Find | settings.otherobjects.Draw)  //���� ���� � ��
                                {
                                    if (DB.database.Objects.ZoneExist(scanner.CurrentZoneID))  ////���� ���� ���� ���-�� � ���� ����
                                    {
                                        for (int i = 0; i < DB.database.Objects.ObjectsDict[scanner.CurrentZoneID].Count; i++)
                                        {
                                            //������� ����� �� ������
                                            DataBase.cObjects.Object abject;
                                            try { abject = DB.database.Objects.ObjectsDict[scanner.CurrentZoneID][i] as DataBase.cObjects.Object; }
                                            catch { continue; }


                                            //���� ����� � ������� ����������
                                            Vector2 objPos = new Vector2(abject.Position.X, abject.Position.Y);
                                            Vector2 imPos = new Vector2(scanner.MyPlayer.XPos, scanner.MyPlayer.YPos);
                                            if (Tools.Vec.InRadius(objPos, imPos, (DB.database.Herbs.MaxSeeDistance * 2)))
                                            {
                                                bool breaked = false;
                                                foreach (Radar.OtherObject obj in scanner.Objects)
                                                {
                                                    if (Enums.ObjDB.HasRareObject(obj.ObjectId))
                                                    {
                                                        if (Tools.Vec.InRadius(objPos, new Vector2(obj.XPos, obj.YPos), settings.nodes.RadiusCheck))
                                                        {
                                                            breaked = true;
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (!breaked)
                                                {
                                                    Enums.Name_And_TextureName finded = new Enums.Name_And_TextureName();

                                                    //���� �������������� ������ �� �������, � ������
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
                                                    else  //��� ������� ��������
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
                                }
                            }
                            #endregion
                        }
                        #endregion



                        #region ��������� �������� ������� �����
                        foreach (Radar.OtherObject obj in scanner.Objects)
                        {
                            //���� ������������ �����
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


                            //���� ������������ ����
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


                            //���� ������������ ��������� �������
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
                        #endregion


                        //������������ �������
                        foreach (Radar.PlayerObject player in scanner.Players)
                        {
                            spriteBatch.DrawPlayer(player, scanner.MyPlayer);
                        }



                        //���� ������
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


                        //������������ ����
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


                        //������ ���� �� ������
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
            else  //����� ������ ����
            {
                GraphicsDevice.Clear(Color.AntiqueWhite);
                spriteBatch.DrawInfo();
            }



            spriteBatch.End();

            drawTime.Stop();  //������������� ������ � ��������� ��������
            DrawTime = drawTime.ElapsedTicks;

            base.Draw(gameTime);
        }



        //��� ������, ��������� ���� ������!
        protected override void OnExiting(object sender, EventArgs args)
        {
            //��������� �������� ���� ������
            DB.SaveDB();

            //��������� ���
            Caching.SaveCache();

            //��������� ���� ������ ��� ���������
            //SerializatorToShow.SaveDBtoShow();


            //������ ����� ������))  (������� ����� ����� � ������� ������� �� ���������!)
            if (BdUpdateThread != null)
            { BdUpdateThread.Abort(); }



            base.OnExiting(sender, args);
        }
    }
}
