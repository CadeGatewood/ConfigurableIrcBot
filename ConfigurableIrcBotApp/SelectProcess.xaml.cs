
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Reflection;

using System;
using ConfigurableIrcBotApp.DataObjects;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Interop;
using System.ComponentModel;

namespace ConfigurableIrcBotApp
{   internal static class IconUtilities
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        public static ImageSource ToImageSource(this Icon icon)
        {
            Bitmap bitmap = icon.ToBitmap();
            IntPtr hBitmap = bitmap.GetHbitmap();

            ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            if (!DeleteObject(hBitmap))
            {
                throw new Win32Exception();
            }

            return wpfBitmap;
        }
    }
    /// <summary>
    /// Interaction logic for SelectProcess.xaml
    /// </summary>
    public partial class SelectProcess : Window
    {
        MainWindow main;

        List<processDisplay> processDisplays = new List<processDisplay>();

        public SelectProcess(MainWindow main)
        {
            this.main = main;
            InitializeComponent();

            loadProcesses();
            processListBox.ItemsSource = this.processDisplays;

        }


        public void loadProcesses()
        {
            processDisplays = new List<processDisplay>();

            var processes = Process.GetProcesses();
            
            foreach (Process process in processes)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    System.Drawing.Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(process.MainModule.FileName);
                    this.processDisplays.Add(new processDisplay(IconUtilities.ToImageSource(icon), process.ProcessName));
                }
            }

            processListBox.Items.Refresh();
        }

        private void RefreshProcess_Click(object sender, RoutedEventArgs e)
        {
            loadProcesses();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void ProcessListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            processDisplay selection = ((sender as ListBox).SelectedItem as processDisplay);
            main.playBot.emulationProcess = selection.processName;
            System.Threading.Thread.Sleep(1000);
            Hide();
        }
    }
}
