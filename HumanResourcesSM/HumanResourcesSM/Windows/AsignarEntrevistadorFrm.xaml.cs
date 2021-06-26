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
    public partial class AsignarEntrevistadorFrm : Window
    {
        public AsignarEntrevistadorFrm(DEmpleado empleado)
        {
            InitializeComponent();

            var resp = new MUsuario().ListadoUsuarioEntrevistador();

            CbEntrevistador.ItemsSource = resp;
            CbEntrevistador.DisplayMemberPath = "usuario";
            CbEntrevistador.SelectedValuePath = "idUsuario";

            DpFechaEntrevista.DisplayDateStart = DateTime.Today;


            EmpleadoSeleccionado = empleado;
        }


        public TypeForm Type;
        public DSeleccion DataFill;

        public DEmpleado EmpleadoSeleccionado;

        public DSeleccion UForm;

        public MSeleccion Metodos = new MSeleccion();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //if (Type == TypeForm.Update)
            //    Update();
            //else
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

            string nombre = "";
            int idDepartamento = 1;


            UForm = new DSeleccion(0, 0, idDepartamento, 1, DateTime.Now, 1, DateTime.Now, "");
        }

        void Create()
        {
            fillData();
            if (UForm == null)
                return;
            string response = Metodos.EditarEmpleadoAlSeleccionar(EmpleadoSeleccionado.idEmpleado, (int)CbEntrevistador.SelectedValue, DpFechaEntrevista.SelectedDate ?? DateTime.Today);
            MessageBox.Show(response);
            if (response == "OK")
            {
                MAuditoria.Insertar(new DAuditoria(
                                    Menu.ActUsuario.idUsuario,
                                    DAuditoria.Registrar,
                                    "Se ha asignado una entrevista al postulado Nº" + EmpleadoSeleccionado.idEmpleado));

                this.DialogResult = true;
                this.Close();
            }

        }

        void SetEnable(bool Enable)
        {
            CbEntrevistador.IsEnabled = Enable;
            DpFechaEntrevista.IsEnabled = Enable;
        }
        void fillForm(DSeleccion Data)
        {
            if (Data != null)
            {
                CbEntrevistador.SelectedValue = Data.idEntrevistador;
                DpFechaEntrevista.SelectedDate = Data.fechaRevision;
            }
        }
        #region Validation
        bool Validate()
        {
            if (CbEntrevistador.SelectedIndex == -1)
            {
                MessageBox.Show("Debes Seleccionar un Entrevistador!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbEntrevistador.Focus();
                return true;
            }

            if (DpFechaEntrevista.SelectedDate == null)
            {
                MessageBox.Show("Debes Seleccionar una Fecha de Entrevista!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                DpFechaEntrevista.Focus();
                return true;
            }
            

            return false;
        }
        #endregion
    }
}
