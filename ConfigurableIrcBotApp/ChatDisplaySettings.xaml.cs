using System.Windows;
using System.Windows.Input;

using System.Text.RegularExpressions;
using System;
using System.Windows.Media;
using System.ComponentModel;
using System.IO;

using System.Drawing.Text;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Forms;

namespace ConfigurableIrcBotApp
{
    /// <summary>
    /// Interaction logic for ChatDisplaySettings.xaml
    /// </summary>
    public partial class ChatDisplaySettings : Window
    {
        private MainWindow main;
        private PopOutChat popOutChat;


        string fontsfolderLocation = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
        string localFonts = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\Fonts";

        List<string> loadedFonts;

        public string PlaceholderText { get; set; }

        public ChatDisplaySettings(MainWindow main)
        {
            InitializeComponent();


            this.main = main;
            this.popOutChat = main.popOutChat;

            loadedFonts = new List<string>();
            loadFontOptions();

            this.DragEnter += new System.Windows.DragEventHandler(fontFileDrop_DragEnter);
            this.Drop += new System.Windows.DragEventHandler(fontFileDrop_DragDrop);

        }

        private void chatDisplaySettings_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void numberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public void refreshAvailableFonts()
        {
            loadedFonts = new List<string>();
            loadedFonts.AddRange(Directory.GetFiles(fontsfolderLocation, "*.ttf"));
            loadedFonts.AddRange(Directory.GetFiles(localFonts, "*.ttf"));
        }

        public void loadFontOptions()
        {
            
        }

        void fontFileDrop_DragEnter(object sender, System.Windows.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop)) e.Effects = System.Windows.DragDropEffects.Copy;
        }

        void fontFileDrop_DragDrop(object sender, System.Windows.DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop);
            
            foreach (string file in files)
            {
                string fileName = System.IO.Path.GetFileName(file);
                string destFile = System.IO.Path.Combine(localFonts, fileName);
                File.Copy(file, destFile, true);
            }

            loadFontOptions();

        }

        private void changeFont_Click(object sender, RoutedEventArgs e)
        {
            FontDialog fd = new FontDialog();
            System.Windows.Forms.DialogResult dr = fd.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.Cancel)
            {
                switch (sectionSelection.Text)
                {
                    case "Title":
                        popOutChat.titleBlock.FontFamily = new System.Windows.Media.FontFamily(fd.Font.Name);
                        popOutChat.titleBlock.FontSize = fd.Font.Size * 96.0 / 72.0;
                        popOutChat.titleBlock.FontWeight = fd.Font.Bold ? FontWeights.Bold : FontWeights.Regular;
                        popOutChat.titleBlock.FontStyle = fd.Font.Italic ? FontStyles.Italic : FontStyles.Normal;
                        break;
                    case "Chat":
                        popOutChat.chatBlock.FontFamily = new System.Windows.Media.FontFamily(fd.Font.Name);
                        popOutChat.chatBlock.FontSize = fd.Font.Size * 96.0 / 72.0;
                        popOutChat.chatBlock.FontWeight = fd.Font.Bold ? FontWeights.Bold : FontWeights.Regular;
                        popOutChat.chatBlock.FontStyle = fd.Font.Italic ? FontStyles.Italic : FontStyles.Normal;
                        break;
                    case "Timer":
                        popOutChat.clockTxt.FontFamily = new System.Windows.Media.FontFamily(fd.Font.Name);
                        popOutChat.clockTxt.FontSize = fd.Font.Size * 96.0 / 72.0;
                        popOutChat.clockTxt.FontWeight = fd.Font.Bold ? FontWeights.Bold : FontWeights.Regular;
                        popOutChat.clockTxt.FontStyle = fd.Font.Italic ? FontStyles.Italic : FontStyles.Normal;
                        break;
                    default:
                        System.Windows.MessageBox.Show("Please select a valid section");
                        break;
                }
            }
        }

        private void changeFontColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            var result = cd.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                switch (sectionSelection.Text)
                {
                    case "Title":
                        popOutChat.titleBlock.Foreground = new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));
                        break;
                    case "Chat":
                        popOutChat.chatBlock.Foreground = new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));
                        break;
                    case "Timer":
                        popOutChat.clockTxt.Foreground = new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));
                        break;
                    default:
                        System.Windows.MessageBox.Show("Please select a valid section");
                        break;
                }
            }
        }

        private void titleChangeButton_Click(object sender, RoutedEventArgs e)
        {
            popOutChat.titleBlock.Content = titleChangeEntry.Text;
        }

        private void sectionSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        
    }
}
