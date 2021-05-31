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
    /// <summary>
    /// Interaction logic for SeleccionarUsuario.xaml
    /// </summary>
    public partial class SeleccionarUsuario : Window
    {

        public SeleccionarUsuario(bool isDateOnly)
        {
            InitializeComponent();
            if (isDateOnly)
            {
                GridUsuario.Visibility = Visibility.Collapsed;
            }
            else
            {
                var res = new MUsuario().Mostrar("");

                CbUsuario.ItemsSource = res;
                CbUsuario.DisplayMemberPath = "usuario";
                CbUsuario.SelectedValuePath = "idUsuario";

                btnEnviar.Visibility = Visibility.Collapsed;
            }

        }

        public DUsuario UsuarioSeleccionado;
        public DateTime? FechaInicioSel, FechaFinalSel;

        private void CbUsuario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbUsuario.SelectedIndex > -1)
            {
                PlaceUsuario.Text = "";
                btnEnviar.Visibility = Visibility.Visible;
                UsuarioSeleccionado = (DUsuario)CbUsuario.SelectedItem;
            }
            else
            {
                PlaceUsuario.Text = "Usuario";
                btnEnviar.Visibility = Visibility.Collapsed;
                UsuarioSeleccionado = null;
            }
        }

        private void btnEnviar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void CbFechaInicio_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbFechaInicio.SelectedDate != null)
            {
                PlaceFechaInicio.Text = "";

                CbFechaFinal.DisplayDateStart = CbFechaInicio.SelectedDate?.Date;
            }
            else
            {
                PlaceFechaInicio.Text = "Fecha de Inicio";

                CbFechaFinal.DisplayDateStart = null;
            }
            FechaInicioSel = CbFechaInicio.SelectedDate;
        }

        private void CbFechaFinal_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbFechaFinal.SelectedDate != null)
            {
                PlaceFechaFinal.Text = "";

                CbFechaInicio.DisplayDateEnd = CbFechaFinal.SelectedDate?.Date;
            }
            else
            {
                PlaceFechaFinal.Text = "Fecha Final";

                CbFechaInicio.DisplayDateEnd = null;
            }
            FechaFinalSel = CbFechaFinal.SelectedDate;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DateTime now = DateTime.Now;
            var startdate = new DateTime(now.Year, now.Month, 1);
            var enddate = startdate.AddMonths(1).AddDays(-1);

            CbFechaInicio.SelectedDate = startdate;
            CbFechaFinal.SelectedDate = enddate;
        }
    }
}
