using System;
using System.Windows;
using System.Windows.Controls;
using Datos;
using Metodos;

namespace HumanResourcesSM.Windows
{
    public partial class SeleccionVista : Window
    {
        public SeleccionVista(DEmpleado empleado, DSeleccion seleccion)
        {
            InitializeComponent();

            DataFill = new FormSeleccion(empleado, seleccion);
        }


        public TypeForm Type = TypeForm.Create;
        public FormSeleccion DataFill;

        public FormSeleccion UForm;
        public MSeleccion Metodos = new MSeleccion();

        public EntrevistarFrm ParentFrm;


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MSeleccion Metd = new MSeleccion();

            var resp = Metd.MostrarPaises();

            CbPaisNac.ItemsSource = resp;
            CbPaisNac.DisplayMemberPath = "Pais";
            CbPaisNac.SelectedValuePath = "Codigo";

            var resp2 = new MDepartamento().Mostrar("");

            CbDepartamento.ItemsSource = resp2;
            CbDepartamento.DisplayMemberPath = "nombre";
            CbDepartamento.SelectedValuePath = "idDepartamento";

            var resp3 = new MTipoTramite().MostrarStatus();

            CbEstadoLegal.ItemsSource = resp3;
            CbEstadoLegal.DisplayMemberPath = "statusCambio";
            CbEstadoLegal.SelectedValuePath = "statusCambio";

            fillForm(DataFill);
        }



        void fillData()
        {
            if (Validate())
            {
                UForm = null;
                return;
            }

            /*         EMPLEADO           */
            int idDepartamento = (int)CbDepartamento.SelectedValue;
            string nombre = txtNombre.Text;
            string apellido = txtApellido.Text;
            string DNI = txtDNI.Text;
            DateTime fechaNacimiento = CbFechaNac.SelectedDate ?? DateTime.Now;
            if (fechaNacimiento == DateTime.Now) return;
            string nacionalidad = (string)CbPaisNac.SelectedValue;
            string direccion = txtDireccion.Text;
            string email = txtEmail.Text;
            string telefono = txtTelefono.Text;
            string URLCV = txtURLCV.Text;
            string estadoLegal = CbEstadoLegal.Text;
            string nombrePuesto = txtNombrePuesto.Text;
            /*         END EMPLEADO           */

            /*         SELECCIÓN           */
            int idSeleccionador = Menu.ActUsuario.idUsuario;
            DateTime fechaAplicacion = CbFechaApl.SelectedDate ?? DateTime.Now;
            if (fechaAplicacion == DateTime.Now) return;
            /*         END SELECCIÓN           */



            DEmpleado Empleado = new DEmpleado(DataFill.empleado.idEmpleado,
                                                idDepartamento,
                                                nombre,
                                                apellido,
                                                DNI,
                                                fechaNacimiento,
                                                nacionalidad,
                                                direccion,
                                                email,
                                                telefono,
                                                URLCV,
                                                estadoLegal,
                                                0);

            DSeleccion Seleccion = new DSeleccion(DataFill.seleccion.idSeleccion,
                                                    DataFill.empleado.idEmpleado,
                                                    0,
                                                    0,
                                                    fechaAplicacion,
                                                    0,
                                                    DateTime.Now,
                                                    nombrePuesto);

            UForm = new FormSeleccion(Empleado, Seleccion);
        }

        //void Create()
        //{
        //    fillData();
        //    if (UForm == null)
        //        return;

        //    ParentFrm.InsertIdioma(UForm);

        //    this.DialogResult = true;
        //    this.Close();

        //}

        void Update()
        {
            fillData();
            if (UForm == null)
                return;

            var resp = Metodos.EditarEmpleado(UForm.seleccion, UForm.empleado);
            Console.WriteLine(resp);

            this.DialogResult = true;
            this.Close();
        }


        void fillForm(FormSeleccion Data)
        {
            if (Data != null)
            {
                DEmpleado Empleado = Data.empleado;
                DSeleccion Seleccion = Data.seleccion;

                txtNombre.Text = Empleado.nombre;
                txtApellido.Text = Empleado.apellido;

                string[] words = Empleado.cedula.Split('-');
                cbTipoDocumento.SelectedIndex = words[0] == "D" ? 0 : words[0] == "P" ? 1 : -1;
                txtDNI.Text = words[1];

                CbPaisNac.SelectedValue = Empleado.nacionalidad;
                CbFechaNac.SelectedDate = Empleado.fechaNacimiento;
                txtEmail.Text = Empleado.email;
                txtTelefono.Text = Empleado.telefono;
                txtDireccion.Text = Empleado.direccion;
                CbEstadoLegal.Text = Empleado.estadoLegal;

                CbDepartamento.SelectedValue = Empleado.idDepartamento;
                CbFechaApl.SelectedDate = Seleccion.fechaAplicacion;
                txtNombrePuesto.Text = Seleccion.nombrePuesto;
                txtURLCV.Text = Empleado.curriculum;
            }
        }


        #region Validation
        bool Validate()
        {
            if (txtNombre.Text == "")
            {
                MessageBox.Show("Debes llenar el campo Nombre!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtNombre.Focus();
                return true;
            }
            if (txtApellido.Text == "")
            {
                MessageBox.Show("Debes llenar el campo Apellido!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtApellido.Focus();
                return true;
            }
            if (txtDNI.Text == "")
            {
                MessageBox.Show("Debes llenar el campo DNI!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtDNI.Focus();
                return true;
            }
            if (CbPaisNac.SelectedIndex == -1)
            {
                MessageBox.Show("Debes Seleccionar el Pais de Nacimiento!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbPaisNac.Focus();
                return true;
            }
            if (CbFechaNac.SelectedDate == null)
            {
                MessageBox.Show("Debes Seleccionar la Fecha de Nacimiento!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbFechaNac.Focus();
                return true;
            }
            if (txtEmail.Text == "" && txtTelefono.Text == "")
            {
                MessageBox.Show("Debes llenar al menos un campo de Email o Telefono!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtEmail.Focus();
                return true;
            }
            if (CbEstadoLegal.Text == "")
            {
                MessageBox.Show("Debes llenar el campo Estado Legal!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbEstadoLegal.Focus();
                return true;
            }
            if (CbDepartamento.SelectedIndex == -1)
            {
                MessageBox.Show("Debes Seleccionar el Departamento!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbDepartamento.Focus();
                return true;
            }
            if (txtNombrePuesto.Text == "")
            {
                MessageBox.Show("Debes llenar el campo de Nombre la Posición!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtNombrePuesto.Focus();
                return true;
            }
            if (CbFechaApl.SelectedDate == null)
            {
                MessageBox.Show("Debes Seleccionar la Fecha de Aplicación!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbFechaApl.Focus();
                return true;
            }
            if (txtURLCV.Text == "")
            {
                MessageBox.Show("Debes llenar el campo de URL del Currículo!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtURLCV.Focus();
                return true;
            }

            return false;
        }
        #endregion

        public class FormSeleccion
        {
            public FormSeleccion(DEmpleado empleado, DSeleccion seleccion)
            {
                this.empleado = empleado;
                this.seleccion = seleccion;
            }

            public FormSeleccion()
            {

            }

            public DEmpleado empleado { get; set; }
            public DSeleccion seleccion { get; set; }
        }
    }
}
