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
            if (searchType == SearchType.Departamento)
            {
                int id = CbDepartamento.SelectedIndex > -1 ? (int)CbDepartamento.SelectedValue : -1;
                var items = Metodos.MostrarByDepartamento(id);

                dgDepartamento.ItemsSource = items;
            }
            else if(searchType == SearchType.Empleado)
            {
                int id = CbEmpleado.SelectedIndex > -1 ? (int)CbEmpleado.SelectedValue : -1;
                var items = Metodos.MostrarByEmpleado(id);

                dgEmpleado.ItemsSource = items;
            }
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

        private void txtSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).CommandParameter;

            

            bool isEmpleado = searchType == SearchType.Empleado;
            if (isEmpleado)
            {
                var resp = Metodos.EncontrarByEmpleado(id)[0];
                Parentfrm.SeleccionarMeta(resp, isEmpleado);
                
            }
            else
            {
                var resp = Metodos.EncontrarByDepartamento(id)[0];
                Parentfrm.SeleccionarMeta(resp, isEmpleado);
            }

            this.Close();
        }

        SearchType searchType = SearchType.Departamento;

        private void RBEmpleado_Checked(object sender, RoutedEventArgs e)
        {
            GridCBEmpleado.Visibility = Visibility.Visible;
            dgEmpleado.Visibility = Visibility.Visible;
            searchType = SearchType.Empleado;
            Refresh();
        }

        private void RBDepartamento_Checked(object sender, RoutedEventArgs e)
        {
            GridCBEmpleado.Visibility = Visibility.Collapsed;
            dgEmpleado.Visibility = Visibility.Collapsed;
            searchType = SearchType.Departamento;
            Refresh();
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
            Refresh();
        }

        enum SearchType
        {
            Empleado,
            Departamento
        }
    }
}
