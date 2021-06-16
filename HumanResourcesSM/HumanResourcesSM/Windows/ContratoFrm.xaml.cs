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

    public partial class ContratoFrm : Window
    {

        public ContratoFrm(DEmpleado empleado, DSeleccion seleccion, bool isContracted = true)
        {
            InitializeComponent();

            txtSueldo.KeyDown += new KeyEventHandler(Validaciones.TextBoxValidatePrices);
            txtHorasSemanales.KeyDown += new KeyEventHandler(Validaciones.TextBox_KeyDown);

            Empleado = empleado;
            Seleccion = seleccion;

            
            Contracted = isContracted;
            if (!Contracted)
            {
                StackContrato.Visibility = Visibility.Collapsed;
                txtTitulo.Text = "No Contratado";
                BgTitulo.Background = (Brush)new BrushConverter().ConvertFrom("#C22723");
                btnEnviar.Content = "Enviar";
                btnEnviar.Foreground = (Brush)new BrushConverter().ConvertFrom("#C22723");
                btnEnviar.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#C22723");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(txtRazon, "Razón de no Contratación");

            }
            else
            {
                cbFechaInicio.SelectedDate = DateTime.Today;
                cbFechaFinal.DisplayDateStart = DateTime.Today.AddDays(1);
                var resp = new MDepartamento().Encontrar(empleado.idDepartamento)[0].nombre;
                txtDepartamento.Text = "Departamento Asignado: " + resp;
            }
        }

        public ContratoFrm(DEmpleado empleado)
        {
            InitializeComponent();

            Empleado = empleado;
            Fired = true;
            StackContrato.Visibility = Visibility.Collapsed;
            txtTitulo.Text = "Despido";
            BgTitulo.Background = (Brush)new BrushConverter().ConvertFrom("#C22723");
            btnEnviar.Content = "Enviar";
            btnEnviar.Foreground = (Brush)new BrushConverter().ConvertFrom("#C22723");
            btnEnviar.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#C22723");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(txtRazon, "Razón de Despido");
        }

        public ContratoFrm(DContrato contrato)
        {
            InitializeComponent();

            txtSueldo.KeyDown += new KeyEventHandler(Validaciones.TextBoxValidatePrices);
            txtHorasSemanales.KeyDown += new KeyEventHandler(Validaciones.TextBox_KeyDown);

            Type = TypeForm.Update;

            DataFill = contrato;

            cbFechaInicio.SelectedDate = DateTime.Today;
            cbFechaFinal.DisplayDateStart = DateTime.Today.AddDays(1);
            txtDepartamento.Visibility = Visibility.Collapsed;
        }


        public TypeForm Type;
        public DContrato DataFill;

        public DContrato UForm;

        public MContrato Metodos = new MContrato();

        public DEmpleado Empleado;
        public DSeleccion Seleccion;
        public bool Contracted = true;
        public bool Fired = false;


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Contracted)
            {
                NoContratado();
                return;
            }
            if (Fired)
            {
                Despedido();
                return;
            }


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


        public void NoContratado()
        {
            var resp = new MContrato().NoContratado(Empleado.idEmpleado, Menu.ActUsuario.idUsuario, txtRazon.Text);

            if (resp.Equals("OK"))
            {
                MAuditoria.Insertar(new DAuditoria(
                                    Menu.ActUsuario.idUsuario,
                                    DAuditoria.Registrar,
                                    "Se ha registrado no contratado el empleado Nº" + Empleado.idEmpleado));

                MessageBox.Show("Accion Completada!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            else
                MessageBox.Show(resp);
        }
        public void Despedido()
        {
            var resp = new MSeleccion().Despido(Empleado.idEmpleado, txtRazon.Text);

            if (resp.Equals("OK"))
            {
                MAuditoria.Insertar(new DAuditoria(
                                    Menu.ActUsuario.idUsuario,
                                    DAuditoria.Eliminar,
                                    "Se ha Despedido al empleado Nº" + Empleado.idEmpleado));

                MessageBox.Show("Despido completado!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            else
                MessageBox.Show(resp);
        }
        public string RegistrarContrato(DContrato contrato)
        {
            DContrato Data = new DContrato(0,
                                           Empleado.idEmpleado,
                                           DateTime.Now,
                                           Seleccion.nombrePuesto,
                                           contrato.sueldo,
                                           contrato.horasSemanales,
                                           0,
                                           0);

            var resp = new MContrato().Insertar(Data, Menu.ActUsuario.idUsuario, txtRazon.Text);

            if (resp.Equals("OK"))
                MAuditoria.Insertar(new DAuditoria(
                                    Menu.ActUsuario.idUsuario, 
                                    DAuditoria.Registrar, 
                                    "Se ha registrado el contrato del empleado Nº" + Empleado.idEmpleado));

            return resp;
        }

        public string ActualizarContrato(DContrato contrato)
        {
            DContrato Data = DataFill;
            Data.sueldo = contrato.sueldo;
            Data.horasSemanales = contrato.horasSemanales;

            var resp = new MContrato().Editar(Data);

            if (resp.Equals("OK"))
                MAuditoria.Insertar(new DAuditoria(
                                    Menu.ActUsuario.idUsuario,
                                    DAuditoria.Editar,
                                    "Se ha Editado el contrato del empleado Nº" + Empleado.idEmpleado));

            return resp;
        }

        void fillData()
        {
            if (Validate())
            {
                UForm = null;
                return;
            }

            double Sueldo = double.Parse(txtSueldo.Text);
            int HorasSemanales = int.Parse(txtHorasSemanales.Text);
            DateTime FechaCulminacion = cbFechaFinal.SelectedDate ?? DateTime.Today;



            UForm = new DContrato(0,
                                  0,
                                  DateTime.Now,
                                  "",
                                  Sueldo,
                                  HorasSemanales,
                                  0,
                                  0);
            UForm.fechaCulminacion = FechaCulminacion;
        }

        void Create()
        {
            fillData();
            if (UForm == null)
                return;

            string res = RegistrarContrato(UForm);

            if (res.Equals("OK"))
                MessageBox.Show("Registro completado!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
            else
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

            if (res.Equals("OK"))
                MessageBox.Show("Edicion completada!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
            else
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
                txtSueldo.Text = Data.sueldo.ToString();
                txtHorasSemanales.Text = Data.horasSemanales.ToString();
                cbFechaInicio.SelectedDate = Data.fechaContratacion;
                cbFechaFinal.SelectedDate = Data.fechaCulminacion;
            }
        }


        #region Validation
        bool Validate()
        {
            if (txtSueldo.Text == "")
            {
                MessageBox.Show("Debes llenar el Campo Sueldo!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtSueldo.Focus();
                return true;
            }
            if (txtHorasSemanales.Text == "")
            {
                MessageBox.Show("Debes llenar el Campo de Horas Semanales!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtHorasSemanales.Focus();
                return true;
            }
            if (cbFechaFinal.SelectedDate != null)
            {
                MessageBox.Show("Debes seleccionar una fecha de culminación!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtHorasSemanales.Focus();
                return true;
            }

            return false;
        }
        #endregion

       
    }
}
