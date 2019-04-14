using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace ConfigurableIrcBotApp.Bots
{
    public class AutoSave
    {
        private MainWindow main;
        public Thread autoSaveThread { get; set; }

        readonly InputSimulator sim;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern IntPtr SetForegroundWindow(IntPtr hWnd);
        public AutoSave(MainWindow main)
        {
            this.main = main;
            sim = new InputSimulator();
            autoSaveThread = new Thread(new ThreadStart(this.Run));
        }
        public void Start()
        {
            autoSaveThread.Start();
        }

        public void Stop()
        {
            autoSaveThread.Abort();
        }
        public void Run()
        {
            
                for (int i = 1; i < 11; i++)
                {
                    Process targetEmulator = Process.GetProcessesByName(main.emulationProcessName).FirstOrDefault();
                    IntPtr hWnd = targetEmulator.MainWindowHandle;
                    if (hWnd != IntPtr.Zero)
                    {
                        SetForegroundWindow(hWnd);
                    }

                    VirtualKeyCode code;
                    Enum.TryParse<VirtualKeyCode>("F" + i, out code);

                    sim.Keyboard.KeyDown(VirtualKeyCode.SHIFT);
                    sim.Keyboard.KeyPress(code);
                    sim.Keyboard.KeyUp(VirtualKeyCode.SHIFT);
                    Thread.Sleep(3600000);
                    if(i >= 10)
                    {
                    i = 1;
                    }
                }
        }
    }
}
