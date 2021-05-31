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

    /// <summary>
    /// Interaction logic for DepartamentoFrm.xaml
    /// </summary>
    public partial class UsuarioFrm : Window
    {
        public UsuarioFrm()
        {
            InitializeComponent();

            MRol METR = new MRol();

            var res = METR.Mostrar();

            CbRol.ItemsSource = res;
            CbRol.DisplayMemberPath = "nombre";
            CbRol.SelectedValuePath = "idRol";
        }


        public TypeForm Type;
        public DUsuario DataFill;
        public List<DSeguridad> ListaSeguridad = new List<DSeguridad>();



        public DUsuario UForm;

        public MUsuario Metodos = new MUsuario();

        public bool IsSelf = false;

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
                fillForm(DataFill, ListaSeguridad);
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
                fillForm(DataFill, ListaSeguridad);
            }
        }



        void fillData()
        {
            if (Validate())
            {
                UForm = null;
                return;
            }

            int idRol = (int)CbRol.SelectedValue;
            string usuario = txtUsuario.txt.Text;
            string password = txtContraseña.Password;

            UForm = new DUsuario(0, 
                                 idRol,
                                 usuario,
                                 password, 
                                 0);
            AgregarSeguridad();
        }

        public void AgregarSeguridad()
        {
            ListaSeguridad.Clear();

            DSeguridad DS = new DSeguridad();
            DSeguridad DS2 = new DSeguridad();
            DSeguridad DS3 = new DSeguridad();

            DS.pregunta = txtPregunta1.txt.Text;
            DS.respuesta = txtRespuesta1.txt.Text;
            ListaSeguridad.Add(DS);

            DS2.pregunta = txtPregunta2.txt.Text;
            DS2.respuesta = txtRespuesta2.txt.Text;
            ListaSeguridad.Add(DS2);

            DS3.pregunta = txtPregunta3.txt.Text;
            DS3.respuesta = txtRespuesta3.txt.Text;
            ListaSeguridad.Add(DS3);

        }

        void Create()
        {
            fillData();
            if (UForm == null && ListaSeguridad.Count > 0)
                return;
            string response = Metodos.Insertar(UForm, ListaSeguridad); //COLOCAR LOS CAMPOS DE PREGUNTAS
            MessageBox.Show(response);
            if (response == "OK")
            {
                MAuditoria.Insertar(new DAuditoria(
                                    Menu.ActUsuario.idUsuario,
                                    DAuditoria.Registrar,
                                    "Se ha registrado un Usuario " + UForm.usuario));

                this.DialogResult = true;
                this.Close();
            }

        }

        void Update()
        {
            fillData();
            if (UForm == null && ListaSeguridad.Count > 0)
                return;
            UForm.idUsuario = DataFill.idUsuario;
            string response = Metodos.Editar(UForm, ListaSeguridad); //COLOCAR LOS CAMPOS DE PREGUNTAS
            MessageBox.Show(response);
            if (response == "OK")
            {
                MAuditoria.Insertar(new DAuditoria(
                                    Menu.ActUsuario.idUsuario,
                                    DAuditoria.Editar,
                                    "Se ha Editado el Usuario Nº" + UForm.idUsuario));

                this.DialogResult = true;
                this.Close();
            }
        }

        void SetEnable(bool Enable)
        {
            txtUsuario.IsEnabled = Enable;
            CbRol.IsEnabled = Enable;
            txtContraseña.IsEnabled = Enable;
            txtPregunta1.IsEnabled = Enable;
            txtRespuesta1.IsEnabled = Enable;
            txtPregunta2.IsEnabled = Enable;
            txtRespuesta2.IsEnabled = Enable;
            txtPregunta3.IsEnabled = Enable;
            txtRespuesta3.IsEnabled = Enable;
        }
        void fillForm(DUsuario Data, List<DSeguridad> DataSeguridad)
        {
            if (Data != null)
            {
                txtUsuario.SetText(Data.usuario);
                CbRol.SelectedValue = Data.idRol;
                if (Type == TypeForm.Read)
                    grdContraseña.Visibility = Visibility.Collapsed;
                else if(Type == TypeForm.Update)
                {
                    txtContraseña.Password = Data.contraseña;
                    PlaceContraseña.Text = "";
                }
                if (IsSelf)
                    CbRol.IsEnabled = false;

                txtPregunta1.SetText(DataSeguridad[0].pregunta);
                txtRespuesta1.SetText(DataSeguridad[0].respuesta);

                txtPregunta2.SetText(DataSeguridad[1].pregunta);
                txtRespuesta2.SetText(DataSeguridad[1].respuesta);

                txtPregunta3.SetText(DataSeguridad[2].pregunta);
                txtRespuesta3.SetText(DataSeguridad[2].respuesta);
            }
        }
        #region Validation
        bool Validate()
        {
            if (txtUsuario.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el campo Usuario!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                txtUsuario.txt.Focus();
                return true;
            }

            if (CbRol.SelectedIndex == -1)
            {
                MessageBox.Show("Debes seleccionar un Rol!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                CbRol.Focus();
                return true;
            }

            if (txtContraseña.Password == "")
            {
                MessageBox.Show("Debes llenar el campo Contraseña!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                txtContraseña.Focus();
                return true;
            }

            if (txtPregunta1.txt.Text == "")
            {
                MessageBox.Show("Debes llenar la pregunta #1!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                txtPregunta1.txt.Focus();
                return true;
            }
            if (txtRespuesta1.txt.Text == "")
            {
                MessageBox.Show("Debes llenar la respuesta #1!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                txtRespuesta1.txt.Focus();
                return true;
            }

            if (txtPregunta2.txt.Text == "")
            {
                MessageBox.Show("Debes llenar la pregunta #2!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                txtPregunta2.txt.Focus();
                return true;
            }
            if (txtRespuesta2.txt.Text == "")
            {
                MessageBox.Show("Debes llenar la respuesta #2!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                txtRespuesta2.txt.Focus();
                return true;
            }
            if (txtPregunta3.txt.Text == "")
            {
                MessageBox.Show("Debes llenar la pregunta #3!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                txtPregunta3.txt.Focus();
                return true;
            }
            if (txtRespuesta3.txt.Text == "")
            {
                MessageBox.Show("Debes llenar la respuesta #3!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                txtRespuesta3.txt.Focus();
                return true;
            }


            return false;
        }
        #endregion

        private void CbRol_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if(CbRol.SelectedIndex > -1)
            {
                PlaceRol.Text = "";
                var rol = CbRol.SelectedItem as DRol;
                txtDescripción.Text = rol.descripcion;
            }
            else
            {
                PlaceRol.Text = "Rol";
                txtDescripción.Text = "";
            }
        }

        private void txtContraseña_GotFocus(object sender, RoutedEventArgs e)
        {
            if(txtContraseña.Password == "")
            {
                PlaceContraseña.Text = "";
            }
        }

        private void txtContraseña_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtContraseña.Password == "")
            {
                PlaceContraseña.Text = "Contraseña";
            }
        }
    }
}
