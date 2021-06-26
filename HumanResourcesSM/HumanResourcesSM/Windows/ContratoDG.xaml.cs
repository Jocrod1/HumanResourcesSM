using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Datos;
using Metodos;

namespace HumanResourcesSM.Windows
{

    public partial class ContratoDG : Page
    {
        MSeleccion Metodos = new MSeleccion();

        GestionMenu Parent = new GestionMenu();

        public ContratoDG(GestionMenu parent)
        {
            InitializeComponent();

            Parent = parent;
        }

        public void Refresh(string nombre)
        {
            var items = Metodos.ListadoEmpleadoContrato(nombre);

            dgOperaciones.ItemsSource = items;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh(txtBuscar.Text);
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).CommandParameter;

            List<DEmpleado> Empleado = new MSeleccion().EmpleadoEntrevista(id);

            Parent.MostrarDetalleSeleccionado(Empleado[0]);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            Refresh(txtBuscar.Text);
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
