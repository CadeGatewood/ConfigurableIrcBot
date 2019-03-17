using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace ConfigurableIrcBotApp.tabManagers
{
    
    class PopoutChatSettingsManager
    {
        private MainWindow main;

        string fontsfolderLocation = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
        string localFonts = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\Fonts";

        public PopoutChatSettingsManager(MainWindow main)
        {
            this.main = main;
        }

        public void changeFont()
        {
            FontDialog fd = new FontDialog();
            System.Windows.Forms.DialogResult dr = fd.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.Cancel)
            {
                switch (main.sectionSelection.Text)
                {
                    case "Title":
                        main.popOutChat.titleBlock.FontFamily = new System.Windows.Media.FontFamily(fd.Font.Name);
                        main.popOutChat.titleBlock.FontSize = fd.Font.Size * 96.0 / 72.0;
                        main.popOutChat.titleBlock.FontWeight = fd.Font.Bold ? FontWeights.Bold : FontWeights.Regular;
                        main.popOutChat.titleBlock.FontStyle = fd.Font.Italic ? FontStyles.Italic : FontStyles.Normal;
                        break;
                    case "Chat":
                        main.popOutChat.chatBlock.FontFamily = new System.Windows.Media.FontFamily(fd.Font.Name);
                        main.popOutChat.chatBlock.FontSize = fd.Font.Size * 96.0 / 72.0;
                        main.popOutChat.chatBlock.FontWeight = fd.Font.Bold ? FontWeights.Bold : FontWeights.Regular;
                        main.popOutChat.chatBlock.FontStyle = fd.Font.Italic ? FontStyles.Italic : FontStyles.Normal;
                        break;
                    case "Timer":
                        main.popOutChat.clockTxt.FontFamily = new System.Windows.Media.FontFamily(fd.Font.Name);
                        main.popOutChat.clockTxt.FontSize = fd.Font.Size * 96.0 / 72.0;
                        main.popOutChat.clockTxt.FontWeight = fd.Font.Bold ? FontWeights.Bold : FontWeights.Regular;
                        main.popOutChat.clockTxt.FontStyle = fd.Font.Italic ? FontStyles.Italic : FontStyles.Normal;
                        break;
                    default:
                        System.Windows.MessageBox.Show("Please select a valid section");
                        break;
                }
            }
        }

        public void changeFontColor()
        {
            ColorDialog cd = new ColorDialog();
            var result = cd.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                switch (main.sectionSelection.Text)
                {
                    case "Title":
                        main.popOutChat.titleBlock.Foreground = new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));
                        break;
                    case "Chat":
                        main.popOutChat.chatBlock.Foreground = new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));
                        break;
                    case "Timer":
                        main.popOutChat.clockTxt.Foreground = new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));
                        break;
                    default:
                        System.Windows.MessageBox.Show("Please select a valid section");
                        break;
                }
            }
        }

    }
}
