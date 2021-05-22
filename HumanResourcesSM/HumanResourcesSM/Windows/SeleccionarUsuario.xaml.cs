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
    /// Interaction logic for SeleccionarUsuario.xaml
    /// </summary>
    public partial class SeleccionarUsuario : Window
    {
        public SeleccionarUsuario()
        {
            InitializeComponent();
            var res = new MUsuario().Mostrar("");

            CbUsuario.ItemsSource = res;
            CbUsuario.DisplayMemberPath = "usuario";
            CbUsuario.SelectedValuePath = "idUsuario";


            btnEnviar.Visibility = Visibility.Collapsed;
        }

        public DUsuario UsuarioSeleccionado;

        private void CbUsuario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbUsuario.SelectedIndex > -1)
            {
                PlaceUsuario.Text = "";
                btnEnviar.Visibility = Visibility.Visible;
                UsuarioSeleccionado = (DUsuario)CbUsuario.SelectedItem;
            }
            else
            {
                PlaceUsuario.Text = "Usuario";
                btnEnviar.Visibility = Visibility.Collapsed;
                UsuarioSeleccionado = null;
            }
        }

        private void btnEnviar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
