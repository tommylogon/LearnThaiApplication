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

        private void Focused(object sender, EventArgs e)
        {
            this.Width = 350;
            
            this.Topmost = true;
        }

        private void LooseFocus(object sender, EventArgs e)
        {
            this.Topmost = true;
            this.Activate();
        }
    }
}
