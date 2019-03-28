using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;

namespace ConfigurableIrcBotApp
{
    public class PlayBot
    {
        InputSimulator sim;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        public PlayBot()
        {
            sim = new InputSimulator();
        }

        public void controlEmulator(PlayBotAction action, string emulationProcessName)
        {
            Process targetEmulator = Process.GetProcessesByName(emulationProcessName).FirstOrDefault();
            IntPtr hWnd = targetEmulator.MainWindowHandle;
            if (hWnd != IntPtr.Zero)
            {
                SetForegroundWindow(hWnd);
            }
            sim.Keyboard.KeyDown(action.keyPress);
            Thread.Sleep(action.duration);
            sim.Keyboard.KeyUp(action.keyPress);
        }
    }
}
