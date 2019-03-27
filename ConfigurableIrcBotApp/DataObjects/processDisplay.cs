using System.Windows.Media;

namespace ConfigurableIrcBotApp.DataObjects
{

    class processDisplay
    {

        public ImageSource imageSource { get; set; }
        public string processName { get; set; }

        public processDisplay(ImageSource imageSource, string processName)
        {
            this.imageSource = imageSource;
            this.processName = processName;
        }

    }
}
