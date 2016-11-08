using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Jet.Framework.Utility
{
    public static class SingleAppMutex
    {
        public const int WS_SHOWNORMAL = 1;

        public const int SW_SHOW = 5;
        public const int SW_SHOWNA = 8;
        public const int SW_RESTORE = 9;
        public const int SW_SHOWDEFAULT = 10;

        [DllImport("User32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        [DllImport("User32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int cmdShow);

        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static bool Monitor(Form mainForm, out Mutex appMutex)
        {
            string mutexName = mainForm.GetType().FullName;
            string semaphoreName = mainForm.GetType().FullName + ".Notify.Active";
            bool createdNew;
            appMutex = new Mutex(true, mutexName, out createdNew);
            if (createdNew == false)
            {
                try
                {
                    Semaphore semaphore = Semaphore.OpenExisting(semaphoreName);
                    semaphore.Release();
                }
                catch { }
                return false;
            }
            else
            {
                mainForm.Load += (s, e) =>
                {
                    Thread monitorSecondApp = new Thread(() =>
                    {
                        Semaphore semaphore = new Semaphore(0, 1, semaphoreName);
                        while (true)
                        {
                            semaphore.WaitOne();
                            mainForm.Invoke(new Action(() =>
                            {
                                if (mainForm.WindowState == FormWindowState.Minimized)
                                    ShowWindow(mainForm.Handle, SW_RESTORE);

                                SetForegroundWindow(mainForm.Handle);
                            }));
                        }
                    });
                    monitorSecondApp.IsBackground = true;
                    monitorSecondApp.Start();
                };
                return true;
            }
        }
    }
}
