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
    public partial class DeudaDG : Page
    {
        MDeuda Metodos = new MDeuda();

        public DeudaDG()
        {
            InitializeComponent();

            RbNoPagado.IsChecked = true;

            var res = new MSeleccion().MostrarEmpleado("");

            foreach (DEmpleado item in res)
            {
                item.nombre = item.nombre + " " + item.apellido;
            }

            CbEmpleado.ItemsSource = res;
            CbEmpleado.DisplayMemberPath = "nombre";
            CbEmpleado.SelectedValuePath = "idEmpleado";
        }

        public void Refresh()
        {
            int idEmpleado = CbEmpleado.SelectedIndex > -1 ? (int)CbEmpleado.SelectedValue : -1;

            int? SearchStatus = 1;
            if (RbNoPagado.IsChecked ?? false)
                SearchStatus = 1;
            else if (RbPagado.IsChecked ?? false)
                SearchStatus = 2;
            else if (RbAmbos.IsChecked ?? false)
                SearchStatus = null;

            
            var Resp = Metodos.MostrarDeudaEmpleado(idEmpleado, SearchStatus, CbTipoDeuda.SelectedIndex);

            dgOperaciones.ItemsSource = Resp;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            DeudaFrm frmTrab = new DeudaFrm();
            bool Resp = frmTrab.ShowDialog() ?? false;
            Refresh();
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult Resp = MessageBox.Show("¿Seguro que quieres eliminar este item?", "Magicolor", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (Resp != MessageBoxResult.Yes)
                return;
            int id = (int)((Button)sender).CommandParameter;
            //var response = Metodos.Encontrar(id)[0];
            var resp = Metodos.Anular(id);

            if (resp.Equals("OK"))
            {
                MAuditoria.Insertar(new DAuditoria(
                                    Menu.ActUsuario.idUsuario,
                                    DAuditoria.Eliminar,
                                    "Se ha eliminado la dedua Nº" + id));

                MessageBox.Show("Eliminar completado!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else MessageBox.Show(resp);

            Refresh();
        }

        private void txtVer_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).CommandParameter;
            var response = Metodos.Encontrar(id);

            DeudaFrm frmTrab = new DeudaFrm();
            frmTrab.Type = TypeForm.Read;
            frmTrab.DataFill = response[0];
            bool Resp = frmTrab.ShowDialog() ?? false;
            Refresh();
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
            Refresh();
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            CbEmpleado.SelectedIndex = -1;
        }

        private void RbSearch(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void BtnReporte_Click(object sender, RoutedEventArgs e)
        {
            if (dgOperaciones.Items.Count == 0)
            {
                MessageBox.Show("No se puede realizar un Reporte vacio!", "SwissNet", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                return;
            }

            int idEmpleado = CbEmpleado.SelectedIndex > -1 ? (int)CbEmpleado.SelectedValue : -1;

            int? SearchStatus = 1;
            if (RbNoPagado.IsChecked ?? false)
                SearchStatus = 1;
            else if (RbPagado.IsChecked ?? false)
                SearchStatus = 2;
            else if (RbAmbos.IsChecked ?? false)
                SearchStatus = null;


            Reports.Reporte reporte = new Reports.Reporte();
            reporte.ExportPDF(Metodos.DeudasPorEmpleado(idEmpleado, SearchStatus, CbTipoDeuda.SelectedIndex), "DeudaActiva");
        }

        private void CbTipoDeuda_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbTipoDeuda.SelectedIndex > -1)
            {
                PlaceTipoDeuda.Text = "";
            }
            else
            {
                PlaceTipoDeuda.Text = "Tipo Deuda";
            }
            Refresh();
        }
    }
}
