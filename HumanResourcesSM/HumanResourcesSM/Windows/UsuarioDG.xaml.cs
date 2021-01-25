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
    public partial class UsuarioDG : Page
    {
        MUsuario Metodos = new MUsuario();

        public UsuarioDG()
        {
            InitializeComponent();
        }

        public void Refresh(string search)
        {

            List<DUsuario> items = Metodos.Mostrar(search);

            List<ModelUsuario> MIT = new List<ModelUsuario>();

            MRol MEtR = new MRol();

            var resp = MEtR.Mostrar();

            foreach(DUsuario item in items)
            {
                DRol DAR = resp.Find((Rol) => Rol.idRol == item.idRol);

                MIT.Add(new ModelUsuario(item.idUsuario, item.usuario, DAR.nombre));
            }


            dgOperaciones.ItemsSource = MIT;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //contentsp.Children.Clear();

            Refresh(txtBuscar.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            UsuarioFrm frmTrab = new UsuarioFrm();
            bool Resp = frmTrab.ShowDialog() ?? false;
            Refresh(txtBuscar.Text);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).CommandParameter;
            var response = Metodos.Encontrar(id);

            UsuarioFrm frm = new UsuarioFrm();
            frm.Type = TypeForm.Update;
            frm.DataFill = response[0];
            bool Resp = frm.ShowDialog() ?? false;
            Refresh(txtBuscar.Text);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            Refresh(txtBuscar.Text);
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult Resp = MessageBox.Show("¿Seguro que quieres eliminrar este item?", "Magicolor", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (Resp != MessageBoxResult.Yes)
                return;
            int id = (int)((Button)sender).CommandParameter;
            DUsuario item = new DUsuario()
            {
                idUsuario = id
            };
            Metodos.Eliminar(item);
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

        private void txtVer_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).CommandParameter;
            var response = Metodos.Encontrar(id);

            UsuarioFrm frmTrab = new UsuarioFrm();
            frmTrab.Type = TypeForm.Read;
            frmTrab.DataFill = response[0];
            bool Resp = frmTrab.ShowDialog() ?? false;
            Refresh(txtBuscar.Text);
        }

        public class ModelUsuario
        {
            public ModelUsuario(int idUsuario, string usuario, string Rol)
            {
                this.idUsuario = idUsuario;
                this.usuario = usuario;
                rol = Rol;
            }

            public int idUsuario { get; set; }
            public string usuario { get; set; }
            public string rol { get; set; }
        }

    }
}
