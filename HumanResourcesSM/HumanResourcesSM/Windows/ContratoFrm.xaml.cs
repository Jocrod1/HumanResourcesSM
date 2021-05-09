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
    public partial class ContratoFrm : Window
    {

        public ContratoFrm(DEmpleado empleado, DSeleccion seleccion)
        {
            InitializeComponent();

            txtSueldo.txt.KeyDown += new KeyEventHandler(Validaciones.TextBoxValidatePrices);
            txtHorasSemanales.KeyDown += new KeyEventHandler(Validaciones.TextBox_KeyDown);

            Empleado = empleado;
            Seleccion = seleccion;

        }

        public ContratoFrm(DContrato contrato)
        {
            InitializeComponent();

            txtSueldo.txt.KeyDown += new KeyEventHandler(Validaciones.TextBoxValidatePrices);
            txtHorasSemanales.KeyDown += new KeyEventHandler(Validaciones.TextBox_KeyDown);

            Type = TypeForm.Update;

            DataFill = contrato;
        }


        public TypeForm Type;
        public DContrato DataFill;

        public DContrato UForm;

        public MContrato Metodos = new MContrato();

        public DEmpleado Empleado;
        public DSeleccion Seleccion;


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(Type == TypeForm.Update)
            {
                Update();
            }
            else
            {
                Create();
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(Type == TypeForm.Read)
            {
                txtTitulo.Text = "Ver Contrato";
                fillForm(DataFill);
                SetEnable(false);
                btnEnviar.Visibility = Visibility.Collapsed;
            }
            else if(Type == TypeForm.Update)
            {
                txtTitulo.Text = "Editar Contrato";
                BgTitulo.Background = (Brush)new BrushConverter().ConvertFrom("#2A347B");
                btnEnviar.Content = "Editar";
                btnEnviar.Foreground = (Brush)new BrushConverter().ConvertFrom("#2A347B");
                btnEnviar.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#2A347B");
                fillForm(DataFill);
            }
        }

        public string RegistrarContrato(DContrato contrato)
        {
            DContrato Data = new DContrato(0,
                                           Empleado.idEmpleado,
                                           DateTime.Now,
                                           Seleccion.nombrePuesto,
                                           contrato.sueldo,
                                           contrato.horasSemanales);

            var resp = new MContrato().Insertar(Data);

            return resp;
        }

        public string ActualizarContrato(DContrato contrato)
        {
            DContrato Data = DataFill;
            Data.sueldo = contrato.sueldo;
            Data.horasSemanales = contrato.horasSemanales;

            var resp = new MContrato().Editar(Data);

            return resp;
        }

        void fillData()
        {
            if (Validate())
            {
                UForm = null;
                return;
            }

            double Sueldo = double.Parse(txtSueldo.txt.Text);
            int HorasSemanales = int.Parse(txtHorasSemanales.txt.Text);
            //DateTime FechaCulminacion = CbFechaCulminacion.SelectedDate ?? DateTime.Now.AddYears(1);

            UForm = new DContrato(0,
                                  0,
                                  DateTime.Now,
                                  "",
                                  Sueldo,
                                  HorasSemanales);
        }

        void Create()
        {
            fillData();
            if (UForm == null)
                return;

            string res = RegistrarContrato(UForm);

            if (!res.Equals("OK"))
                MessageBox.Show(res);

            this.DialogResult = res.Equals("OK");
            this.Close();
        }

        void Update()
        {
            fillData();
            if (UForm == null)
                return;

            string res = ActualizarContrato(UForm);

            if (!res.Equals("OK"))
                MessageBox.Show(res);

            this.DialogResult = res.Equals("OK");
            this.Close();
        }

        void SetEnable(bool Enable)
        {
            txtSueldo.IsEnabled = Enable;
            txtHorasSemanales.IsEnabled = Enable;
        }

        void fillForm(DContrato Data)
        {
            if(Data != null)
            {
                txtSueldo.SetText(Data.sueldo.ToString());
                txtHorasSemanales.SetText(Data.horasSemanales.ToString());
            }
        }

        //private void CbFechaCulminacion_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (CbFechaCulminacion.SelectedDate != null)
        //    {
        //        PlaceFechaCulminacion.Text = "";
        //    }
        //    else
        //    {
        //        PlaceFechaCulminacion.Text = "Culminación del Contrato";
        //    }
        //}


        #region Validation
        bool Validate()
        {
            if (txtSueldo.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el Campo Sueldo!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtSueldo.txt.Focus();
                return true;
            }
            if (txtHorasSemanales.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el Campo de Horas Semanales!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtHorasSemanales.txt.Focus();
                return true;
            }
            //if (CbFechaCulminacion.SelectedDate == null)
            //{
            //    MessageBox.Show("Debes Seleccionar una Fecha de Culminación de Contrato!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
            //    CbFechaCulminacion.Focus();
            //    return true;
            //}

            return false;
        }
        #endregion

       
    }
}
