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

using Datos;
using Metodos;

namespace HumanResourcesSM.Windows
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public DUsuario ActUsuario;
        public Menu(DUsuario User)
        {
            InitializeComponent();
            ActUsuario = User;

            RelacionesLaboralesDG frm = new RelacionesLaboralesDG();

            ContentFrame.Content = frm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TipoTramiteDG frm = new TipoTramiteDG();

            ContentFrame.Content = frm;
        }
    }
}
