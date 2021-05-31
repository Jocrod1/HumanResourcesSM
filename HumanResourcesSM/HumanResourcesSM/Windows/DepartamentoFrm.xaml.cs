using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
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
    public enum TypeForm
    {
        Create,
        Read,
        Update,
        Delete
    }

    public class Validaciones
    {
        #region CharConverter
        public enum MapType : uint
        {
            MAPVK_VK_TO_VSC = 0x0,
            MAPVK_VSC_TO_VK = 0x1,
            MAPVK_VK_TO_CHAR = 0x2,
            MAPVK_VSC_TO_VK_EX = 0x3,
        }

        [DllImport("user32.dll")]
        public static extern int ToUnicode(
         uint wVirtKey,
         uint wScanCode,
         byte[] lpKeyState,
         [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)]
            StringBuilder pwszBuff,
         int cchBuff,
         uint wFlags);
        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, MapType uMapType);

        public static char GetCharFromKey(Key key)
        {
            char ch = ' ';

            int virtualKey = KeyInterop.VirtualKeyFromKey(key);
            byte[] keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            uint scanCode = MapVirtualKey((uint)virtualKey, MapType.MAPVK_VK_TO_VSC);
            StringBuilder stringBuilder = new StringBuilder(2);

            int result = ToUnicode((uint)virtualKey, scanCode, keyboardState, stringBuilder, stringBuilder.Capacity, 0);
            switch (result)
            {
                case -1:
                    break;
                case 0:
                    break;
                case 1:
                    {
                        ch = stringBuilder[0];
                        break;
                    }
                default:
                    {
                        ch = stringBuilder[0];
                        break;
                    }
            }
            return ch;
        }
        #endregion
        public static bool IsValidEmail(string email)
        {
            var Foo = new EmailAddressAttribute();
            bool Valid = Foo.IsValid(email);
            return Valid;
        }

        public static bool ValidHttpURL(string s)
        {
            Uri resultURI;

            if (!Regex.IsMatch(s, @"^https?:\/\/", RegexOptions.IgnoreCase))
                s = "http://" + s;

            if (Uri.TryCreate(s, UriKind.Absolute, out resultURI))
                return (resultURI.Scheme == Uri.UriSchemeHttp ||
                        resultURI.Scheme == Uri.UriSchemeHttps);

            return false;
        }

        public static void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                TextBox s = e.Source as TextBox;
                if (s != null)
                {
                    s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }

                e.Handled = true;
            }

            char ch = GetCharFromKey(e.Key);

            if (!Char.IsDigit(ch))
            {

                e.Handled = true;
            }
        }

        public static void TextBoxValidatePrices(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                TextBox s = e.Source as TextBox;
                if (s != null)
                {
                    s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }

                e.Handled = true;
            }

            char ch = GetCharFromKey(e.Key);

            TextBox txtBox = (TextBox)sender;

            if (ch == 46 && txtBox.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (Regex.IsMatch(txtBox.Text, @"\.\d\d"))
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 9 && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }
    }

    /// <summary>
    /// Interaction logic for DepartamentoFrm.xaml
    /// </summary>
    public partial class DepartamentoFrm : Window
    {
        public DepartamentoFrm()
        {
            InitializeComponent();
        }


        public TypeForm Type;
        public DDepartamento DataFill;

        public DDepartamento UForm;

        public MDepartamento Metodos = new MDepartamento();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Type == TypeForm.Update)
                Update();
            else
                Create();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Type == TypeForm.Read)
            {
                txtTitulo.Text = "Ver";
                fillForm(DataFill);
                SetEnable(false);
                btnEnviar.Visibility = Visibility.Collapsed;
            }
            else if (Type == TypeForm.Update)
            {
                txtTitulo.Text = "Editar";
                BgTitulo.Background = (Brush)new BrushConverter().ConvertFrom("#2A347B");
                btnEnviar.Content = "Editar";
                btnEnviar.Foreground = (Brush)new BrushConverter().ConvertFrom("#2A347B");
                btnEnviar.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#2A347B");
                fillForm(DataFill);
            }
        }



        void fillData()
        {
            if (Validate())
            {
                UForm = null;
                return;
            }

            string nombre = txtNombre.txt.Text;
            string descripcion = txtDescripcion.Text;

            UForm = new DDepartamento(0, nombre, descripcion);
        }

        void Create()
        {
            fillData();
            if (UForm == null)
                return;
            string response = Metodos.Insertar(UForm);
            MessageBox.Show(response);
            if (response == "OK")
            {

                MAuditoria.Insertar(new DAuditoria(
                                    Menu.ActUsuario.idUsuario,
                                    DAuditoria.Registrar,
                                    "Se ha registrado el departamento " + UForm.nombre));


                MessageBox.Show("Registro completado!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }

        }

        void Update()
        {
            fillData();
            if (UForm == null)
                return;
            UForm.idDepartamento = DataFill.idDepartamento;
            string response = Metodos.Editar(UForm);
            MessageBox.Show(response);
            if (response == "OK")
            {
                MAuditoria.Insertar(new DAuditoria(
                                    Menu.ActUsuario.idUsuario,
                                    DAuditoria.Editar,
                                    "Se ha Editado el departamento " + UForm.nombre));


                MessageBox.Show("Edición completada!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
        }

        private void PlaceDescripcion_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtDescripcion.Text == "")
            {
                PlaceDescripcion.Text = "";
            }
        }

        private void PlaceDescripcion_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtDescripcion.Text == "")
            {
                PlaceDescripcion.Text = "Descripción";
            }
        }

        void SetEnable(bool Enable)
        {
            txtNombre.IsEnabled = Enable;
            txtDescripcion.IsEnabled = Enable;
        }
        void fillForm(DDepartamento Data)
        {
            if (Data != null)
            {
                txtNombre.SetText(Data.nombre);
                txtDescripcion.Text = Data.descripcion;
                PlaceDescripcion.Text = "";
            }
        }
        #region Validation
        bool Validate()
        {
            if (txtNombre.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el campo Nombre!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtNombre.txt.Focus();
                return true;
            }

            return false;
        }
        #endregion
    }
}
