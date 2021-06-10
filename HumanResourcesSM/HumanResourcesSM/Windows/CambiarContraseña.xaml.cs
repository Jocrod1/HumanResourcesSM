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

    public partial class CambiarContraseña : Window
    {
        PreguntasSeguridad ParentForm;

        public CambiarContraseña(PreguntasSeguridad parentfrm)
        {
            InitializeComponent();

            ParentForm = parentfrm;
        }

        public DSeguridad DataFill;

        MUsuario Metodos = new MUsuario();

        private string usuario;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            usuario = DataFill.usuario;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Create();
        }

        private bool Validate()
        {
            if (txtContraseña.Password == "")
            {
                MessageBox.Show("Debe ingresar la contraseña!!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                txtContraseña.Focus();
                return true;
            }

            if (StrongPassword())
            {
                return true;
            }

            if (txtConfirmar.Password == "")
            {
                MessageBox.Show("Debe ingresar la confirmación de la contraseña!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                txtConfirmar.Focus();
                return true;
            }

            if (txtContraseña.Password != txtConfirmar.Password)
            {
                MessageBox.Show("Las contraseñas deben coincidir!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                txtConfirmar.Focus();
                return true;
            }

            return false;
        }

        private bool StrongPassword()
        {
            if (txtContraseña.Password.Length < 6)
            {
                MessageBox.Show("La contraseña no puede ser menor de 6 carácteres!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                txtContraseña.Focus();
                return true;
            }
            if (txtContraseña.Password.Length > 24)
            {
                MessageBox.Show("La contraseña no puede ser mayor de 24 carácteres!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                txtContraseña.Focus();
                return true;
            }
            if (!txtContraseña.Password.Any(char.IsUpper))
            {
                MessageBox.Show("La contraseña debe contener al menos una mayúscula!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                txtContraseña.Focus();
                return true;
            }
            if (!txtContraseña.Password.Any(char.IsLower))
            {
                MessageBox.Show("La contraseña debe contener al menos una minúscula!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                txtContraseña.Focus();
                return true;
            }
            if (txtContraseña.Password.Contains(" "))
            {
                MessageBox.Show("La contraseña no debe contener espacios en blanco!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                txtContraseña.Focus();
                return true;
            }
            if (SpecialCharacter())
            {
                MessageBox.Show("La contraseña debe contener al menos un carácter especial!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                txtContraseña.Focus();
                return true;
            }
            return false;
        }

        private bool SpecialCharacter()
        {
            string specialCh = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
            char[] specialChArray = specialCh.ToCharArray();
            foreach (char ch in specialChArray)
            {
                if (txtContraseña.Password.Contains(ch))
                    return true;
            }
            return false;
        }

        void Create()
        {
            if (Validate()) return;

            MessageBoxResult rpta;
            rpta = MessageBox.Show("¿Está seguro que desea implementar esta contraseña?", "Variedades Magicolor", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (rpta == MessageBoxResult.No)
                return;

            string respuesta = Metodos.EditarContraseña(usuario, txtContraseña.Password);

            if (respuesta.Equals("OK"))
            {
                MessageBox.Show("Contraseña cambiada de forma satisfactoria!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else
            {
                MessageBox.Show(respuesta, "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            this.Hide();
        }
    }
}
