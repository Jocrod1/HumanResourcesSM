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

    public partial class DeudaFrm : Window
    {

        public DeudaFrm()
        {
            InitializeComponent();

            txtMonto.KeyDown += new KeyEventHandler(Validaciones.TextBoxValidatePrices);

            var res = new MSeleccion().MostrarEmpleado("");

            foreach (DEmpleado item in res)
            {
                item.nombre = item.nombre + " " + item.apellido;
            }

            CbEmpleado.ItemsSource = res;
            CbEmpleado.DisplayMemberPath = "nombre";
            CbEmpleado.SelectedValuePath = "idEmpleado";

        }


        public TypeForm Type;
        public DDeuda DataFill;

        public DDeuda UForm;

        public MDeuda Metodos = new MDeuda();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
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
        }

        void fillData()
        {
            if (Validate())
            {
                UForm = null;
                return;
            }

            int idEmpleado = (int)CbEmpleado.SelectedValue;
            double Monto = double.Parse(txtMonto.Text);
            int TipoDeuda = CbTipoDeuda.SelectedIndex;
            string Concepto = txtConcepto.Text;

            int Repetitivo = ChBFijo.IsChecked == true ? 1 : 0; 
            int TipoPago = CbTipoPago.SelectedIndex;


            UForm = new DDeuda(0,
                               idEmpleado,
                               Monto,
                               Concepto,
                               TipoDeuda,
                               Repetitivo,
                               TipoPago,
                               1);
        }

        void Create()
        {
            fillData();
            if (UForm == null)
                return;
            List<DDeuda> deudas = new List<DDeuda>();
            deudas.Add(UForm);
            string response = Metodos.Insertar(deudas);
            if (response == "OK")
            {
                MAuditoria.Insertar(new DAuditoria(
                                    Menu.ActUsuario.idUsuario,
                                    DAuditoria.Registrar,
                                    "Se ha registrado una " + UForm.tipoDeudaStringz + " " + UForm.concepto));

                MessageBox.Show("Registro completado!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }

        }

        void SetEnable(bool Enable)
        {
            CbEmpleado.IsEnabled = Enable;
            txtMonto.IsEnabled = Enable;
            CbTipoDeuda.IsEnabled = Enable;
            txtConcepto.IsEnabled = Enable;
            CbTipoPago.IsEnabled = Enable;
            ChBFijo.IsEnabled = Enable;
        }
        void fillForm(DDeuda Data)
        {
            if (Data != null)
            {
                CbEmpleado.SelectedValue = Data.idEmpleado;
                txtMonto.Text = Data.monto.ToString();
                CbTipoDeuda.SelectedIndex = Data.tipoDeuda;
                txtConcepto.Text = Data.concepto;
                CbTipoPago.SelectedIndex = Data.tipoPago;
                ChBFijo.IsChecked = Data.repetitivo == 1;
            }
        }

        #region Validation
        bool Validate()
        {
            if (txtConcepto.Text == "")
            {
                MessageBox.Show("Debes llenar el campo Nombre!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtConcepto.Focus();
                return true;
            }
            if (CbEmpleado.SelectedIndex == -1)
            {
                MessageBox.Show("Debes seleccionar un Empleado!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbEmpleado.Focus();
                return true;
            }
            if (CbTipoDeuda.SelectedIndex == -1)
            {
                MessageBox.Show("Debes seleccionar si es Boficicación o Deducción!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbTipoDeuda.Focus();
                return true;
            }
            if (CbTipoPago.SelectedIndex == -1)
            {
                MessageBox.Show("Debes seleccionar un Tipo Pago!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbTipoDeuda.Focus();
                return true;
            }
            
            if (txtMonto.Text == "")
            {
                MessageBox.Show("Debes llenar el campo Monto!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtMonto.Focus();
                return true;
            }



            return false;
        }

        #endregion

        
    }
}
