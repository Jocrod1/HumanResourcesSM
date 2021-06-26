using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Datos;
using Metodos;

namespace HumanResourcesSM.Windows
{

    public partial class AsignarEntrevistadorDG : Page
    {
        MSeleccion Metodos = new MSeleccion();

        public AsignarEntrevistadorDG()
        {
            InitializeComponent();

        }

        public void Refresh(string nombre)
        {
            var items = Metodos.MostrarEmpleadoRegistrado(nombre);

            dgOperaciones.ItemsSource = items;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh(txtBuscar.Text);
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).CommandParameter;

            List<DEmpleado> Empleado = new MSeleccion().EncontrarEmpleado(id);

            AsignarEntrevistadorFrm frm = new AsignarEntrevistadorFrm(Empleado[0]);
            frm.ShowDialog();

            Refresh(txtBuscar.Text);
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





    public class DesactivateInformationNotSelected : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            string usuario = value.ToString();

            if (usuario != "")
            {
                return "Visible";
            }
            else
            {
                return "Collapsed";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
