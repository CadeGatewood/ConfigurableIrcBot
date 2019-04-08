
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
using System.Collections.ObjectModel;

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

        ObservableCollection<processDisplay> processDisplays = new ObservableCollection<processDisplay>();

        public SelectProcess(MainWindow main)
        {
            this.main = main;
            InitializeComponent();

            loadProcesses();

            processListBox.ItemsSource = this.processDisplays;

        }


        public void loadProcesses()
        {
            try { 
                var newProcessList = new ObservableCollection<processDisplay>();

                var processes = Process.GetProcesses();

                removeProcesses(processes);

                foreach (Process process in processes)
                {
                    if (!String.IsNullOrEmpty(process.MainWindowTitle))
                    {
                        System.Drawing.Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(process.MainModule.FileName);
                        addProcess(new processDisplay(IconUtilities.ToImageSource(icon), process.ProcessName));
                    }
                }
            }
            catch(Exception processException)
            {
                //problem accessing a process, some processes will through accessdenied exceptions, will deal with this later
                //todo idk something
            }

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
            main.emulationProcessName = selection.processName;
            System.Threading.Thread.Sleep(1000);
            Hide();
        }

        private void addProcess(processDisplay process)
        {
            if (!processDisplays.Select(proc => proc.processName)
                                .Contains(process.processName))
                processDisplays.Add(process);
        }

        private void removeProcesses(Process[] currentProcesses)
        {
            var oldProcessDisplays = processDisplays
                .Where(proc => processDisplays
                .Select(process => process.processName)
                .Except(currentProcesses
                    .Select(process => process.ProcessName))
                    .Contains(proc.processName));
            foreach(processDisplay process in oldProcessDisplays){
                processDisplays.Remove(process);
            }
        }
    }
}
