using System;
using System.Windows;
using System.Windows.Controls;

namespace LearnThaiApplication
{
    /// <summary>
    /// Interaction logic for ContentMan.xaml
    /// </summary>
    public partial class ContentMan : Window
    {
        public object DataItem { get; set; }

        public ContentMan()
        {
            InitializeComponent();
        }

        private void WindowActivated(object sender, EventArgs e)
        {
            //this.Topmost = true;
        }

        private void WindowDeactivated(object sender, EventArgs e)
        {
            //this.Topmost = true;
            //this.Activate();
        }

        private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((Viewbox)this.Content).Width = this.Width;
        }
    }
}