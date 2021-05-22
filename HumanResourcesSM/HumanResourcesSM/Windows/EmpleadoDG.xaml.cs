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
    public partial class EmpleadoDG : Page
    {
        MSeleccion Metodos = new MSeleccion();

        public EmpleadoDG()
        {
            InitializeComponent();
        }

        public void Refresh(string search)
        {

            var items = Metodos.MostrarEmpleadoDG(search);

            dgOperaciones.ItemsSource = items;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //contentsp.Children.Clear();

            Refresh(txtBuscar.Text);
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).CommandParameter;

            var resp = Metodos.AnularEmpleado(id);
            Refresh(txtBuscar.Text);

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).CommandParameter;
            

            EmpleadoFrm frmTrab = new EmpleadoFrm(id);
            bool Resp = frmTrab.ShowDialog() ?? false;
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

        private void BtnReporte_Click(object sender, RoutedEventArgs e)
        {
            if (dgOperaciones.Items.Count == 0)
            {
                MessageBox.Show("No se puede realizar un Reporte vacio!", "SwissNet", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                return;
            }

            SeleccionarUsuario dialog = new SeleccionarUsuario();
            if(dialog.ShowDialog() ?? false)
            {
                Reports.Reporte reporte = new Reports.Reporte();
                reporte.ExportPDF(Metodos.EntrevistadosPorUsuario(dialog.UsuarioSeleccionado.idUsuario), "EntrevistadosPorEmpleado");
            }

             
            
        }
    }
}
