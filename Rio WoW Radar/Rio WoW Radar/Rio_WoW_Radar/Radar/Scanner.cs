using System;
using System.Collections;

namespace Rio_WoW_Radar.Radar
{
    public class Scanner
    {
        Memory WowReader = new Memory();

        UIntPtr ClientConnection = UIntPtr.Zero;
        UIntPtr ObjectManager = UIntPtr.Zero;
        UIntPtr FirstObject = UIntPtr.Zero;

        public uint TotalWowObjects = 0;
        public uint ReadedWowObjects = 0;

        public PlayerObject MyPlayer = new PlayerObject();
        public object Target = new PlayerObject();
        public ulong TargetGuid = 0;
        public ulong LastTargetGuid = 0;
        public WowObject TempObject = new WowObject();

        public ArrayList All = new ArrayList();
        public ArrayList Players = new ArrayList();
        public ArrayList Npcs = new ArrayList();
        public ArrayList Objects = new ArrayList();

        public uint KilledMobs = 0;
        public uint LastKilledMobs = uint.MaxValue;

        public uint CurrentZoneID = 0;
        public string CurrentZoneName = "";

      
        public Scanner()
        {
            WowReader.SetProcess("Wow", Memory.Mode.READ);

            InitAddresses();
        }

        public Scanner(int pid)
        {
            WowReader.SetProcess(pid, Memory.Mode.READ);

            InitAddresses();
        }

        ~Scanner()
        {
            WowReader.CloseProcess();
        }


        public bool InWorld()
        {
            try
            {
                return WowReader.ReadInt32((UIntPtr)Offsets.Client.StaticInWorld) == 1;
            }
            catch
            {
                return false;
            }
        }


        public bool HasConnected()
        {
            try
            {
                return WowReader.ReadByte(ClientConnection + Offsets.Client.HasConnectedOffset) == 5;
            }
            catch
            {
                return false;
            }
        }


        public uint GetCurrZoneID()
        {
            try
            {
                return WowReader.ReadUInt32((UIntPtr)Offsets.Client.StaticCurrentZoneID);
            }
            catch
            {
                return 0;
            }
        }


        public void InitAddresses()
        {
            ClientConnection = WowReader.ReadPointer((UIntPtr)Offsets.Client.StaticClientConnection);
            ObjectManager = WowReader.ReadPointer(ClientConnection + Offsets.Client.ObjectManagerOffset);
            FirstObject = WowReader.ReadPointer(ObjectManager + Offsets.Client.FirstObjectOffset);
            MyPlayer.Guid = WowReader.ReadUInt64(ObjectManager + Offsets.Client.LocalGuidOffset);
        }


