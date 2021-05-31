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
    public partial class SeleccionarPago : Window
    {

        public SeleccionarPago()
        {
            InitializeComponent();
            MSeleccion MSel = new MSeleccion();
            var res = MSel.MostrarEmpleado("");

            foreach (DEmpleado item in res)
            {
                item.nombre = item.nombre + " " + item.apellido;
            }

            CbEmpleado.ItemsSource = res;
            CbEmpleado.DisplayMemberPath = "nombre";
            CbEmpleado.SelectedValuePath = "idEmpleado";

            btnEnviar.Visibility = Visibility.Collapsed;


        }

        public DPago PagoSeleccionado;

        private void btnEnviar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void CbEmpleado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbEmpleado.SelectedIndex > -1)
            {
                PlaceEmpleado.Text = "";
            }
            else
            {
                PlaceEmpleado.Text = "Empleado";
            }
        }

        private void CbPago_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbPago.SelectedIndex > -1)
            {
                PlacePago.Text = "";
                btnEnviar.Visibility = Visibility.Visible;
                PagoSeleccionado = (DPago)CbPago.SelectedItem;
            }
            else
            {
                PlacePago.Text = "Pago";
                btnEnviar.Visibility = Visibility.Collapsed;
                PagoSeleccionado = null;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
