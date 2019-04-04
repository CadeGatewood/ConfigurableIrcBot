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
        readonly InputSimulator sim;
        MainWindow main;


        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        public PlayBot(MainWindow main)
        {
            this.main = main;
            sim = new InputSimulator();
        }

        public void controlEmulator(PlayBotAction action, string emulationProcessName)
        {
            try
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
            catch(Exception e)
            {
                main.writeError("\n\n There was an error sending events from chat. \n Make sure to select a destination process.", e);
            }
        }
    }
}
