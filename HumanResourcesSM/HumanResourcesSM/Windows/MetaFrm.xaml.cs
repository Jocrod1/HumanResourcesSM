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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;

using Datos;
using Metodos;

namespace HumanResourcesSM.Windows
{
    public partial class MetaFrm : Window
    {

        void init()
        {
            InitializeComponent();
            txtValorMeta.KeyDown += new KeyEventHandler(Validaciones.TextBoxValidatePrices);
        }
        public MetaFrm()
        {
            init();
            Type = TypeForm.Create;
        }
        public MetaFrm(TypeForm type, MetaType mtype, DMeta meta)
        {
            init();

            Type = type;
            MType = mtype;
            DataFill = meta;

        }

        public TypeForm Type;
        public MetaType MType;
        public DMeta DataFill;

        public DMeta UForm;

        public MMeta Metodos = new MMeta();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Type == TypeForm.Update)
                Update();
            else
                Create();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(Type == TypeForm.Read)
            {
                txtTitulo.Text = "Ver";
                fillForm(DataFill);
                SetEnable(false);
                btnEnviar.Visibility = Visibility.Collapsed;
            }
            else if(Type == TypeForm.Update)
            {
                txtTitulo.Text = "Editar";
                BgTitulo.Background = (Brush)new BrushConverter().ConvertFrom("#2A347B");
                btnEnviar.Content = "Editar";
                btnEnviar.Foreground = (Brush)new BrushConverter().ConvertFrom("#2A347B");
                btnEnviar.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#2A347B");
                fillForm(DataFill);
            }
            else if(Type == TypeForm.Create)
            {
                RBEmpleado.IsChecked = true;
            }

        }

        void fillData()
        {
            if (Validate())
            {
                UForm = null;
                return;
            }


            int idSeleccion = (int)CbSeleccion.SelectedValue;

            int idEmpleado = MType.Equals(MetaType.Empleado) ? idSeleccion : 1;
            int idDepartamento = MType.Equals(MetaType.Departamento) ? idSeleccion : 1;
            int idTipoMetrica = (int)CbTipoMetrica.SelectedValue;
            int ValorMeta = int.Parse(txtValorMeta.Text);
            DateTime FechaInicio = CbFechaInicio.SelectedDate ?? DateTime.Now;
            DateTime FechaFinal = CbFechaFinal.SelectedDate ?? DateTime.Now;


            UForm = new DMeta(0,
                              idTipoMetrica,
                              ValorMeta,
                              idEmpleado,
                              idDepartamento,
                              1,
                              FechaInicio,
                              FechaFinal,
                              Menu.ActUsuario.idUsuario);
        }

       

        void Create()
        {
            fillData();
            if (UForm == null)
                return;
            var resp = Metodos.Insertar(UForm);
            if (resp == "OK")
            {
                MAuditoria.Insertar(new DAuditoria(
                                    Menu.ActUsuario.idUsuario,
                                    DAuditoria.Registrar,
                                    "Se ha registrado una Meta para el empleado Nº" + UForm.idEmpleado));

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
            UForm.idMeta = DataFill.idMeta;
            var resp = Metodos.Editar(UForm);
            if (resp == "OK")
            {
                MAuditoria.Insertar(new DAuditoria(
                                    Menu.ActUsuario.idUsuario,
                                    DAuditoria.Editar,
                                    "Se ha Editado la Meta Nº" + UForm.idMeta));

                MessageBox.Show("Edición completada!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
        }

        void SetEnable(bool Enable)
        {
            RBDepartamento.IsEnabled = Enable;
            RBEmpleado.IsEnabled = Enable;
            CbSeleccion.IsEnabled = Enable;
            CbTipoMetrica.IsEnabled = Enable;
            txtValorMeta.IsEnabled = Enable;
            CbFechaInicio.IsEnabled = Enable;
            CbFechaFinal.IsEnabled = Enable;
        }

        void fillForm(DMeta Data)
        {
            if (Data != null)
            {
                if(MType == MetaType.Departamento)
                {
                    RBDepartamento.IsChecked = true;
                    CbSeleccion.SelectedValue = Data.idDepartamento;
                }
                else if(MType == MetaType.Empleado)
                {
                    RBEmpleado.IsChecked = true;
                    CbSeleccion.SelectedValue = Data.idEmpleado;
                }
                RBDepartamento.IsEnabled = false;
                RBEmpleado.IsEnabled = false;
                CbTipoMetrica.SelectedValue = Data.idTipoMetrica;
                txtValorMeta.Text = Data.valorMeta.ToString();
                CbFechaInicio.SelectedDate = Data.fechaInicio;
                CbFechaFinal.SelectedDate = Data.fechaFinal;
            }
        }

        private void RBEmpleado_Checked(object sender, RoutedEventArgs e)
        {
            MType = MetaType.Empleado;

            MSeleccion MSel = new MSeleccion();
            var res = MSel.MostrarEmpleado("");

            foreach (DEmpleado item in res)
            {
                item.nombre = item.nombre + " " + item.apellido;
            }

            CbSeleccion.ItemsSource = res;
            CbSeleccion.DisplayMemberPath = "nombre";
            CbSeleccion.SelectedValuePath = "idEmpleado";

            CbSeleccion.SelectedIndex = -1;
        }

        private void RBDepartamento_Checked(object sender, RoutedEventArgs e)
        {
            MType = MetaType.Departamento;

            var res = new MDepartamento().Mostrar("");

            CbSeleccion.ItemsSource = res;
            CbSeleccion.DisplayMemberPath = "nombre";
            CbSeleccion.SelectedValuePath = "idDepartamento";

            CbSeleccion.SelectedIndex = -1;
        }

        private void CbSeleccion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbSeleccion.SelectedIndex > -1)
            {
                int id = 0;
                if (MType == MetaType.Empleado)
                {
                    var Empleado = (DEmpleado)CbSeleccion.SelectedItem;
                    id = Empleado.idDepartamento;
                }
                else if(MType == MetaType.Departamento)
                {
                    id = (int)CbSeleccion.SelectedValue;
                }

                var resp = new MTipoMetrica().Mostrar(id);

                CbTipoMetrica.ItemsSource = resp;
                CbTipoMetrica.DisplayMemberPath = "nombre";
                CbTipoMetrica.SelectedValuePath = "idTipoMetrica";

                CbTipoMetrica.IsEnabled = true;
            }
            else
            {
                CbTipoMetrica.SelectedIndex = -1;
                CbTipoMetrica.IsEnabled = false;
            }
        }

        #region Validation
        bool Validate()
        {
            if (CbSeleccion.SelectedIndex == -1)
            {
                string Val = RBEmpleado.IsChecked ?? false ? "Empleado" : RBDepartamento.IsChecked ?? false ? "Departamento" : "[VALOR NULO]";
                MessageBox.Show("Debes Seleccionar un " + Val + "!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbSeleccion.Focus();
                return true;
            }
            if (CbTipoMetrica.SelectedIndex == -1)
            {
                MessageBox.Show("Debes Seleccionar un tipo de Métrica!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbTipoMetrica.Focus();
                return true;
            }
            if (txtValorMeta.Text == "")
            {
                MessageBox.Show("Debes llenar el campo Valor Meta!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtValorMeta.Focus();
                return true;
            }
            if (CbFechaInicio.SelectedDate == null)
            {
                MessageBox.Show("Debes Seleccionar la Fecha de Inicio!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbFechaInicio.Focus();
                return true;
            }
            if (CbFechaFinal.SelectedDate == null)
            {
                MessageBox.Show("Debes Seleccionar la Fecha Final!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbFechaFinal.Focus();
                return true;
            }
            if (CbFechaInicio.SelectedDate?.Date > CbFechaFinal.SelectedDate?.Date)
            {
                MessageBox.Show("La Fecha de Inicio no puede ser después de la fecha Final!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbFechaInicio.Focus();
                return true;
            }

            return false;
        }


        #endregion

        private void CbFechaInicio_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbFechaInicio.SelectedDate != null)
            {
                CbFechaFinal.DisplayDateStart = CbFechaInicio.SelectedDate?.Date;
            }
            else
            {
                CbFechaFinal.DisplayDateStart = null;
            }
        }

        private void CbFechaFinal_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbFechaFinal.SelectedDate != null)
            {
                CbFechaInicio.DisplayDateEnd = CbFechaFinal.SelectedDate?.Date;
            }
            else
            {
                CbFechaInicio.DisplayDateEnd = null;
            }
        }

       

    }

    public enum MetaType
    {
        Empleado,
        Departamento
    }
}
