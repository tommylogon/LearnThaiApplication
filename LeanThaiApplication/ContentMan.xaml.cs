using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            this.Topmost = true;
        }

        private void WindowDeactivated(object sender, EventArgs e)
        {
            this.Topmost = true;
            this.Activate();
        }

        private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((Viewbox)this.Content).Width = this.Width;

            /*foreach (Control cont in ((StackPanel)((Viewbox)this.Content).Child).Children)
            {
                if (cont is TextBox txt)
                {
                    txt.Width = ((Viewbox)this.Content).Width;
                }
            }*/
        }
    }
}