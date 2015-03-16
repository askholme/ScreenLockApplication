using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ScreenLockApplication
{
    
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        static uint GetLastInputTime()
        {
            uint idleTime = 0;
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
            lastInputInfo.dwTime = 0;

            uint envTicks = (uint)Environment.TickCount;

            if (GetLastInputInfo(ref lastInputInfo))
            {
                uint lastInputTick = lastInputInfo.dwTime;

                idleTime = envTicks - lastInputTick;
            }

            return ((idleTime > 0) ? (idleTime / 1000) : 0);
        }
        [StructLayout( LayoutKind.Sequential )]
        public struct LASTINPUTINFO
        {
            public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 cbSize;    
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dwTime;
        }
        [DllImport("user32.dll")]
        public static extern Boolean GetLastInputInfo(ref LASTINPUTINFO plii);
        [DllImport("user32.dll")]
        public static extern Boolean LockWorkStation();

        private int getLimit()
        {
            try
            {
                return Convert.ToInt32(this.limit.Text);
            }
            catch
            {
                return 9999;
            }
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            Int32 time = Convert.ToInt32(GetLastInputTime());
            label1.Text =  time.ToString() + "s";
            if (this.getLimit()<=time) {
                LockWorkStation();
            }
        }

        private void Settings_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                this.Hide();
            }
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }


    }
}
