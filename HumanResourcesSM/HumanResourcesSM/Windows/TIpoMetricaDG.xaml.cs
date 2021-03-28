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
    public partial class TipoMetricaDG : Page
    {
        MTipoMetrica Metodos = new MTipoMetrica();

        public TipoMetricaDG()
        {
            InitializeComponent();

            var res = new MDepartamento().Mostrar("");

            CbDepartamento.ItemsSource = res;
            CbDepartamento.DisplayMemberPath = "nombre";
            CbDepartamento.SelectedValuePath = "idDepartamento";
        }

        public void Refresh()
        {
            int idDepartemento = CbDepartamento.SelectedIndex > -1 ? (int)CbDepartamento.SelectedValue : -1;

            var Resp = Metodos.Mostrar(idDepartemento);

            dgOperaciones.ItemsSource = Resp;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            TipoMetricaFrm frmTrab = new TipoMetricaFrm();
            bool Resp = frmTrab.ShowDialog() ?? false;
            Refresh();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).CommandParameter;
            var response = Metodos.Encontrar(id);

            TipoMetricaFrm frm = new TipoMetricaFrm();
            frm.Type = TypeForm.Update;
            frm.DataFill = response[0];
            bool Resp = frm.ShowDialog() ?? false;
            Refresh();
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult Resp = MessageBox.Show("¿Seguro que quieres eliminrar este item?", "Magicolor", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (Resp != MessageBoxResult.Yes)
                return;
            int id = (int)((Button)sender).CommandParameter;
            var response = Metodos.Encontrar(id)[0];
            Metodos.Eliminar(response.idTipoMetrica);
            Refresh();
        }
        private void txtVer_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).CommandParameter;
            var response = Metodos.Encontrar(id);

            TipoMetricaFrm frmTrab = new TipoMetricaFrm();
            frmTrab.Type = TypeForm.Read;
            frmTrab.DataFill = response[0];
            bool Resp = frmTrab.ShowDialog() ?? false;
            Refresh();
        }

        private void CbEmpleado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
            if (CbDepartamento.SelectedIndex > -1)
            {
                PlaceDepartamento.Text = "";
            }
            else
            {
                PlaceDepartamento.Text = "Departamento";
            }
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            CbDepartamento.SelectedIndex = -1;
        }
    }
}
