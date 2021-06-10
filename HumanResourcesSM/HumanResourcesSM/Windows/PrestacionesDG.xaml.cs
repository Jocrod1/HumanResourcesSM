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

    public partial class PrestacionesDG : Page
    {
        MPrestacion Metodos = new MPrestacion();

        public PrestacionesDG()
        {
            InitializeComponent();
        }

        public void Refresh()
        {

            List<DPrestacion> items = Metodos.Mostrar(CbEmpleado.Text, TipoEstado());

            dgOperaciones.ItemsSource = items;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RBSolicitudes.IsChecked = true;

            Refresh();

            var resp = new MSeleccion().MostrarEmpleado();

            CbEmpleado.ItemsSource = resp;
            CbEmpleado.DisplayMemberPath = "nombre";
            CbEmpleado.SelectedValuePath = "nombre";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrestacionesFrm frmPrest = new PrestacionesFrm();
            bool Resp = frmPrest.ShowDialog() ?? false;
            Refresh();
        }


        private void txtVer_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).CommandParameter;
            List<DPrestacion> response = Metodos.Encontrar(id);

            RespuestaPrestacionFrm frmPrest = new RespuestaPrestacionFrm(response[0]);

            bool Resp = frmPrest.ShowDialog() ?? false;

            CbEmpleado.Text = "";
            RBSolicitudes.IsChecked = true;
            Refresh();

        }

        private void RBSolicitudes_Checked(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void RBOtorgados_Checked(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void RBNoOtorgados_Checked(object sender, RoutedEventArgs e)
        {
            Refresh();
        }


        private void CbEmpleado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private int TipoEstado ()
        {
            if(RBSolicitudes.IsChecked == true)
            {
                return 1;
            }
            if(RBOtorgados.IsChecked == true)
            {
                return 2;
            }
            if(RBNoOtorgados.IsChecked == true)
            {
                return 3;
            }
            return 0;
        }

    }
}
