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
    public partial class TipoMetricaFrm : Window
    {
        public TipoMetricaFrm()
        {
            InitializeComponent();

            var res = new MDepartamento().Mostrar("");

            CbDepartamento.ItemsSource = res;
            CbDepartamento.DisplayMemberPath = "nombre";
            CbDepartamento.SelectedValuePath = "idDepartamento";

        }


        public TypeForm Type;
        public DTipoMetrica DataFill;

        public DTipoMetrica UForm;

        public MTipoMetrica Metodos = new MTipoMetrica();

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

            string nombre = txtNombre.Text;
            int idDepartamento = (int)CbDepartamento.SelectedValue;


            UForm = new DTipoMetrica(0, nombre, idDepartamento);
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
                                    "Se ha registrado un Tipo Metrica para el departamento Nº" + UForm.idDepartamento));

                this.DialogResult = true;
                this.Close();
            }

        }

        void Update()
        {
            fillData();
            if (UForm == null)
                return;
            UForm.idTipoMetrica = DataFill.idTipoMetrica;
            string response = Metodos.Editar(UForm);
            MessageBox.Show(response);
            if (response == "OK")
            {
                MAuditoria.Insertar(new DAuditoria(
                                    Menu.ActUsuario.idUsuario,
                                    DAuditoria.Editar,
                                    "Se ha Editado el Tipo Metrica Nº" + UForm.idTipoMetrica));

                this.DialogResult = true;
                this.Close();
            }
        }

        void SetEnable(bool Enable)
        {
            txtNombre.IsEnabled = Enable;
            CbDepartamento.IsEnabled = Enable;
        }
        void fillForm(DTipoMetrica Data)
        {
            if (Data != null)
            {
                CbDepartamento.SelectedValue = Data.idDepartamento;
                txtNombre.Text = Data.nombre;
            }
        }
        #region Validation
        bool Validate()
        {
            if (txtNombre.Text == "")
            {
                MessageBox.Show("Debes llenar el campo Nombre de Tipo de Tramite!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtNombre.Focus();
                return true;
            }

            if (CbDepartamento.SelectedIndex == -1)
            {
                MessageBox.Show("Debes seleccionar un Departamento!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbDepartamento.Focus();
                return true;
            }
            

            return false;
        }
        #endregion
    }
}
