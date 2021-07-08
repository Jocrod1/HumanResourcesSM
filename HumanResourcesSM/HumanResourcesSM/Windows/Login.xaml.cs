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
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();

            if(randComp == null)
            {
                randComp = new Random(DateTime.Now.Second + DateTime.Now.Millisecond);
                idSecretQ = randComp.Next(0, 3);
            }
        }

        public static Random randComp;
        public static int idSecretQ;

        public void Log()
        {
            if (txtUsuario.Text == "")
            {
                MessageBox.Show("¡Debe escribir su Usuario!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (txtPassword.Password == "")
            {
                MessageBox.Show("¡Debe escribir la Contraseña!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MUsuario Metodo = new MUsuario();

            var response = Metodo.Login(txtUsuario.Text, txtPassword.Password);

            if (response.Count > 0)
            {
                MAuditoria.Insertar(new DAuditoria(
                                                response[0].idUsuario,
                                                MAuditoria.IniciarSesion,
                                                "Ha Iniciado Sesión"));


                Menu Frm = new Menu(response[0]);
                Frm.Closing += new System.ComponentModel.CancelEventHandler(Window_Closing);
                Frm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("¡Credenciales Erroneas!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            Log();
        }

        private void StackPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Log();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Close();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (txtUsuario.Text == "")
            {
                MessageBox.Show("Debe proporcionar un Nombre de Usuario en el login", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                txtUsuario.Focus();
            }
            else
            {
                MUsuario Metodo = new MUsuario();

                var res = Metodo.Seguridad(txtUsuario.Text);

                if(res.Count > 0)
                {
                    PreguntasSeguridad frmPreguntas = new PreguntasSeguridad(this);
                    frmPreguntas.DataFill = res;
                    bool Resp = frmPreguntas.ShowDialog() ?? false;
                }
                else
                {
                    MessageBox.Show("El usuario que ha escrito en el login no existe", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
        }
    }
}