        public void Ping(bool NpcEnabled, bool PlayersEnabled, bool FriendlyPlayersEnabled, bool ObjectsEnabled)
        {
            //Обнуляем и обнуляем все
            bool MyPlayersFinded = false;
            TotalWowObjects = 0;
            ReadedWowObjects = 0;
            All.Clear();
            Players.Clear();
            Npcs.Clear();
            Objects.Clear();

            //Устанавливаем базовый адресс текущего объекта как адресс первого объекта в менеджере объектов
            UIntPtr CurrObject_BaseAddress = FirstObject;



            //Получаем гуид последнего выбранного юнита
            LastTargetGuid = WowReader.ReadULong((UIntPtr)Offsets.Client.StaticLastTargetGUID);


            //Получаем текущую зону
            CurrentZoneID = GetCurrZoneID();
            CurrentZoneName = Enums.ZonesDB.GetTextOfZone(CurrentZoneID);


            //Читаем нашего игрока
            ReadMyPlayer();



            //ПОЛУЧАЕМ ТЕКУЩУЮ ЦЕЛЬ
            {
                ulong Guid = WowReader.ReadUInt64((UIntPtr)Offsets.Client.StaticLocalTargetGUID);

                if (Guid != 0)
                {
                    UIntPtr BaseAddress = GetObjectBaseByGuid(Guid);
                    UIntPtr UnitFieldsAddress = WowReader.ReadPointer(BaseAddress + Offsets.Object.UnitFields);
                    short Type = (short)WowReader.ReadUInt32(BaseAddress + Offsets.Object.Type);

                    if (Type == (int)Defines.ObjectType.PLAYER)  //PLAYER
                    {
                        float XPos = WowReader.ReadFloat(BaseAddress + Offsets.Unit.Pos_X);
                        float YPos = WowReader.ReadFloat(BaseAddress + Offsets.Unit.Pos_Y);
                        float ZPos = WowReader.ReadFloat(BaseAddress + Offsets.Unit.Pos_Z);
                        float Rotation = WowReader.ReadFloat(BaseAddress + Offsets.Object.Rot);

                        uint CurrentHealth = WowReader.ReadUInt32(UnitFieldsAddress + Offsets.Unit.Health);
                        uint CurrentEnergy = WowReader.ReadUInt32(UnitFieldsAddress + Offsets.Unit.Energy);
                        uint MaxHealth = WowReader.ReadUInt32(UnitFieldsAddress + Offsets.Unit.MaxHealth);
                        uint Level = WowReader.ReadUInt32(UnitFieldsAddress + Offsets.Unit.Level);
                        uint MaxEnergy = WowReader.ReadUInt32(UnitFieldsAddress + Offsets.Unit.MaxEnergy);


                        string Name = PlayerNameFromGuid(Guid);
                        byte Race = WowReader.ReadByte(UnitFieldsAddress + Offsets.Unit.Race);
                        byte Class = WowReader.ReadByte(UnitFieldsAddress + Offsets.Unit.Class);
                        byte Gender = WowReader.ReadByte(UnitFieldsAddress + Offsets.Unit.Gender);

                        Target = new PlayerObject(Guid, XPos, YPos, ZPos, Rotation, CurrObject_BaseAddress, UnitFieldsAddress, Type, Name, Race, Class, Gender, CurrentHealth, MaxHealth, CurrentEnergy, MaxEnergy, Level);
                    }
                    else if (Type == (int)Defines.ObjectType.UNIT)  //NPC
                    {
                        uint NpcID = WowReader.ReadUInt32(UnitFieldsAddress + Offsets.Unit.NpcID);

                        float XPos = WowReader.ReadFloat(BaseAddress + Offsets.Unit.Pos_X);
                        float YPos = WowReader.ReadFloat(BaseAddress + Offsets.Unit.Pos_Y);
                        float ZPos = WowReader.ReadFloat(BaseAddress + Offsets.Unit.Pos_Z);
                        float Rotation = WowReader.ReadFloat(BaseAddress + Offsets.Object.Rot);

                        uint CurrentHealth = WowReader.ReadUInt32(UnitFieldsAddress + Offsets.Unit.Health);
                        uint CurrentEnergy = WowReader.ReadUInt32(UnitFieldsAddress + Offsets.Unit.Energy);
                        uint MaxHealth = WowReader.ReadUInt32(UnitFieldsAddress + Offsets.Unit.MaxHealth);
                        uint Level = WowReader.ReadUInt32(UnitFieldsAddress + Offsets.Unit.Level);
                        uint MaxEnergy = WowReader.ReadUInt32(UnitFieldsAddress + Offsets.Unit.MaxEnergy);

                        string Name = MobNameFromGuid(Guid, NpcID);
                        ulong SummonedBy = WowReader.ReadUInt64(UnitFieldsAddress + Offsets.Unit.SummonedBy);

                        Target = new NpcObject(Guid, NpcID, XPos, YPos, ZPos, Rotation, CurrObject_BaseAddress, UnitFieldsAddress, Type, Name, CurrentHealth, MaxHealth, CurrentEnergy, MaxEnergy, Level);
                    }
                }
                else
                {
                    Target = null;
                }
            }


            //Прочитываем объектный менеджер из первого объекта по последний.
            while (CurrObject_BaseAddress != UIntPtr.Zero && (uint)CurrObject_BaseAddress % 2 == 0)
            {
                TotalWowObjects++;

                //Получаем тип объекта и пропускаем если это неизвестный объект
                short Type = WowReader.ReadShort(CurrObject_BaseAddress + Offsets.Object.Type);
                if (Type != 3 & Type != 4 & Type != 5) { goto Skip; }  //Скипаем если тип объекта неизвестен

                //Получаем указатель на поле значений этого объекта
                UIntPtr UnitFieldsAddress = WowReader.ReadPointer(CurrObject_BaseAddress + Offsets.Unit.UnitFields);

                switch (Type)
                {
                    case (int)Defines.ObjectType.UNIT: //NPC!
                        {
                            if (!NpcEnabled) { goto Skip; }  //Если нпс выключены, пропускаем

                            ulong Guid = WowReader.ReadUInt64(CurrObject_BaseAddress + Offsets.Object.Guid);
                            uint NpcID = WowReader.ReadUInt32(UnitFieldsAddress + Offsets.Unit.NpcID);

                            float XPos = WowReader.ReadFloat(CurrObject_BaseAddress + Offsets.Unit.Pos_X);
                            float YPos = WowReader.ReadFloat(CurrObject_BaseAddress + Offsets.Unit.Pos_Y);
                            float ZPos = WowReader.ReadFloat(CurrObject_BaseAddress + Offsets.Unit.Pos_Z);
                            float Rotation = WowReader.ReadFloat(CurrObject_BaseAddress + Offsets.Object.Rot);
                            uint Level = WowReader.ReadUInt32(UnitFieldsAddress + Offsets.Unit.Level);

                            uint CurrentHealth = WowReader.ReadUInt32(UnitFieldsAddress + Offsets.Unit.Health);
                            uint MaxHealth = WowReader.ReadUInt32(UnitFieldsAddress + Offsets.Unit.MaxHealth);
                            uint CurrentEnergy = WowReader.ReadUInt32(UnitFieldsAddress + Offsets.Unit.Energy);
                            uint MaxEnergy = WowReader.ReadUInt32(UnitFieldsAddress + Offsets.Unit.MaxEnergy);


                            string Name = MobNameFromGuid(Guid, NpcID);
                            ulong SummonedBy = WowReader.ReadUInt64(UnitFieldsAddress + Offsets.Unit.SummonedBy);

                            ReadedWowObjects++;
                            NpcObject npc = new NpcObject(Guid, NpcID, XPos, YPos, ZPos, Rotation, CurrObject_BaseAddress, UnitFieldsAddress, Type, Name, CurrentHealth, MaxHealth, CurrentEnergy, MaxEnergy, Level);
                            Npcs.Add(npc);
                            All.Add(npc);
                        }
                        break;
                    case (int)Defines.ObjectType.PLAYER: //PLAYER
                        {
                            ulong Guid = WowReader.ReadUInt64(CurrObject_BaseAddress + Offsets.Object.Guid);

                            //Проходим если это мы
                            if (Guid == MyPlayer.Guid)
                            {
                                MyPlayersFinded = true;
                                goto Skip;
                            } 

                            //Если игроки выключены, пропускаем
                            if (!PlayersEnabled) { goto Skip; }

                            byte Race = WowReader.ReadByte(UnitFieldsAddress + Offsets.Unit.Race);

                            //Скипаем союзников, если такая настройка включена
                            if (!FriendlyPlayersEnabled & !Defines.IsEnemy(Race, MyPlayer.Race)) { goto Skip; }


                            float XPos = WowReader.ReadFloat(CurrObject_BaseAddress + Offsets.Unit.Pos_X);
                            float YPos = WowReader.ReadFloat(CurrObject_BaseAddress + Offsets.Unit.Pos_Y);
                            float ZPos = WowReader.ReadFloat(CurrObject_BaseAddress + Offsets.Unit.Pos_Z);
                            float Rotation = WowReader.ReadFloat(CurrObject_BaseAddress + Offsets.Object.Rot);
                            uint Level = WowReader.ReadUInt32(UnitFieldsAddress + Offsets.Unit.Level);

                            uint CurrentHealth = WowReader.ReadUInt32(UnitFieldsAddress + Offsets.Unit.Health);
                            uint MaxHealth = WowReader.ReadUInt32(UnitFieldsAddress + Offsets.Unit.MaxHealth);
                            uint CurrentEnergy = WowReader.ReadUInt32(UnitFieldsAddress + Offsets.Unit.Energy);
                            uint MaxEnergy = WowReader.ReadUInt32(UnitFieldsAddress + Offsets.Unit.MaxEnergy);
                            byte Class = WowReader.ReadByte(UnitFieldsAddress + Offsets.Unit.Class);
                            byte Gender = WowReader.ReadByte(UnitFieldsAddress + Offsets.Unit.Gender);
                            string Name = PlayerNameFromGuid(Guid);

                            if (Name == "Yeah")
                            { }

                            //Добавляем игрока в список игроков
                            ReadedWowObjects++;
                            PlayerObject player = new PlayerObject(Guid, XPos, YPos, ZPos, Rotation, CurrObject_BaseAddress, UnitFieldsAddress, Type, Name, Race, Class, Gender, CurrentHealth, MaxHealth, CurrentEnergy, MaxEnergy, Level);   
                            Players.Add(player);
                            All.Add(player);
                        }
                        break;
                    case (int)Defines.ObjectType.GAMEOBJ:  //OBJECTS
                        {
                            if (!ObjectsEnabled) { goto Skip; }  //Если объекты выключены, пропускаем

                            //Чтение объекта (сам наварганил)
                            uint objectID = WowReader.ReadUInt(UnitFieldsAddress + Offsets.Object.ID);
                            float objectX = WowReader.ReadFloat(CurrObject_BaseAddress + Offsets.Object.Pos_X);
                            float objectY = WowReader.ReadFloat(CurrObject_BaseAddress + Offsets.Object.Pos_Y);
                            float objectZ = WowReader.ReadFloat(CurrObject_BaseAddress + Offsets.Object.Pos_Z);

                            //Добавляем объект в список
                            ReadedWowObjects++;
                            OtherObject obj = new OtherObject(objectID, objectX, objectY, objectZ, CurrObject_BaseAddress, UnitFieldsAddress);
                            Objects.Add(obj);
                            All.Add(obj);
                        }
                        break;
                }


            Skip:  //Для пропуска объекта

                //Устанавливаем текущий объект как следующий объект из менеджера объектов
                CurrObject_BaseAddress = NextObject(CurrObject_BaseAddress);
            }
           

            //Если нашего игрока нету в списке
            if (!MyPlayersFinded)
            {
                //Скорее всего что-то сбилось, реинициализируем адресса
                InitAddresses();
            }
        }


