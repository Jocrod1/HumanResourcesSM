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
    public partial class TipoTramiteFrm : Window
    {
        public TipoTramiteFrm()
        {
            InitializeComponent();
        }


        public TypeForm Type;
        public DTipoTramite DataFill;

        public DTipoTramite UForm;

        public MTipoTramite Metodos = new MTipoTramite();

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
            string StatusCambio = txtStatusCambio.txt.Text;
            string descripcion = txtDescripcion.txt.Text;

            UForm = new DTipoTramite(0, nombre, StatusCambio, descripcion);
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
                                    "Se ha registrado un Tipo Tramite " + UForm.nombre));


                this.DialogResult = true;
                this.Close();
            }

        }

        void Update()
        {
            fillData();
            if (UForm == null)
                return;
            UForm.idTipoTramite = DataFill.idTipoTramite;
            string response = Metodos.Editar(UForm);
            MessageBox.Show(response);
            if (response == "OK")
            {
                MAuditoria.Insertar(new DAuditoria(
                                    Menu.ActUsuario.idUsuario,
                                    DAuditoria.Editar,
                                    "Se ha Editado el Tipo Tramite Nº" + UForm.idTipoTramite));

                this.DialogResult = true;
                this.Close();
            }
        }

        void SetEnable(bool Enable)
        {
            txtNombre.IsEnabled = Enable;
            txtStatusCambio.IsEnabled = Enable;
        }
        void fillForm(DTipoTramite Data)
        {
            if (Data != null)
            {
                txtNombre.SetText(Data.nombre);
                txtStatusCambio.SetText(Data.statusCambio);
                txtStatusCambio.SetText(Data.descripcion);
            }
        }
        #region Validation
        bool Validate()
        {
            if (txtNombre.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el campo Nombre!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                txtNombre.txt.Focus();
                return true;
            }

            if (txtStatusCambio.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el campo Estatus!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                txtStatusCambio.txt.Focus();
                return true;
            }
            if (txtDescripcion.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el campo Descripción!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                txtStatusCambio.txt.Focus();
                return true;
            }

            return false;
        }
        #endregion
    }
}
