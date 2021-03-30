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
    public partial class MetaVista : Window
    {
        MMeta Metodos = new MMeta();

        EvaluacionFrm Parentfrm;

        public MetaVista(EvaluacionFrm par)
        {
            InitializeComponent();

            Parentfrm = par;
        }

        public void Refresh()
        {

            var items = Metodos.MostrarByDepartamento(CbDepartamento.SelectedIndex);

            dgDepartamento.ItemsSource = items;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //contentsp.Children.Clear();

            var res = new MDepartamento().Mostrar("");

            CbDepartamento.ItemsSource = res;
            CbDepartamento.DisplayMemberPath = "nombre";
            CbDepartamento.SelectedValuePath = "idDepartamento";

            RBDepartamento.IsChecked = true;
            Refresh();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            Refresh();
        }

        private void txtSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).CommandParameter;

            var resp = Metodos.Encontrar(id)[0];

            bool isEmpleado = RBEmpleado.IsChecked ?? false;

            Parentfrm.SeleccionarMeta(resp, isEmpleado);
            this.Close();
        }
        private void RBEmpleado_Checked(object sender, RoutedEventArgs e)
        {
            GridCBEmpleado.Visibility = Visibility.Visible;
        }

        private void RBDepartamento_Checked(object sender, RoutedEventArgs e)
        {
            GridCBEmpleado.Visibility = Visibility.Collapsed;
        }

        private void CbDepartamento_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbDepartamento.SelectedIndex > -1)
            {
                PlaceDepartamento.Text = "";

                var resp = new MSeleccion().MostrarEmpleadoByDepartamento((int)CbDepartamento.SelectedValue);

                CbEmpleado.ItemsSource = resp;
                CbEmpleado.DisplayMemberPath = "nombre";
                CbEmpleado.SelectedValuePath = "idEmpleado";

                CbEmpleado.IsEnabled = true;
                Refresh();
            }
            else
            {
                PlaceDepartamento.Text = "Seleccionar Departamento";
                CbEmpleado.IsEnabled = false;
                CbEmpleado.SelectedIndex = -1;
            }
        }

        private void CbEmpleado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbEmpleado.SelectedIndex > -1)
            {
                PlaceEmpleado.Text = "";
                Refresh();
            }
            else
            {
                PlaceEmpleado.Text = "Seleccionar Empleado";
            }
        }

        private void txtLimpiar_Click(object sender, RoutedEventArgs e)
        {
            CbDepartamento.SelectedIndex = -1;
        }
    }
}
