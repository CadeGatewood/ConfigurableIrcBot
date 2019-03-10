using System.Windows;
using System.Windows.Input;

using System.Text.RegularExpressions;
using System;
using System.Windows.Media;

namespace ConfigurableIrcBotApp
{
    /// <summary>
    /// Interaction logic for ChatDisplaySettings.xaml
    /// </summary>
    public partial class ChatDisplaySettings : Window
    {
        private MainWindow main;
        private PopOutChat popOutChat;

        public ChatDisplaySettings(MainWindow main)
        {
            InitializeComponent();
            this.main = main;
            this.popOutChat = main.popOutChat;
        }

        private void numberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public void FontFileDrop(object sender, DragEventArgs e)
        {

        }

        private void changeFont_Click(object sender, RoutedEventArgs e)
        {
            if(fontSelection.SelectedValue == null)
            {
                main.write("Please select a font");
            }
            

        }

        private void changeColor_Click(object sender, RoutedEventArgs e)
        {
            editSectionColor(sectionSelection.SelectedValue.ToString());
        }

        private void changeFontSize_Click(object sender, RoutedEventArgs e)
        {
            editFontSize(sectionSelection.SelectedValue.ToString());
        }

        private void editFont(string section)
        {
            switch (section)
            {
                case "Chat":
                    //write something to convert font selection to file

                    //popOutChat.chatBlock.FontFamily(fontSelection.SelectedValue)
                    break;
                
            }

        }
        private void editFontSize(string section)
        {
            switch (section)
            {
                case "Chat":
                    popOutChat.chatBlock.FontSize = Convert.ToDouble(fontSizeEntry.Text);
                    break;
                case "Title":
                    popOutChat.TitleBlock.FontSize = Convert.ToDouble(fontSizeEntry.Text);
                    break;
                case "Timer":
                    popOutChat.clockTxt.FontSize = Convert.ToDouble(fontSizeEntry.Text);
                    break;
                default:
                    main.write("Please enter a font size");
                    break;
            }
        }

        private void editSectionColor(string section)
        {
            switch (section)
            {
                case "Chat":
                    popOutChat.chatBlock.Foreground = (Brush) new BrushConverter().ConvertFromString(text: colorEntry.Text);
                    break;
                case "Title":
                    popOutChat.TitleBlock.Foreground = (Brush)new BrushConverter().ConvertFromString(text: colorEntry.Text);
                    break;
                case "Timer":
                    popOutChat.clockTxt.Foreground = (Brush)new BrushConverter().ConvertFromString(text: colorEntry.Text);
                    break;
                default:
                    main.write("Please enter a hex color code");
                    break;
            }
        }
    }
}
