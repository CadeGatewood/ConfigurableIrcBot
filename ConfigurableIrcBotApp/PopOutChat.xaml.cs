using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;


using System.Windows.Media;
using System.ComponentModel;
using System.Diagnostics;

namespace ConfigurableIrcBotApp
{
    /// <summary>
    /// Interaction logic for PopOutChat.xaml
    /// </summary>
    public partial class PopOutChat : Window
    {
        private MainWindow main;

        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        Stopwatch stopWatch = new Stopwatch();
        string currentTime = string.Empty;

        public PopOutChat(MainWindow main)
        {
            InitializeComponent();
            this.main = main;
        }

        private void popoutChat_Closing(object sender, CancelEventArgs e)
        {
                main.setChatPoppedOut(false);
                e.Cancel = true;
                Hide();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            lblTime.Foreground = Brushes.White;
            lblTime.Content = DateTime.Now.ToLongTimeString();
        }

        private void chatTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
