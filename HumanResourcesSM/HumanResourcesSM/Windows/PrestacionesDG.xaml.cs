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
    public partial class PrestacionesDG : Page
    {
        MTipoTramite Metodos = new MTipoTramite();

        public PrestacionesDG()
        {
            InitializeComponent();
        }

        public void Refresh()
        {

            List<DTipoTramite> items = Metodos.Mostrar("");


            dgOperaciones.ItemsSource = items;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //contentsp.Children.Clear();

            Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            TipoTramiteFrm frmTrab = new TipoTramiteFrm();
            bool Resp = frmTrab.ShowDialog() ?? false;
            Refresh();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).CommandParameter;
            var response = Metodos.Encontrar(id);

            TipoTramiteFrm frm = new TipoTramiteFrm();
            frm.Type = TypeForm.Update;
            frm.DataFill = response[0];
            bool Resp = frm.ShowDialog() ?? false;
            Refresh();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            Refresh();
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult Resp = MessageBox.Show("¿Seguro que quieres eliminar este item?", "Magicolor", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (Resp != MessageBoxResult.Yes)
                return;
            int id = (int)((Button)sender).CommandParameter;
            var resp = Metodos.Eliminar(id);

            if (resp.Equals("OK"))
            {
                MAuditoria.Insertar(new DAuditoria(
                                    Menu.ActUsuario.idUsuario,
                                    DAuditoria.Eliminar,
                                    "Se ha Eliminado el Tipo Tramite Nº" + id));

                MessageBox.Show("Eliminar completado!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else MessageBox.Show(resp);

            Refresh();
        }

        private void txtVer_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).CommandParameter;
            var response = Metodos.Encontrar(id);

            TipoTramiteFrm frmTrab = new TipoTramiteFrm();
            frmTrab.Type = TypeForm.Read;
            frmTrab.DataFill = response[0];
            bool Resp = frmTrab.ShowDialog() ?? false;
            Refresh();

            //MessageBox.Show(response[0].fechaNacimiento.ToString());
        }

        private void RBSolicitudes_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void RBOtorgados_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void RBNoOtorgados_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
