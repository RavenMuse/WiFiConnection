using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace ShutDownTimer
{
    public class ShutdownTimer:IDisposable

    {

        public bool enable { get; set; }
        public TimeSpan shutdownTimeSpan { get; set; }
        public DateTime shutdownTime { get; set; }
        private Thread shutdownTimer;

        public ShutdownTimer()
        {
            Init();
            
        }

        private void Init()
        {
            enable = false;
            shutdownTime = DateTime.Now;
            shutdownTimeSpan = TimeSpan.Zero;
            shutdownTimer = null;
        }

        

        public bool Start(TimeSpan shutDownTimeSpan)
        {

            try
            {
                if (shutDownTimeSpan != TimeSpan.Zero)
                {
                    enable = true;
                    this.shutdownTimeSpan = shutDownTimeSpan;
                    shutdownTime = DateTime.Now.Add(this.shutdownTimeSpan);
                    shutdownTimer = new Thread(new ThreadStart(Shutdown));
                    shutdownTimer.IsBackground = true;
                    shutdownTimer.Start();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public bool Start(DateTime shutdownTime)
        {
            try
            {
                if (shutdownTime != DateTime.Now)
                {
                    enable = true;
                    this.shutdownTime = shutdownTime;
                    this.shutdownTimeSpan = shutdownTime - DateTime.Now;
                    shutdownTimer = new Thread(new ThreadStart(Shutdown));
                    shutdownTimer.Start();
                    return true;

                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        
        }
        public bool Stop()
        {
            try
            {
                
                shutdownTimer.Abort();                
                Init();
                enable = false;
                return true;

            }
            catch (Exception)
            {
                return false;
                
            }
        }
        public void Dispose()
        {
            enable = false;
            Stop();
        }
        /// <summary>
        /// Shutdown线程
        /// </summary>
        private void Shutdown()
        {
            while (true)
            {
                Thread.Sleep(1000);
                shutdownTimeSpan = shutdownTimeSpan.Add(new TimeSpan(0, 0, -1));
                if (shutdownTimeSpan == TimeSpan.Zero)
                {
                    break;
                }
            }
            DoExitWin(WinApi.EWX_FORCE | WinApi.EWX_SHUTDOWN);

        }

        public  bool DoExitWin(int flg)
        {
            bool flag;
            WinApi.TokPriv1Luid tp;
            IntPtr hproc = WinApi.GetCurrentProcess();
            IntPtr htok = IntPtr.Zero;
            flag = WinApi.OpenProcessToken(hproc, WinApi.TOKEN_ADJUST_PRIVILEGES | WinApi.TOKEN_QUERY, ref   htok);
            tp.Count = 1;
            tp.Luid = 0;
            tp.Attr = WinApi.SE_PRIVILEGE_ENABLED;
            flag = WinApi.LookupPrivilegeValue(null, WinApi.SE_SHUTDOWN_NAME, ref   tp.Luid);
            flag = WinApi.AdjustTokenPrivileges(htok, false, ref   tp, 0, IntPtr.Zero, IntPtr.Zero);
            flag = WinApi.ExitWindowsEx(flg, 0);
            return flag;
        }

        public static TimeSpan TimeParse(string hour, string minute)
        {
            int h, m;
            if (int.TryParse(hour, out h) && int.TryParse(minute, out m))
            {
                return new TimeSpan(h, m, 0);
            }
            else
            {
                return TimeSpan.Zero;
            }
        }

    }
}
