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
    /// Interaction logic for AuditoriaDG.xaml
    /// </summary>
    public partial class AuditoriaDG : Window
    {
        public MAuditoria Metodos = new MAuditoria();

        public AuditoriaDG()
        {
            InitializeComponent();
        }

        public void Refresh()
        {
            string accion = "";

            if (CbAcciones.SelectedIndex > -1)
                accion = (string)CbAcciones.SelectedValue;

            string Usuario = "";

            if (CbUsuario.SelectedIndex > -1)
                Usuario = (string)CbUsuario.SelectedValue;

            List<DAuditoria> DisplayData = Metodos.Mostrar(dpFechaInicio.SelectedDate, dpFechaFinal.SelectedDate, Usuario, accion);

            dgOperaciones.ItemsSource = DisplayData;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var res = new MUsuario().Mostrar("");

            CbUsuario.ItemsSource = res;
            CbUsuario.DisplayMemberPath = "usuario";
            CbUsuario.SelectedValuePath = "usuario";

            var resAcciones = new MAuditoria().MostrarAcciones();

            CbAcciones.ItemsSource = resAcciones;
            CbAcciones.DisplayMemberPath = "accion";
            CbAcciones.SelectedValuePath = "accion";

            DateTime StartofWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            dpFechaInicio.SelectedDate = StartofWeek;
            dpFechaFinal.SelectedDate = StartofWeek.AddDays(6);

            Refresh();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            Refresh();
        }

        private void CbAcciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbAcciones.SelectedIndex > -1)
                PlaceAcciones.Text = "";
            else
                PlaceAcciones.Text = "Acciones";

            Refresh();
        }

        private void dpFechaInicio_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpFechaInicio.SelectedDate != null)
            {
                PlaceFechaInicio.Text = "";

                dpFechaFinal.DisplayDateStart = dpFechaInicio.SelectedDate?.Date;
            }
            else
            {
                PlaceFechaInicio.Text = "Fecha de Inicio";

                dpFechaFinal.DisplayDateStart = null;
            }
            Refresh();
        }

        private void dpFechaFinal_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpFechaFinal.SelectedDate != null)
            {
                PlaceFechaFinal.Text = "";

                dpFechaInicio.DisplayDateEnd = dpFechaFinal.SelectedDate?.Date;
            }
            else
            {
                PlaceFechaFinal.Text = "Fecha Final";

                dpFechaInicio.DisplayDateEnd = null;
            }
            Refresh();
        }

        private void CbUsuario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbUsuario.SelectedIndex > -1)
            {
                PlaceUsuario.Text = "";
            }
            else
            {
                PlaceUsuario.Text = "Usuario";
            }
            Refresh();
        }
    }
}