        private void ReadMyPlayer()
        {
            MyPlayer.BaseAddress = GetObjectBaseByGuid(MyPlayer.Guid);

            MyPlayer.XPos = WowReader.ReadFloat(MyPlayer.BaseAddress + Offsets.Unit.Pos_X);
            MyPlayer.YPos = WowReader.ReadFloat(MyPlayer.BaseAddress + Offsets.Unit.Pos_Y);
            MyPlayer.ZPos = WowReader.ReadFloat(MyPlayer.BaseAddress + Offsets.Unit.Pos_Z);
            MyPlayer.Rotation = WowReader.ReadFloat(MyPlayer.BaseAddress + Offsets.Object.Rot);
            MyPlayer.UnitFieldsAddress = WowReader.ReadPointer(MyPlayer.BaseAddress + Offsets.Object.UnitFields);
            MyPlayer.CurrentHealth = WowReader.ReadUInt32(MyPlayer.UnitFieldsAddress + Offsets.Unit.Health);
            MyPlayer.CurrentEnergy = WowReader.ReadUInt32(MyPlayer.UnitFieldsAddress + Offsets.Unit.Energy);
            MyPlayer.MaxHealth = WowReader.ReadUInt32(MyPlayer.UnitFieldsAddress + Offsets.Unit.MaxHealth);
            MyPlayer.Level = WowReader.ReadUInt32(MyPlayer.UnitFieldsAddress + Offsets.Unit.Level);
            MyPlayer.MaxEnergy = WowReader.ReadUInt32(MyPlayer.UnitFieldsAddress + Offsets.Unit.MaxEnergy);

            MyPlayer.Race = WowReader.ReadByte(MyPlayer.UnitFieldsAddress + Offsets.Unit.Race);
            MyPlayer.Class = WowReader.ReadByte(MyPlayer.UnitFieldsAddress + Offsets.Unit.Class);
            MyPlayer.Gender = WowReader.ReadByte(MyPlayer.UnitFieldsAddress + Offsets.Unit.Gender);
            MyPlayer.Name = PlayerNameFromGuid(MyPlayer.Guid);

            //Опыт персонажа
            {
                //uint maxXp = WowReader.ReadUInt32(MyPlayer.UnitFieldsAddress + Offsets.Unit.MaxXP);
                //uint currXp = WowReader.ReadUInt32(MyPlayer.UnitFieldsAddress + Offsets.Unit.CurrentXP);
            }
        }



