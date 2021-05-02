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
            //if (Type == TypeForm.Update)
            Update();
            //else
            //    Create();
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
            string nombre = txtNombre.txt.Text;
            string apellido = txtApellido.txt.Text;
            string DNI = txtDNI.txt.Text;
            DateTime fechaNacimiento = CbFechaNac.SelectedDate ?? DateTime.Now;
            if (fechaNacimiento == DateTime.Now) return;
            string nacionalidad = (string)CbPaisNac.SelectedValue;
            string direccion = txtDireccion.txt.Text;
            string email = txtEmail.txt.Text;
            string telefono = txtTelefono.txt.Text;
            string URLCV = txtURLCV.txt.Text;
            string estadoLegal = CbEstadoLegal.Text;
            string nombrePuesto = txtNombrePuesto.txt.Text;
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
                                                    idSeleccionador,
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


        //void SetEnable(bool Enable)
        //{
        //    CbIdioma.IsEnabled = Enable;
        //    CbNivel.IsEnabled = Enable;
        //}

        void fillForm(FormSeleccion Data)
        {
            if (Data != null)
            {
                DEmpleado Empleado = Data.empleado;
                DSeleccion Seleccion = Data.seleccion;

                txtNombre.SetText(Empleado.nombre);
                txtApellido.SetText(Empleado.apellido);
                txtDNI.SetText(Empleado.cedula);
                CbPaisNac.SelectedValue = Empleado.nacionalidad;
                CbFechaNac.SelectedDate = Empleado.fechaNacimiento;
                txtEmail.SetText(Empleado.email);
                txtTelefono.SetText(Empleado.telefono);
                txtDireccion.SetText(Empleado.direccion);
                CbEstadoLegal.Text = Empleado.estadoLegal;
                CbDepartamento.SelectedValue = Empleado.idDepartamento;
                CbFechaApl.SelectedDate = Seleccion.fechaAplicacion;
                txtNombrePuesto.SetText(Seleccion.nombrePuesto);
                txtURLCV.SetText(Empleado.curriculum);
            }
        }

        private void CbDepartamento_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbDepartamento.SelectedIndex > -1)
            {
                PlaceDepartamento.Text = "";
            }
            else
            {
                PlaceDepartamento.Text = "Departamento";
            }
        }

        private void CbFechaNac_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbFechaNac.SelectedDate != null)
            {
                PlaceFechaNac.Text = "";
            }
            else
            {
                PlaceFechaNac.Text = "Fecha de Nacimiento";
            }
        }

        private void CbPaisNac_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbPaisNac.SelectedIndex > -1)
            {
                PlacePaisNac.Text = "";
            }
            else
            {
                PlacePaisNac.Text = "País de Nacimiento";
            }
        }

        private void CbEstadoLegal_GotFocus(object sender, RoutedEventArgs e)
        {
            if (CbEstadoLegal.Text == "")
            {
                PlaceEstadoLegal.Visibility = Visibility.Collapsed;
            }
        }

        private void CbEstadoLegal_LostFocus(object sender, RoutedEventArgs e)
        {
            if (CbEstadoLegal.Text == "")
            {
                PlaceEstadoLegal.Visibility = Visibility.Visible;
            }
        }

        private void CbFechaApl_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbFechaApl.SelectedDate != null)
            {
                PlaceFechaApl.Text = "";
            }
            else
            {
                PlaceFechaApl.Text = "Fecha de Aplicación";
            }
        }

        #region Validation
        bool Validate()
        {
            if (txtNombre.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el campo Nombre!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtNombre.txt.Focus();
                return true;
            }
            if (txtApellido.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el campo Apellido!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtApellido.txt.Focus();
                return true;
            }
            if (txtDNI.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el campo DNI!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtDNI.txt.Focus();
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
            if (txtEmail.txt.Text == "" && txtTelefono.txt.Text == "")
            {
                MessageBox.Show("Debes llenar al menos un campo de Email o Telefono!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtEmail.txt.Focus();
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
            if (txtNombrePuesto.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el campo de Nombre la Posición!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtNombrePuesto.txt.Focus();
                return true;
            }
            if (CbFechaApl.SelectedDate == null)
            {
                MessageBox.Show("Debes Seleccionar la Fecha de Aplicación!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbFechaApl.Focus();
                return true;
            }
            if (txtURLCV.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el campo de URL del Currículo!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtURLCV.txt.Focus();
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
