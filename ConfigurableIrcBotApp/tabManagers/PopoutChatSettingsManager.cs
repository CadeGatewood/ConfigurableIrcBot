using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ConfigurableIrcBotApp.tabManagers
{
    
    public class PopoutChatSettingsManager
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
                    case "Play Bot Controls":
                        main.popOutChat.playBotControlDisplayGrid.FontFamily = new System.Windows.Media.FontFamily(fd.Font.Name);
                        main.popOutChat.playBotControlDisplayGrid.FontSize = fd.Font.Size * 96.0 / 72.0;
                        main.popOutChat.playBotControlDisplayGrid.FontWeight = fd.Font.Bold ? FontWeights.Bold : FontWeights.Regular;
                        main.popOutChat.playBotControlDisplayGrid.FontStyle = fd.Font.Italic ? FontStyles.Italic : FontStyles.Normal;
                        break;
                    case "Command Mode":
                        main.popOutChat.rightVote.FontFamily = new System.Windows.Media.FontFamily(fd.Font.Name);
                        main.popOutChat.rightVote.FontSize = fd.Font.Size * 96.0 / 72.0;
                        main.popOutChat.rightVote.FontWeight = fd.Font.Bold ? FontWeights.Bold : FontWeights.Regular;
                        main.popOutChat.rightVote.FontStyle = fd.Font.Italic ? FontStyles.Italic : FontStyles.Normal;

                        main.popOutChat.leftVote.FontFamily = new System.Windows.Media.FontFamily(fd.Font.Name);
                        main.popOutChat.leftVote.FontSize = fd.Font.Size * 96.0 / 72.0;
                        main.popOutChat.leftVote.FontWeight = fd.Font.Bold ? FontWeights.Bold : FontWeights.Regular;
                        main.popOutChat.leftVote.FontStyle = fd.Font.Italic ? FontStyles.Italic : FontStyles.Normal;
                        break;
                    default:
                        System.Windows.MessageBox.Show("Please select a valid font section");
                        break;
                }
            }
        }

        public void changeColor()
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
                    case "Play Bot Controls":
                        main.popOutChat.playBotControlDisplayGrid.Foreground = new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));
                        break;
                    case "Command Mode":
                        main.popOutChat.rightVote.Foreground = new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));
                        main.popOutChat.leftVote.Foreground = new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));
                        break;
                    case "Vote Bar":
                        main.popOutChat.voteProgressBar.Foreground = new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));
                        break;
                    default:
                        System.Windows.MessageBox.Show("Please select a valid section");
                        break;
                }
            }
        }

        public void changeBackGroundColors()
        {
            ColorDialog cd = new ColorDialog();
            var result = cd.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                var color = new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));
                main.popOutChat.titleBlock.Background = color;
                main.popOutChat.controlsDock.Background = color;
                main.popOutChat.clockTxt.Background = color;
                main.popOutChat.chatBlock.Background = color;
                main.popOutChat.commandModeDock.Background = color;
                main.popOutChat.voteProgressBar.Background = color;
                main.popOutChat.Background = color;
            }
        }

        public void offsetTimer(TimeSpan offset)
        {
            try
            {
                main.popOutChat.startTime = main.popOutChat.startTime.Subtract(offset);
            }
            catch (Exception e)
            {
                main.writeError("\n\nThere was an issue combining offset", e);
            }
        }

        public void loadConfigurations()
        {
            if (ConfigurationManager.AppSettings["titleBlock"] != null)
            {
                //main.popOutChat.titleBlock.FontFamily = new System.Windows.Media.FontFamily(fd.Font.Name);
                var fontName = ConfigurationManager.AppSettings["titleBlock"];
                main.popOutChat.titleBlock.FontFamily = new System.Windows.Media.FontFamily(fontName);
                main.popOutChat.titleBlock.FontSize = Double.Parse(ConfigurationManager.AppSettings["titleBlockSize"]);
                main.popOutChat.titleBlock.FontWeight = FontWeights.Regular;
                main.popOutChat.titleBlock.FontStyle = FontStyles.Normal;
                main.popOutChat.titleBlock.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom(ConfigurationManager.AppSettings["titleBlockColor"]));
            }

            if (ConfigurationManager.AppSettings["chatBlock"] != null)
            {
                var fontName = ConfigurationManager.AppSettings["chatBlock"];
                main.popOutChat.chatBlock.FontFamily = new System.Windows.Media.FontFamily(fontName);
                main.popOutChat.chatBlock.FontSize = Double.Parse(ConfigurationManager.AppSettings["chatBlockSize"]);
                main.popOutChat.chatBlock.FontWeight = FontWeights.Regular;
                main.popOutChat.chatBlock.FontStyle = FontStyles.Normal;
                main.popOutChat.chatBlock.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom(ConfigurationManager.AppSettings["chatBlockColor"]));
            }

            if (ConfigurationManager.AppSettings["clockTxt"] != null)
            {
                var fontName = ConfigurationManager.AppSettings["clockTxt"];
                main.popOutChat.clockTxt.FontFamily = new System.Windows.Media.FontFamily(fontName);
                main.popOutChat.clockTxt.FontSize = Double.Parse(ConfigurationManager.AppSettings["clockTxtSize"]);
                main.popOutChat.clockTxt.FontWeight = FontWeights.Regular;
                main.popOutChat.clockTxt.FontStyle = FontStyles.Normal;
                main.popOutChat.clockTxt.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom(ConfigurationManager.AppSettings["clockTxtColor"]));
            }

            if (ConfigurationManager.AppSettings["playBotControlDisplayGrid"] != null)
            {
                var fontName = ConfigurationManager.AppSettings["playBotControlDisplayGrid"];
                main.popOutChat.playBotControlDisplayGrid.FontFamily = new System.Windows.Media.FontFamily(fontName);
                main.popOutChat.playBotControlDisplayGrid.FontSize = Double.Parse(ConfigurationManager.AppSettings["playBotControlDisplayGridSize"]);
                main.popOutChat.playBotControlDisplayGrid.FontWeight = FontWeights.Regular;
                main.popOutChat.playBotControlDisplayGrid.FontStyle = FontStyles.Normal;
                main.popOutChat.playBotControlDisplayGrid.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom(ConfigurationManager.AppSettings["playBotControlDisplayGridColor"]));
            }

            if (ConfigurationManager.AppSettings["rightVote"] != null)
            {
                var fontName = ConfigurationManager.AppSettings["rightVote"];
                main.popOutChat.rightVote.FontFamily = new System.Windows.Media.FontFamily(fontName);
                main.popOutChat.rightVote.FontSize = Double.Parse(ConfigurationManager.AppSettings["rightVoteSize"]);
                main.popOutChat.rightVote.FontWeight = FontWeights.Regular;
                main.popOutChat.rightVote.FontStyle = FontStyles.Normal;
                main.popOutChat.rightVote.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom(ConfigurationManager.AppSettings["rightVoteColor"]));
            }

            if (ConfigurationManager.AppSettings["leftVote"] != null)
            {
                var fontName = ConfigurationManager.AppSettings["leftVote"];
                main.popOutChat.leftVote.FontFamily = new System.Windows.Media.FontFamily(fontName);
                main.popOutChat.leftVote.FontSize = Double.Parse(ConfigurationManager.AppSettings["leftVoteSize"]);
                main.popOutChat.leftVote.FontWeight = FontWeights.Regular;
                main.popOutChat.leftVote.FontStyle = FontStyles.Normal;
                main.popOutChat.leftVote.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom(ConfigurationManager.AppSettings["leftVoteColor"]));
            }

            if (ConfigurationManager.AppSettings["titleImage"] != null)
            {
                main.popOutChat.titleImage.Source = new BitmapImage(new Uri(ConfigurationManager.AppSettings["titleImage"]));
            }
        }

        public void saveDisplayFontSettings()
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;

            if (settings["titleBlock"] != null)
            {
                configFile.AppSettings.Settings["titleBlock"].Value = main.popOutChat.titleBlock.FontFamily.Source;
                configFile.AppSettings.Settings["titleBlock" + "Size"].Value = main.popOutChat.titleBlock.FontSize.ToString();
                configFile.AppSettings.Settings["titleBlock" + "Color"].Value = main.popOutChat.titleBlock.Foreground.ToString();
                configFile.Save();
            }
            else
            {
                configFile.AppSettings.Settings.Add("titleBlock", main.popOutChat.titleBlock.FontFamily.Source);
                configFile.AppSettings.Settings.Add("titleBlock" + "Size", main.popOutChat.titleBlock.FontSize.ToString());
                configFile.AppSettings.Settings.Add("titleBlock" + "Color", main.popOutChat.titleBlock.Foreground.ToString());
                configFile.Save();
            }

            if (settings["chatBlock"] != null)
            {
                configFile.AppSettings.Settings["chatBlock"].Value = main.popOutChat.chatBlock.FontFamily.Source;
                configFile.AppSettings.Settings["chatBlock" + "Size"].Value = main.popOutChat.chatBlock.FontSize.ToString();
                configFile.AppSettings.Settings["chatBlock" + "Color"].Value = main.popOutChat.chatBlock.Foreground.ToString();
                configFile.Save();
            }
            else
            {
                configFile.AppSettings.Settings.Add("chatBlock", main.popOutChat.chatBlock.FontFamily.Source);
                configFile.AppSettings.Settings.Add("chatBlock" + "Size", main.popOutChat.chatBlock.FontSize.ToString());
                configFile.AppSettings.Settings.Add("chatBlock" + "Color", main.popOutChat.chatBlock.Foreground.ToString());
                configFile.Save();
            }

            if (settings["clockTxt"] != null)
            {
                configFile.AppSettings.Settings["clockTxt"].Value = main.popOutChat.clockTxt.FontFamily.Source;
                configFile.AppSettings.Settings["clockTxt" + "Size"].Value = main.popOutChat.clockTxt.FontSize.ToString();
                configFile.AppSettings.Settings["clockTxt" + "Color"].Value = main.popOutChat.clockTxt.Foreground.ToString();
                configFile.Save();
            }
            else
            {
                configFile.AppSettings.Settings.Add("clockTxt", main.popOutChat.clockTxt.FontFamily.Source);
                configFile.AppSettings.Settings.Add("clockTxt" + "Size", main.popOutChat.clockTxt.FontSize.ToString());
                configFile.AppSettings.Settings.Add("clockTxt" + "Color", main.popOutChat.clockTxt.Foreground.ToString());
                configFile.Save();
            }

            if (settings["playBotControlDisplayGrid"] != null)
            {
                configFile.AppSettings.Settings["playBotControlDisplayGrid"].Value = main.popOutChat.playBotControlDisplayGrid.FontFamily.Source;
                configFile.AppSettings.Settings["playBotControlDisplayGrid" + "Size"].Value = main.popOutChat.playBotControlDisplayGrid.FontSize.ToString();
                configFile.AppSettings.Settings["playBotControlDisplayGrid" + "Color"].Value = main.popOutChat.playBotControlDisplayGrid.Foreground.ToString();
                configFile.Save();
            }
            else
            {
                configFile.AppSettings.Settings.Add("playBotControlDisplayGrid", main.popOutChat.playBotControlDisplayGrid.FontFamily.Source);
                configFile.AppSettings.Settings.Add("playBotControlDisplayGrid" + "Size", main.popOutChat.playBotControlDisplayGrid.FontSize.ToString());
                configFile.AppSettings.Settings.Add("playBotControlDisplayGrid" + "Color", main.popOutChat.playBotControlDisplayGrid.Foreground.ToString());
                configFile.Save();
            }

            if (settings["rightVote"] != null)
            {
                configFile.AppSettings.Settings["rightVote"].Value = main.popOutChat.rightVote.FontFamily.Source;
                configFile.AppSettings.Settings["rightVote" + "Size"].Value = main.popOutChat.rightVote.FontSize.ToString();
                configFile.AppSettings.Settings["rightVote" + "Color"].Value = main.popOutChat.rightVote.Foreground.ToString();
                configFile.Save();
            }
            else
            {
                configFile.AppSettings.Settings.Add("rightVote", main.popOutChat.rightVote.FontFamily.Source);
                configFile.AppSettings.Settings.Add("rightVote" + "Size", main.popOutChat.rightVote.FontSize.ToString());
                configFile.AppSettings.Settings.Add("rightVote" + "Color", main.popOutChat.rightVote.Foreground.ToString());
                configFile.Save();
            }

            if (settings["leftVote"] != null)
            {
                configFile.AppSettings.Settings["leftVote"].Value = main.popOutChat.leftVote.FontFamily.Source;
                configFile.AppSettings.Settings["leftVote" + "Size"].Value = main.popOutChat.leftVote.FontSize.ToString();
                configFile.AppSettings.Settings["leftVote" + "Color"].Value = main.popOutChat.leftVote.Foreground.ToString();
                configFile.Save();
            }
            else
            {
                configFile.AppSettings.Settings.Add("leftVote", main.popOutChat.leftVote.FontFamily.Source);
                configFile.AppSettings.Settings.Add("leftVote" + "Size", main.popOutChat.leftVote.FontSize.ToString());
                configFile.AppSettings.Settings.Add("leftVote" + "Color", main.popOutChat.leftVote.Foreground.ToString());
                configFile.Save();
            }
        }

        public void saveTitlePictureSource()
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            if (main.popOutChat.titleImage.Source == null)
                return;
            if (settings["titleImage"] != null)
            {
                configFile.AppSettings.Settings["titleImage"].Value = main.popOutChat.titleImage.Source.ToString();
                configFile.Save();
            }
            else
            {
                configFile.AppSettings.Settings.Add("titleImage", main.popOutChat.titleImage.Source.ToString());
                configFile.Save();
            }
        }
    }
}