        private string MobNameFromGuid(ulong Guid, uint NpcID)
        {
            //Эмиль пидоблас прикол
            //Не забудьте переключить режим в Memory.Mode.ALL
            //{
                //Иначе читаем и добавляем в кэш
                //UIntPtr ObjectBase = GetObjectBaseByGuid(Guid);
                //UIntPtr pointer = WowReader.ReadPointer(ObjectBase + Offsets.Name.mobName);
                //UIntPtr finalPointer = WowReader.ReadPointer(pointer + Offsets.Name.mobNameEx);

                //string ReadedMobName = WowReader.ReadUTF8String(finalPointer, 34);

                ////Замена имени на свое
                //string toReplace = "Эмиль Пидоблас";
                //if (ReadedMobName.Length >= toReplace.Length)
                //{
                //    WowReader.WriteUTF8String(finalPointer, toReplace);
                //}

                //return ReadedMobName;
            //}

            string MobName = "";
            if (Caching.GetMobName(NpcID, ref MobName))
            {
                return MobName;
            }
            else
            {
                //Иначе читаем и добавляем в кэш
                UIntPtr ObjectBase = GetObjectBaseByGuid(Guid);
                UIntPtr pointer = WowReader.ReadPointer(ObjectBase + Offsets.Name.mobName);
                UIntPtr finalPointer = WowReader.ReadPointer(pointer + Offsets.Name.mobNameEx);

                string ReadedMobName = WowReader.ReadUTF8String(finalPointer, 34);
                Caching.AddMobName(NpcID, ReadedMobName);

                return ReadedMobName;
            }
        }



