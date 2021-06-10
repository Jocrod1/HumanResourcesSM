using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Datos;
using Metodos;

namespace HumanResourcesSM.Windows
{

    public partial class RelacionesLaboralesFrm : Window
    {
        public RelacionesLaboralesFrm()
        {
            InitializeComponent();

            MSeleccion MSel = new MSeleccion();
            var res = MSel.MostrarEmpleado("");

            foreach(DEmpleado item in res)
            {
                item.nombre = item.nombre + " " + item.apellido;
            }

            CbEmpleado.ItemsSource = res;
            CbEmpleado.DisplayMemberPath = "nombre";
            CbEmpleado.SelectedValuePath = "idEmpleado";



            MTipoTramite MTTR = new MTipoTramite();
            var resp = MTTR.Mostrar("");

            CbTipoTramite.ItemsSource = resp;
            CbTipoTramite.DisplayMemberPath = "nombre";
            CbTipoTramite.SelectedValuePath = "idTipoTramite";
        }


        public TypeForm Type;
        public DRelacionesLaborales DataFill;

        public DRelacionesLaborales UForm;

        public MRelacionesLaborales Metodos = new MRelacionesLaborales();

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
            int idEmpleado = (int)CbEmpleado.SelectedValue;
            int idTipoTramite = (int)CbTipoTramite.SelectedValue;
            DateTime FechaTramite = DPFechaTramite.SelectedDate ?? DateTime.Now;
            string Url = txtUrl.Text;

            UForm = new DRelacionesLaborales(0,
                                             idEmpleado,
                                             idTipoTramite,
                                             FechaTramite,
                                             Url);
        }

        void Create()
        {
            fillData();
            if (UForm == null)
                return;
            string response = Metodos.Insertar(UForm);
            if (response == "OK")
            {
                MAuditoria.Insertar(new DAuditoria(
                                    Menu.ActUsuario.idUsuario,
                                    DAuditoria.Registrar,
                                    "Se ha registrado una Relación Laboral al empleado Nº" + UForm.idEmpleado));

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
            UForm.idTipoTramite = DataFill.idTipoTramite;
        }

        void SetEnable(bool Enable)
        {
            CbEmpleado.IsEnabled = Enable;
            CbTipoTramite.IsEnabled = Enable;
            DPFechaTramite.IsEnabled = Enable;
            txtUrl.IsEnabled = Enable;
        }
        void fillForm(DRelacionesLaborales Data)
        {
            if (Data != null)
            {
                CbEmpleado.SelectedValue = Data.idEmpleado;
                CbTipoTramite.SelectedValue = Data.idTipoTramite;
                txtUrl.Text = Data.documentoUrl;
            }
        }

        #region Validation
        bool Validate()
        {

            if (CbEmpleado.SelectedIndex == -1)
            {
                MessageBox.Show("Debes seleccionar un Empleado!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbEmpleado.Focus();
                return true;
            }

            if (CbTipoTramite.SelectedIndex == -1)
            {
                MessageBox.Show("Debes seleccionar un Rol!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbTipoTramite.Focus();
                return true;
            }

            if (DPFechaTramite.SelectedDate == null)
            {
                MessageBox.Show("Debes seleccionar una Fecha de Tramite!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                DPFechaTramite.Focus();
                return true;
            }

            if (txtUrl.Text == "")
            {
                MessageBox.Show("Debes llenar el campo URL!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtUrl.Focus();
                return true;
            }

            return false;
        }
        #endregion

    }
}
