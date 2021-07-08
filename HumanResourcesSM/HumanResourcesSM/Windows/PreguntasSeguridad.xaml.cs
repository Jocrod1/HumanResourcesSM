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
    public partial class PreguntasSeguridad : Window
    {
        Login ParentForm;

        public PreguntasSeguridad(Login parentfrm)
        {
            InitializeComponent();

            ParentForm = parentfrm;
        }



        public List<DSeguridad> DataFill;


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Create();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtTitulo.Text = DataFill[0].usuario;


            int id = Login.idSecretQ;
            txtPregunta.Text = DataFill[id].pregunta;
        }

        private bool Validate()
        {
            if (txtRespuesta.Password == "")
            {
                MessageBox.Show("Debes responder la pregunta!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                txtRespuesta.Focus();
                return true;
            }

            return false;
        }

        void Create()
        {
            if (Validate()) return;

            if ((txtRespuesta.Password == DataFill[0].respuesta))
            {
                CambiarContraseña frmContraseña = new CambiarContraseña(this);
                frmContraseña.DataFill = DataFill[0];
                MessageBox.Show("Datos correctos, debes introducir una nueva contraseña!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Hide();
                bool Resp = frmContraseña.ShowDialog() ?? false;
            }
            else
            {
                MessageBox.Show("Datos Incorrectos!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
