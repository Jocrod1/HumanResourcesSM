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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HumanResourcesSM.Windows
{
    /// <summary>
    /// Interaction logic for UserInputText.xaml
    /// </summary>
    public partial class UserInputText : UserControl
    {
        public string Placeholder { get; set; }

        public UserInputText()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public void SetText(string text)
        {
            if(txt.Text == "")
            {
                txt.Foreground = Brushes.Gray;
                txt.Text = Placeholder;
            }
            else
            {
                txt.Foreground = Brushes.Black;
                txt.Text = text;
            }
        }

        private void txt_GotFocus(object sender, RoutedEventArgs e)
        {
            if(txt.Text == "")
            {
                txtPlaceHolder.Visibility = Visibility.Collapsed;
            }
        }

        private void txt_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txt.Text == "")
            {
                txtPlaceHolder.Visibility = Visibility.Visible;
            }
        }
    }
}
