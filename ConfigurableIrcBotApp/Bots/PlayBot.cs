using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace ConfigurableIrcBotApp
{
    public class PlayBot
    {
        readonly InputSimulator sim;
        MainWindow main;

        private Semaphore _controlPool;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        public PlayBot(MainWindow main)
        {
            this.main = main;
            sim = new InputSimulator();

            _controlPool = new Semaphore(1, 1);
        }

        public void comboControlEmulator(List<PlayBotAction> actions, string emulationProcessName, int repeat)
        {
            try
            {
                if (main.voteResults > 50)
                    _controlPool.WaitOne();

                Process targetEmulator = Process.GetProcessesByName(emulationProcessName).FirstOrDefault();
                IntPtr hWnd = targetEmulator.MainWindowHandle;
                if (hWnd != IntPtr.Zero)
                {
                    SetForegroundWindow(hWnd);
                }

                for (int i = 0; i < repeat; i++)
                {
                    foreach (PlayBotAction action in actions)
                    {
                        sim.Keyboard.KeyDown(action.keyPress);
                    }

                    Thread.Sleep(actions.Max(action => action.duration));

                    foreach (PlayBotAction action in actions)
                    {
                        sim.Keyboard.KeyUp(action.keyPress);
                    }

                    if (repeat > 1)
                    {
                        Thread.Sleep(250);
                    }
                }
                

                if (main.voteResults > 50)
                    _controlPool.Release();

            }
            catch (Exception e)
            {
                main.writeError("\n\n There was an error sending events from chat.", e);
            }
        }

        public void resetSemaphore()
        {
            _controlPool = new Semaphore(1, 1);
        }

    }
}