        private string PlayerNameFromGuid(ulong guid)
        {
            string playerName = "";
            if (Caching.GetPlayerName(guid, ref playerName))
            {
                return playerName;
            }
            else
            {
                ulong mask, base_, offset, current, shortGUID, testGUID;

                mask = WowReader.ReadUInt32((UIntPtr)Offsets.Name.nameStore + Offsets.Name.nameMask);
                base_ = WowReader.ReadUInt32((UIntPtr)Offsets.Name.nameStore + Offsets.Name.nameBase);

                shortGUID = guid & 0xffffffff;
                offset = 12 * (mask & shortGUID);

                current = WowReader.ReadUInt32((UIntPtr)(base_ + offset + 8));
                offset = WowReader.ReadUInt32((UIntPtr)(base_ + offset));

                if ((current & 0x1) == 0x1) { return ""; }

                testGUID = WowReader.ReadUInt32((UIntPtr)(current));

                while (testGUID != shortGUID)
                {
                    current = WowReader.ReadUInt32((UIntPtr)(current + offset + 4));

                    if ((current & 0x1) == 0x1) { return ""; }
                    testGUID = WowReader.ReadUInt32((UIntPtr)(current));
                }


                //Читаем имя игрока
                string readedName = WowReader.ReadUTF8String((UIntPtr)(current + Offsets.Name.nameString), 26);

                //Добавляем в кэш прочитаннон имя
                Caching.AddPlayerName(guid, readedName);

                //Возвращяем прочитаный ник
                return readedName;
            }
        }


        private UIntPtr GetObjectBaseByGuid(ulong Guid)
        {
            TempObject.BaseAddress = FirstObject;

            try
            {
                while (TempObject.BaseAddress != UIntPtr.Zero)
                {
                    TempObject.Guid = WowReader.ReadUInt64(TempObject.BaseAddress + Offsets.Object.Guid);
                    if (TempObject.Guid == Guid)
                        return TempObject.BaseAddress;
                    TempObject.BaseAddress = WowReader.ReadPointer(TempObject.BaseAddress + Offsets.Client.NextObjectOffset);
                }
            }
            catch { return UIntPtr.Zero; }

            return UIntPtr.Zero;
        }


        private ulong GetObjectGuidByBase(UIntPtr Base)
        {
            return WowReader.ReadUInt64(Base + Offsets.Object.Guid);
        }


        public UIntPtr NextObject(UIntPtr unitAddress)
        {
            return WowReader.ReadPointer(unitAddress + Offsets.Client.NextObjectOffset);
        }
    }


    //Класс-событие для новой зоны
    public class ZoneChangedEventArgs : EventArgs
    {
        public uint NewZoneID { get; set; }
        public string NewZoneName { get; set; }

        public ZoneChangedEventArgs(uint NewZoneID, string NewZoneName)
        {
            this.NewZoneID = NewZoneID;
            this.NewZoneName = NewZoneName;
        }
    }
}
