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
using System.Globalization;

using Datos;
using Metodos;

namespace HumanResourcesSM.Windows
{
    /// <summary>
    /// Interaction logic for SeleccionFrm.xaml
    /// </summary>
    public partial class AsignacionFrm : Page
    {
        public AsignacionFrm()
        {
            InitializeComponent();
        }

        MUsuario Metodos = new MUsuario();

        void RefreshDG() {
            var usuarios = Metodos.Mostrar("");

            List<ModeloUsuario> NoEntrevistadores = new List<ModeloUsuario>();
            List<ModeloUsuario> Entrevistadores = new List<ModeloUsuario>();

            foreach(DUsuario item in usuarios)
            {
                string rol = item.idRol == 1 ? "Administrador" : "Normal";

                if(item.entrevistando == 0)
                {
                    NoEntrevistadores.Add(new ModeloUsuario(item.idUsuario, item.usuario, rol));
                }
                else
                {
                    Entrevistadores.Add(new ModeloUsuario(item.idUsuario, item.usuario, rol));
                }
            }

            DgUsuarios.ItemsSource = NoEntrevistadores;
            DgEntrevistadores.ItemsSource = Entrevistadores;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshDG();
        }

        private void txtVer_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).CommandParameter;

            Metodos.Entrevistando(id, true);

            RefreshDG();
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).CommandParameter;

            Metodos.Entrevistando(id, false);

            RefreshDG();
        }

        class ModeloUsuario
        {
            public ModeloUsuario(int idUsuario, string usuario, string rol)
            {
                this.idUsuario = idUsuario;
                this.usuario = usuario;
                this.rol = rol;
            }

            public int idUsuario { get; set; }
            public string usuario { get; set; }
            public string rol { get; set; }
        }
    }
}
