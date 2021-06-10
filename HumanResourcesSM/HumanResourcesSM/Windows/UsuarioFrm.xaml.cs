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
            string usuario = txtUsuario.Text;
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

            DS.pregunta = txtPregunta1.Text;
            DS.respuesta = txtRespuesta1.Text;
            ListaSeguridad.Add(DS);

            DS2.pregunta = txtPregunta2.Text;
            DS2.respuesta = txtRespuesta2.Text;
            ListaSeguridad.Add(DS2);

            DS3.pregunta = txtPregunta3.Text;
            DS3.respuesta = txtRespuesta3.Text;
            ListaSeguridad.Add(DS3);

        }

        void Create()
        {
            fillData();
            if (UForm == null && ListaSeguridad.Count > 0)
                return;
            string response = Metodos.Insertar(UForm, ListaSeguridad); //COLOCAR LOS CAMPOS DE PREGUNTAS
            if (response == "OK")
            {
                MAuditoria.Insertar(new DAuditoria(
                                    Menu.ActUsuario.idUsuario,
                                    DAuditoria.Registrar,
                                    "Se ha registrado un Usuario " + UForm.usuario));
                MessageBox.Show("Registro completado!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
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
            if (response == "OK")
            {
                MAuditoria.Insertar(new DAuditoria(
                                    Menu.ActUsuario.idUsuario,
                                    DAuditoria.Editar,
                                    "Se ha Editado el Usuario Nº" + UForm.idUsuario));
                MessageBox.Show("Edición completada!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
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
                txtUsuario.Text = Data.usuario;
                CbRol.SelectedValue = Data.idRol;
                if (Type == TypeForm.Read)
                    grdContraseña.Visibility = Visibility.Collapsed;
                else if(Type == TypeForm.Update)
                {
                    txtContraseña.Password = Data.contraseña;
                }
                if (IsSelf)
                    CbRol.IsEnabled = false;

                txtPregunta1.Text = DataSeguridad[0].pregunta;
                txtRespuesta1.Text = DataSeguridad[0].respuesta;

                txtPregunta2.Text = DataSeguridad[1].pregunta;
                txtRespuesta2.Text = DataSeguridad[1].respuesta;

                txtPregunta3.Text = DataSeguridad[2].pregunta;
                txtRespuesta3.Text = DataSeguridad[2].respuesta;
            }
        }
        #region Validation
        bool Validate()
        {
            if (txtUsuario.Text == "")
            {
                MessageBox.Show("Debes llenar el campo Usuario!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                txtUsuario.Focus();
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

            if (txtPregunta1.Text == "")
            {
                MessageBox.Show("Debes llenar la pregunta #1!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                txtPregunta1.Focus();
                return true;
            }
            if (txtRespuesta1.Text == "")
            {
                MessageBox.Show("Debes llenar la respuesta #1!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                txtRespuesta1.Focus();
                return true;
            }

            if (txtPregunta2.Text == "")
            {
                MessageBox.Show("Debes llenar la pregunta #2!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                txtPregunta2.Focus();
                return true;
            }
            if (txtRespuesta2.Text == "")
            {
                MessageBox.Show("Debes llenar la respuesta #2!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                txtRespuesta2.Focus();
                return true;
            }
            if (txtPregunta3.Text == "")
            {
                MessageBox.Show("Debes llenar la pregunta #3!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                txtPregunta3.Focus();
                return true;
            }
            if (txtRespuesta3.Text == "")
            {
                MessageBox.Show("Debes llenar la respuesta #3!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                txtRespuesta3.Focus();
                return true;
            }
            var UserCheck = Metodos.EncontrarByUsuario(txtUsuario.Text);
            if (UserCheck.Count > 0)
            {
                var user = UserCheck[0];
                if (user.estado == 1)
                {
                    MessageBox.Show("El usuario ingresado ya está registrado!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtUsuario.Focus();
                    return true;
                }
                else if(Type == TypeForm.Update)
                {
                    if(txtUsuario.Text != DataFill.usuario)
                    {
                        MessageBox.Show("El usuario ingresado ya está registrado!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                        txtUsuario.Focus();
                        return true;
                    }
                }
                else if (Type == TypeForm.Create && user.estado == 0)
                {
                    var resp = MessageBox.Show("El usuario ingresado ya está registrado pero está anulado. ¿Desea Reactivarlo?!", "Magicolor", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (resp == MessageBoxResult.Yes)
                    {
                        int idRol = (int)CbRol.SelectedValue;
                        string usuario = txtUsuario.Text;
                        string password = txtContraseña.Password;

                        UForm = new DUsuario(user.idUsuario,
                                             idRol,
                                             usuario,
                                             password,
                                             0);
                        AgregarSeguridad();

                        string response = Metodos.Editar(UForm, ListaSeguridad);
                        if (response == "OK")
                        {
                            MAuditoria.Insertar(new DAuditoria(
                                                Menu.ActUsuario.idUsuario,
                                                DAuditoria.Editar,
                                                "Se ha Reactivado el Usuario Nº" + UForm.idUsuario));
                            MessageBox.Show("Reactivación completada!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.DialogResult = true;
                            this.Close();
                            return true;
                        }
                        else
                        {
                            MessageBox.Show(response, "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    }
                }
            }


            return false;
        }
        #endregion


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


        private void CbRol_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if(CbRol.SelectedIndex > -1)
            {
                var rol = CbRol.SelectedItem as DRol;
                txtDescripción.Text = rol.descripcion;
            }
            else
            {
                txtDescripción.Text = "";
            }
        }
    }
}
