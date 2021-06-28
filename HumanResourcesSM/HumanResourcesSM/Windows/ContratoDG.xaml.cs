using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
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
            int idUsuario = Menu.ActUsuario.idRol != 4 ? -1 : Menu.ActUsuario.idUsuario; 

            var items = Metodos.ListadoEmpleadoContrato(nombre, idUsuario);

            dgOperaciones.ItemsSource = items;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(Menu.ActUsuario.idRol == 4)
            {
                dgOperaciones.Columns[3].Visibility = Visibility.Collapsed;
            }

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
    public class ChangeRedColorRowInteviewDates : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            DateTime FechaEntrevista = DateTime.Parse(values[0].ToString());

            if (FechaEntrevista < DateTime.Today)
            {
                return (SolidColorBrush)(new BrushConverter().ConvertFrom("Red"));
            }
            else
            {
                return (SolidColorBrush)(new BrushConverter().ConvertFrom("Black"));
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
