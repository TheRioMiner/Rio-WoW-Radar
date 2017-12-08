using System;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Rio_WoW_Radar
{
    public class Memory
    {
        private Process ProcessToRead;

        private IntPtr ProcessHwnd;


        public enum Mode : uint
        {
            READ = 0x10,
            WRITE = 0x20,
            BOTH = 0x30,
            ALL = 0x1f0fff,
        }


        public int WriteUInt32(UIntPtr MemoryAddress, uint Data)
        {
            return this.WriteProcessMemory(MemoryAddress, BitConverter.GetBytes(Data));
        }

        public int WriteUInt(UIntPtr MemoryAddress, uint Data)
        {
            return this.WriteProcessMemory(MemoryAddress, BitConverter.GetBytes(Data));
        }

        public int WriteInt32(UIntPtr MemoryAddress, int Data)
        {
            return this.WriteProcessMemory(MemoryAddress, BitConverter.GetBytes(Data));
        }

        public int WriteInt(UIntPtr MemoryAddress, int Data)
        {
            return this.WriteProcessMemory(MemoryAddress, BitConverter.GetBytes(Data));
        }

        public int WriteUInt64(UIntPtr MemoryAddress, UInt64 Data)
        {
            return this.WriteProcessMemory(MemoryAddress, BitConverter.GetBytes(Data));
        }

        public int WriteULong(UIntPtr MemoryAddress, ulong Data)
        {
            return this.WriteProcessMemory(MemoryAddress, BitConverter.GetBytes(Data));
        }

        public int WriteInt64(UIntPtr MemoryAddress, Int64 Data)
        {
            return this.WriteProcessMemory(MemoryAddress, BitConverter.GetBytes(Data));
        }

        public int WriteLong(UIntPtr MemoryAddress, long Data)
        {
            return this.WriteProcessMemory(MemoryAddress, BitConverter.GetBytes(Data));
        }

        public int WriteFloat(UIntPtr MemoryAddress, float Data)
        {
            return this.WriteProcessMemory(MemoryAddress, BitConverter.GetBytes(Data));
        }

        public int WriteDouble(UIntPtr MemoryAddress, double Data)
        {
            return this.WriteProcessMemory(MemoryAddress, BitConverter.GetBytes(Data));
        }

        public int WriteUTF8String(UIntPtr MemoryAddress, string Data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Data + "\0");

            return this.WriteProcessMemory(MemoryAddress, bytes);
        }

        private int WriteProcessMemory(UIntPtr MemoryAddress, byte[] buffer)
        {
            uint size = (uint)buffer.Length;
            if (this.ProcessToRead == null)
            {
                throw new ArgumentNullException("Process too writing too is null");
            }
            IntPtr intPtr;
            return MemoryApi.WriteProcessMemory(this.ProcessHwnd, MemoryAddress, buffer, size, out intPtr);
        }

        public bool SetProcess(object process, Mode Mode)
        {
            switch (process.GetType().ToString())
            {
                case "System.String":
                    {
                        Process[] processesByName = Process.GetProcessesByName(process.ToString());
                        if (processesByName.Length != 0)
                        {
                            this.ProcessToRead = processesByName[0];
                            goto SetPrivileges;
                        }
                        return false;
                    }
                case "System.Int32":
                    {
                        try
                        {
                            Process proc = Process.GetProcessById((int)process);
                            this.ProcessToRead = proc;

                            goto SetPrivileges;
                        }
                        catch
                        {
                            return false;
                        }
                    }
                case "System.UInt32":
                    {
                        try
                        {
                            Process proc = Process.GetProcessById((int)process);
                            this.ProcessToRead = proc;

                            goto SetPrivileges;
                        }
                        catch
                        {
                            return false;
                        }
                    }
                case "System.Single":
                    {
                        try
                        {
                            Process proc = Process.GetProcessById((int)process);
                            this.ProcessToRead = proc;

                            goto SetPrivileges;
                        }
                        catch
                        {
                            return false;
                        }
                    }

                case "System.Diagnostics.Process[]":
                    {
                        Process[] processArray = (Process[])process;
                        if (processArray.Length == 0)
                        {
                            return false;
                        }
                        this.ProcessToRead = processArray[0];

                        goto SetPrivileges;
                    }
                case "System.Diagnostics.Process":
                    {
                        Process proc = (Process)process;
                        this.ProcessToRead = proc;

                        goto SetPrivileges;
                    }
                default:
                    //Unknown process type
                    return false;
            }


        SetPrivileges:
            if (!this.SetDebugPrivilege())
            {
                return false;
            }


            //Finally open process
            this.OpenProcess((uint)Mode);
            return true;
        }



        private void OpenProcess(uint Mode)
        {
            if (this.ProcessToRead != null)
            {
                this.ProcessHwnd = MemoryApi.OpenProcess(Mode, 1, (uint)this.ProcessToRead.Id);
                return;
            }
            else
            {
                throw new ArgumentNullException("Process to open is null");
            }
        }


        public bool CloseProcess()
        {
            if (MemoryApi.CloseHandle(this.ProcessHwnd) == 0)
            {
                return false;
            }
            this.ProcessToRead = new Process();
            this.ProcessHwnd = IntPtr.Zero;
            return true;
        }


        public int IsReady()
        {
            if (this.ProcessToRead == null)
            {
                return 0;
            }
            if (this.ProcessToRead.HasExited)
            {
                return -1;
            }
            return 1;
        }


        public bool SetDebugPrivilege()
        {
            IntPtr zero = IntPtr.Zero;
            MemoryApi.LUID luid = default(MemoryApi.LUID);
            luid.HighPart = 0;
            luid.LowPart = 0u;
            MemoryApi.TOKEN_PRIVILEGES tOKEN_PRIVILEGES = default(MemoryApi.TOKEN_PRIVILEGES);

            IntPtr currentProcess = MemoryApi.GetCurrentProcess();

            if (!MemoryApi.OpenProcessToken(currentProcess, 40u, out zero))
            {
                return false;
            }

            if (!MemoryApi.LookupPrivilegeValue("", "SeDebugPrivilege", out luid))
            {
                return false;
            }

            tOKEN_PRIVILEGES.PrivilegeCount = 1u;
            tOKEN_PRIVILEGES.Luid = luid;
            tOKEN_PRIVILEGES.Attributes = 2u;

            return MemoryApi.AdjustTokenPrivileges(zero, false, ref tOKEN_PRIVILEGES, 0u, IntPtr.Zero, IntPtr.Zero) && MemoryApi.CloseHandle(zero) != 0;
        }


        public byte[] ReadRaw(UIntPtr MemoryAddress, uint Len)
        {
            return this.ReadProcessMemory(MemoryAddress, Len);
        }


        public string ReadString(UIntPtr MemoryAddress, uint Len)
        {
            string text = "";
            byte[] array = this.ReadProcessMemory(MemoryAddress, Len);
            int num = 0;
            while ((long)num > (long)((ulong)Len))
            {
                text += (char)array[num];
                num++;
            }
            return text;
        }


        public string ReadUTF8String(UIntPtr MemoryAddress)
        {
            const uint maxStrLenght = 200;

            List<byte> bytes = new List<byte>();
            byte b = this.ReadByte(MemoryAddress);
            uint readedBytes = 0;

            while ((b != 0x0) & readedBytes < maxStrLenght) //While not find end of string (or string limit)
            {
                bytes.Add(b);
                MemoryAddress = UIntPtr.Add(MemoryAddress, 1);
                b = this.ReadByte(MemoryAddress);

                readedBytes++;
            }

            return Encoding.UTF8.GetString(bytes.ToArray());
        }


        //Почанковое чтение текста
        public string ReadUTF8String(UIntPtr MemoryAddress, byte chunkLenght)
        {
            byte[] readedBytes = new byte[0];
            const byte maxChunks = 4;
            byte readedChunks = 0;

            while (readedChunks < maxChunks)
            {
                //Read chunk
                byte[] readed = this.ReadRaw(MemoryAddress + (chunkLenght * readedChunks), chunkLenght);

                //Finding ending in string
                for (int i = 0; i < chunkLenght; i++)
                {
                    if (readed[i] == 0)  //If find in chunk end of string
                    {
                        //Adds chunk to str and return
                        readedBytes = ConcatArrays(readedBytes, readed);

                        //And return string from readed bytes
                        return Encoding.UTF8.GetString(readedBytes, 0, (i + (chunkLenght * readedChunks))); 
                    }
                }

                //Adds readed string
                readedBytes = ConcatArrays(readedBytes, readed);
                readedChunks++;  //And increment readed chunks
            }

            return Encoding.UTF8.GetString(readedBytes);
        }


        public float ReadFloat(UIntPtr Address)
        {
            byte[] value = this.ReadProcessMemory(Address, 4u);
            return BitConverter.ToSingle(value, 0);
        }

        public short ReadShort(UIntPtr Address)
        {
            byte[] value = this.ReadProcessMemory(Address, 2u);
            return BitConverter.ToInt16(value, 0);
        }

        public ushort ReadUShort(UIntPtr Address)
        {
            byte[] value = this.ReadProcessMemory(Address, 2u);
            return BitConverter.ToUInt16(value, 0);
        }

        public uint ReadUInt32(UIntPtr MemoryAddress)
        {
            byte[] value = this.ReadProcessMemory(MemoryAddress, 4u);
            return BitConverter.ToUInt32(value, 0);
        }

        public uint ReadUInt(UIntPtr MemoryAddress)
        {
            byte[] value = this.ReadProcessMemory(MemoryAddress, 4u);
            return BitConverter.ToUInt32(value, 0);
        }

        public int ReadInt32(UIntPtr MemoryAddress)
        {
            byte[] value = this.ReadProcessMemory(MemoryAddress, 4u);
            return BitConverter.ToInt32(value, 0);
        }

        public int ReadInt(UIntPtr MemoryAddress)
        {
            byte[] value = this.ReadProcessMemory(MemoryAddress, 4u);
            return BitConverter.ToInt32(value, 0);
        }

        public UInt64 ReadUInt64(UIntPtr MemoryAddress)
        {
            byte[] value = this.ReadProcessMemory(MemoryAddress, 8u);
            return BitConverter.ToUInt64(value, 0);
        }

        public ulong ReadULong(UIntPtr MemoryAddress)
        {
            byte[] value = this.ReadProcessMemory(MemoryAddress, 8u);
            return BitConverter.ToUInt64(value, 0);
        }

        public Int64 ReadInt64(UIntPtr MemoryAddress)
        {
            byte[] value = this.ReadProcessMemory(MemoryAddress, 8u);
            return BitConverter.ToInt64(value, 0);
        }

        public UIntPtr ReadPointer(UIntPtr MemoryAddress)
        {
            byte[] value = this.ReadProcessMemory(MemoryAddress, 4u);
            return (UIntPtr)BitConverter.ToUInt32(value, 0);
        }

        public long ReadLong(UIntPtr MemoryAddress)
        {
            byte[] value = this.ReadProcessMemory(MemoryAddress, 8u);
            return BitConverter.ToInt64(value, 0);
        }

        public byte ReadByte(UIntPtr MemoryAddress)
        {
            byte[] value = this.ReadProcessMemory(MemoryAddress, 1u);
            return value[0];
        }



        private byte[] ReadProcessMemory(UIntPtr MemoryAddress, uint bytesToRead)
        {
            byte[] array = new byte[bytesToRead];
            if (this.ProcessToRead == null)
            {
                return array;
            }
            IntPtr intPtr;
            MemoryApi.ReadProcessMemory(this.ProcessHwnd, MemoryAddress, array, bytesToRead, out intPtr);
            return array;
        }

        public static byte[] ConcatArrays(byte[] a, byte[] b)
        {
            try
            {
                byte[] output = new byte[a.Length + b.Length];
                Array.Copy(a, output, a.Length);
                Array.Copy(b, 0, output, a.Length, b.Length);
                return output;
            }
            catch
            { return a; }
        }

        public static bool SetDebugPrivileges()
        {
            IntPtr zero = IntPtr.Zero;
            MemoryApi.LUID luid = default(MemoryApi.LUID);
            luid.HighPart = 0;
            luid.LowPart = 0u;
            MemoryApi.TOKEN_PRIVILEGES tOKEN_PRIVILEGES = default(MemoryApi.TOKEN_PRIVILEGES);

            IntPtr currentProcess = MemoryApi.GetCurrentProcess();

            if (!MemoryApi.OpenProcessToken(currentProcess, 40u, out zero))
            {
                return false;
            }

            if (!MemoryApi.LookupPrivilegeValue("", "SeDebugPrivilege", out luid))
            {
                return false;
            }

            tOKEN_PRIVILEGES.PrivilegeCount = 1u;
            tOKEN_PRIVILEGES.Luid = luid;
            tOKEN_PRIVILEGES.Attributes = 2u;

            return MemoryApi.AdjustTokenPrivileges(zero, false, ref tOKEN_PRIVILEGES, 0u, IntPtr.Zero, IntPtr.Zero) && MemoryApi.CloseHandle(zero) != 0;
        }
    }

    internal class MemoryApi
    {
        public struct LUID
        {
            public uint LowPart;

            public int HighPart;
        }

        public struct TOKEN_PRIVILEGES
        {
            public uint PrivilegeCount;

            public MemoryApi.LUID Luid;

            public uint Attributes;
        }

        public const uint PROCESS_ALL = 2035711u;

        public const uint PROCESS_TERMINATE = 1u;

        public const uint PROCESS_CREATE_THREAD = 2u;

        public const uint PROCESS_VM_OPERATION = 8u;

        public const uint PROCESS_VM_READ = 16u;

        public const uint PROCESS_VM_WRITE = 32u;

        public const uint PROCESS_DUPHANDLE = 64u;

        public const uint PROCESS_SET_INFORMATION = 512u;

        public const uint PROCESS_QUERY_INOFRMATION = 1024u;

        public const uint PROCESS_SYNCHRONIZE = 1048576u;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern int CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll")]
        public static extern int ReadProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress, [In] [Out] byte[] buffer, uint size, out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern int WriteProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress, [In] [Out] byte[] buffer, uint size, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, out MemoryApi.LUID lpLuid);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, [MarshalAs(UnmanagedType.Bool)] bool DisableAllPrivileges, ref MemoryApi.TOKEN_PRIVILEGES NewState, uint BufferLength, IntPtr previous, IntPtr ReturnLength);
    }
}
