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

using Datos;
using Metodos;

namespace HumanResourcesSM.Windows
{
    /// <summary>
    /// Interaction logic for DepartamentoDG.xaml
    /// </summary>
    public partial class PagoSeleccionVista : Window
    {
        MPago Metodos = new MPago();

        PagoFrm Parentfrm;

        public PagoSeleccionVista(PagoFrm par)
        {
            InitializeComponent();

            Parentfrm = par;
        }

        public void Refresh()
        {
            var items = Metodos.MostrarEmpleado(txtBuscar.Text);

            dgEmpleado.ItemsSource = items;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //contentsp.Children.Clear();

            Refresh();
        }

        private void txtSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).CommandParameter;
            var resp = Metodos.MostrarEmpleadoDetalle(id)[0];
            Parentfrm.SetEmpleado(resp);

            this.Close();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            Refresh();
        }

        private void txtBuscar_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtBuscar.Text == "")
            {
                txtBucarPlaceH.Text = "";
            }

        }

        private void txtBuscar_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtBuscar.Text == "")
            {
                txtBucarPlaceH.Text = "Buscar...";
            }

        }
    }
}
