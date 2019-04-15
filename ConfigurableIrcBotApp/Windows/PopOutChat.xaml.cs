using System;
using System.Windows;

using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

namespace ConfigurableIrcBotApp
{

    public partial class PopOutChat : Window
    {
        private MainWindow main;
        string currentTime = string.Empty;
        Timer tickTimer;

        public DateTime startTime { get; set; }
        DateTime pausedTime;

        bool _running;
        bool _paused;

        public string titleType { get; set; }
        public PopOutChat(MainWindow main)
        {
            InitializeComponent();
            this.main = main;

            titleType = "Text";

            _running = false;
            _paused = false;

        }

        

        private void popoutChat_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            TimeSpan currentSpan = DateTime.Now - startTime;
            var watchTime = String.Format("{0:00}d {1:00}h {2:00}m {3:00}s",
                     currentSpan.Days, currentSpan.Hours, currentSpan.Minutes, currentSpan.Seconds);

            clockTxt.Text = watchTime; 
        }

        public void startTimer()
        {
            if(startTime == DateTime.MinValue)
            {
                startTime = DateTime.Now;
            }

            if (_paused)
            {
                this.startTime = startTime.Add(DateTime.Now - pausedTime);
                _paused = false;
            }

            tickTimer = new Timer();
            tickTimer.Tick += new EventHandler(timer_Tick);
            tickTimer.Interval = 1000;
            tickTimer.Enabled = true;

            tickTimer.Start();

            _running = true;
        }

        public void pauseTimer()
        {
            tickTimer.Stop();
            pausedTime = DateTime.Now;
            _paused = true;
            _running = false;
        }

        public void stopTimer()
        {
            if (_running)
            {
                tickTimer.Stop();
                this.startTime = DateTime.MinValue;
                _paused = false;
                _running = false;
            }
        }
        
        public void writeToChat(Message message)
        {
            chatBlock.AppendText(message.userName + ": " + message.message + "\r");
            chatBlock.ScrollToEnd();
        }

        public void updateVote()
        {
            this.Dispatcher.Invoke(() => //Use Dispather to Update UI Immediately  
            {
                voteProgressBar.Value = main.voteResults;
            });
        }
    }
}
