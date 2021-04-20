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
    public partial class DeudaFrm : Window
    {
        public DeudaFrm()
        {
            InitializeComponent();

            txtMonto.txt.KeyDown += new KeyEventHandler(Validaciones.TextBoxValidatePrices);


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
                PanelPagado.Visibility = Visibility.Visible;
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
            double Monto = double.Parse(txtMonto.txt.Text);
            int TipoDeuda = CbTipoDeuda.SelectedIndex;
            string Concepto = txtConcepto.txt.Text;


            UForm = new DDeuda(0,
                               idEmpleado,
                               Monto,
                               0,
                               Concepto,
                               TipoDeuda,
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
            MessageBox.Show(response);
            if (response == "OK")
            {
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
        }
        void fillForm(DDeuda Data)
        {
            if (Data != null)
            {
                CbEmpleado.SelectedValue = Data.idEmpleado;
                txtMonto.SetText(Data.monto.ToString());
                txtPagado.Text = Data.pagado.ToString();
                txtRestante.Text = (Data.monto - Data.pagado).ToString();
                CbTipoDeuda.SelectedIndex = Data.tipoDeuda;
                txtConcepto.SetText(Data.concepto);
            }
        }

        private void CbDepartamento_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbEmpleado.SelectedIndex > -1)
            {
                PlaceEmpleado.Text = "";
            }
            else
            {
                PlaceEmpleado.Text = "Empleado";
            }
        }

        private void CbTipoDeuda_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbTipoDeuda.SelectedIndex > -1)
            {
                PlaceTipoDeuda.Text = "";
            }
            else
            {
                PlaceTipoDeuda.Text = "Tipo";
            }
        }

        #region Validation
        bool Validate()
        {
            if (CbEmpleado.SelectedIndex == -1)
            {
                MessageBox.Show("Debes seleccionar un Empleado!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                CbEmpleado.Focus();
                return true;
            }
            if (txtMonto.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el campo Monto Inicial!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                txtMonto.txt.Focus();
                return true;
            }
            if (CbTipoDeuda.SelectedIndex == -1)
            {
                MessageBox.Show("Debes seleccionar un Tipo!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                CbTipoDeuda.Focus();
                return true;
            }
            if (txtConcepto.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el campo Concepto!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                txtConcepto.txt.Focus();
                return true;
            }




            return false;
        }
        #endregion

       
    }
}
