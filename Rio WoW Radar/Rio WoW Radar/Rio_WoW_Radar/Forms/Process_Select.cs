using System;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Rio_WoW_Radar.Forms
{
    public partial class Process_Select : Form
    {
        public int pid = -1;

        public Process_Select()
        {
            InitializeComponent();
            RefreshList();
        }


        private void RefreshList()
        {
            try
            {
                Process[] wowProcesses = Process.GetProcessesByName("Wow");

                dataGridView1.Rows.Clear();
                Memory.SetDebugPrivileges();
                foreach (Process proc in wowProcesses)
                {
                    if ((proc != null) & (!proc.HasExited))
                    {
                        IntPtr procHwnd = MemoryApi.OpenProcess((int)Memory.Mode.READ, 1, (uint)proc.Id);

                        int pid = proc.Id;
                        string hexPid = proc.Id.ToString("X2");
                        string login = ReadUTF8String(procHwnd, (UIntPtr)Offsets.Client.Other.StaticLoginString);

                        //В мире
                        string inWorld = ReadProcessMemory(procHwnd, (UIntPtr)Offsets.Client.StaticInWorld, 1u)[0] == 1 ? "Да" : "Нет";

                        //Присоединен?
                        UIntPtr connectionPointer = ReadPointer(procHwnd, (UIntPtr)Offsets.Client.StaticClientConnection);
                        string connected = ReadProcessMemory(procHwnd, connectionPointer + Offsets.Client.HasConnectedOffset, 1u)[0] == 5 ? "Да" : "Нет";

                        MemoryApi.CloseHandle(procHwnd); //Обязательно закрываем

                        //Добавляем
                        dataGridView1.Rows.Add(pid, hexPid, login, inWorld, connected);
                    }
                }
            }
            catch (Exception ex) { Tools.MsgBox.Exception(ex, "Ошибка обновления списка с процессами"); }
        }



        private void button_ok_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                pid = int.Parse(dataGridView1.SelectedRows[0].Cells["PPid"].Value.ToString());
                this.Close();
            }
        }


        private void button_refresh_Click(object sender, EventArgs e)
        {
            RefreshList();
        }



        public string ReadUTF8String(IntPtr Hwnd, UIntPtr MemoryAddress)
        {
            List<byte> bytes = new List<byte>();
            byte b = ReadProcessMemory(Hwnd, MemoryAddress, 1u)[0];

            while ((b != 0x0)) //Пока не кончится строка
            {
                bytes.Add(b);
                MemoryAddress = UIntPtr.Add(MemoryAddress, 1);
                b = ReadProcessMemory(Hwnd, MemoryAddress, 1u)[0];  //Читаем байт
            }

            return Encoding.UTF8.GetString(bytes.ToArray());
        }

        public UIntPtr ReadPointer(IntPtr Hwnd, UIntPtr MemoryAddress)
        {
            byte[] value = this.ReadProcessMemory(Hwnd, MemoryAddress, 4u);
            return (UIntPtr)BitConverter.ToUInt32(value, 0);
        }

        private byte[] ReadProcessMemory(IntPtr Hwnd, UIntPtr MemoryAddress, uint bytesToRead)
        {
            byte[] array = new byte[bytesToRead];
            IntPtr intPtr;
            MemoryApi.ReadProcessMemory(Hwnd, MemoryAddress, array, bytesToRead, out intPtr);
            return array;
        }
    }
}
