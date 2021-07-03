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

        MPago Metodo = new MPago();

        private void CbEmpleado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbEmpleado.SelectedIndex > -1)
            {

                var resp = Metodo.MostrarByEmpleado((int)CbEmpleado.SelectedValue);

                CbPago.ItemsSource = resp;
                CbPago.DisplayMemberPath = "PeriodoString";
                CbPago.SelectedValuePath = "idPago";

                GridPago.IsEnabled = true;

            }
            else
            {
                CbPago.SelectedIndex = -1;
                CbPago.IsEnabled = false;
            }
        }

        private void CbPago_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbPago.SelectedIndex > -1)
            {
                btnEnviar.Visibility = Visibility.Visible;
                PagoSeleccionado = (DPago)CbPago.SelectedItem;
            }
            else
            {
                btnEnviar.Visibility = Visibility.Collapsed;
                PagoSeleccionado = null;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
