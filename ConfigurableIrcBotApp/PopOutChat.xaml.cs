using System;
using System.Windows;

using System.ComponentModel;
using System.Windows.Forms;

namespace ConfigurableIrcBotApp
{
    /// <summary>
    /// Interaction logic for PopOutChat.xaml
    /// </summary>
    public partial class PopOutChat : Window
    {
        private MainWindow main;
        string currentTime = string.Empty;
        Timer stopWatch;

        DateTime startTime;
        DateTime pauseTime;
        TimeSpan elapsedTimeSpan;

        bool _running;
        bool _wasPaused;

        public string titleType { get; set; }
        public PopOutChat(MainWindow main)
        {
            InitializeComponent();
            this.main = main;

            titleType = "Text";
        }

        private void popoutChat_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        void timer_Tick(object sender, EventArgs e)
        {

            TimeSpan ts = DateTime.Now - startTime;
            if (_wasPaused)
            {
                ts.Add(elapsedTimeSpan);
            }
            
            currentTime = String.Format("{0:00}d {1:00}h {2:00}m {3:00}s",
                     ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
            clockTxt.Text = currentTime; 
        }

        public void startTimer()
        {
            stopWatch = new Timer();
            stopWatch.Tick += new EventHandler(timer_Tick);
            stopWatch.Interval = 1000;
            stopWatch.Enabled = true;
            stopWatch.Start();

            startTime = DateTime.Now;
            if (_wasPaused)
            {
                startTime = pauseTime;
            }
            _running = true;
        }

        public void pauseTimer()
        {
            stopWatch.Stop();
            _running = false;
            _wasPaused = true;
            pauseTime = DateTime.Now;
            elapsedTimeSpan = DateTime.Now - startTime;
        }

        public void stopTimer()
        {
            if (_running)
            {
                stopWatch.Stop();
                _wasPaused = false;
                _running = false;
            }
        }
        
        public void writeToChat(Message message)
        {
            chatBlock.AppendText(message.userName + ": " + message.message + "\r");
            chatBlock.ScrollToEnd();
        }
        
    }
}
