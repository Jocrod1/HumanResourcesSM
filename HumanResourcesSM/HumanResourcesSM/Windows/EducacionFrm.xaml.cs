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
    public partial class EducacionFrm : Window
    {
        public EducacionFrm(SeleccionFrm Par)
        {
            InitializeComponent();

            ParentFrm = Par;
            isSelection = true;
        }

        public EducacionFrm(DEmpleado empleado)
        {
            InitializeComponent();

            Empleado = empleado;
            isSelection = false;
        }


        public TypeForm Type;
        public DEducacion DataFill;

        public DEducacion UForm;

        public MEducacion Metodos = new MEducacion();

        public SeleccionFrm ParentFrm;
        public DEmpleado Empleado;
        bool isSelection;

        MEducacion methodEdu = new MEducacion();


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

        public void InsertEducacion(DEducacion Edu)
        {
            Edu.idEmpleado = Empleado.idEmpleado;
            var resp = methodEdu.Insertar(Edu);
            if (resp == "OK")
            {
                MAuditoria.Insertar(new DAuditoria(
                                    Menu.ActUsuario.idUsuario,
                                    DAuditoria.Registrar,
                                    "Se ha registrado un Educación del empleado Nº" + Empleado.idEmpleado));

                MessageBox.Show("Registro completado!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void EditEducacion(DEducacion Edu)
        {
            Edu.idEmpleado = Empleado.idEmpleado;
            var resp = methodEdu.Editar(Edu);
            if (resp == "OK")
            {
                MAuditoria.Insertar(new DAuditoria(
                                    Menu.ActUsuario.idUsuario,
                                    DAuditoria.Editar,
                                    "Se ha Editado Educación del empleado Nº" + Empleado.idEmpleado));

                MessageBox.Show("Edición completada!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        void fillData()
        {
            if (Validate())
            {
                UForm = null;
                return;
            }

            string nombreCarrera = txtNombreCarrera.txt.Text;
            string nombreInstitucion = txtNombreInstitucion.txt.Text;
            DateTime fechaingreso = CbFechaIngreso.SelectedDate ?? DateTime.Now;
            if (fechaingreso == DateTime.Now) return;
            DateTime? fechaEgreso = CbFechaEgreso.SelectedDate;

            UForm = new DEducacion(0,
                                   Menu.ActUsuario.idUsuario,
                                   nombreCarrera,
                                   nombreInstitucion,
                                   fechaingreso,
                                   fechaEgreso);
        }

        void Create()
        {
            fillData();
            if (UForm == null)
                return;

            if (isSelection)
                ParentFrm.InsertEducacion(UForm);
            else
                InsertEducacion(UForm);

            this.DialogResult = true;
            this.Close();

        }

        void Update()
        {
            fillData();
            if (UForm == null)
                return;

            UForm.idEducacion = DataFill.idEducacion;
            if (isSelection)
                ParentFrm.EditEducacion(UForm, UForm.idEducacion);
            else
                EditEducacion(UForm);

            this.DialogResult = true;
            this.Close();
        }


        void SetEnable(bool Enable)
        {
            txtNombreCarrera.IsEnabled = Enable;
            txtNombreInstitucion.IsEnabled = Enable;
            CbFechaIngreso.IsEnabled = Enable;
            CbFechaEgreso.IsEnabled = Enable;
            ChBNotEnded.IsEnabled = Enable;
        }

        void fillForm(DEducacion Data)
        {
            if (Data != null)
            {
                txtNombreCarrera.SetText(Data.nombreCarrera);
                txtNombreInstitucion.SetText(Data.nombreInstitucion);
                CbFechaIngreso.SelectedDate = Data.fechaIngreso;
                CbFechaEgreso.SelectedDate = Data.fechaEgreso;
                ChBNotEnded.IsChecked = Data.fechaEgreso == null;
            }
        }

        private void CbFechaIngreso_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbFechaIngreso.SelectedDate != null)
            {
                PlaceFechaIngreso.Text = "";

                CbFechaEgreso.DisplayDateStart = CbFechaIngreso.SelectedDate?.Date;
            }
            else
            {
                PlaceFechaIngreso.Text = "Fecha de Ingreso";

                CbFechaEgreso.DisplayDateStart = null;
            }
        }

        private void CbFechaEgreso_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbFechaEgreso.SelectedDate != null)
            {
                PlaceFechaEgreso.Text = "";

                CbFechaIngreso.DisplayDateEnd = CbFechaEgreso.SelectedDate?.Date;
            }
            else
            {
                PlaceFechaEgreso.Text = "Fecha de Egreso";

                CbFechaIngreso.DisplayDateEnd = null;
            }
        }

        private void ChBNotEnded_Checked(object sender, RoutedEventArgs e)
        {
            CbFechaEgreso.SelectedDate = null;
            CbFechaEgreso.IsEnabled = false;
        }

        private void ChBNotEnded_Unchecked(object sender, RoutedEventArgs e)
        {
            CbFechaEgreso.IsEnabled = true;
        }

        #region Validation
        bool Validate()
        {
            if (txtNombreCarrera.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el Nombre de Titulo!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtNombreCarrera.txt.Focus();
                return true;
            }
            if (txtNombreInstitucion.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el Nombre de Institución!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtNombreInstitucion.txt.Focus();
                return true;
            }
            if (CbFechaIngreso.SelectedDate == null)
            {
                MessageBox.Show("Debes Seleccionar la Fecha de Ingreso!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbFechaIngreso.Focus();
                return true;
            }
            if (CbFechaEgreso.SelectedDate == null && !(ChBNotEnded.IsChecked ?? false))
            {
                MessageBox.Show("Debes Seleccionar la Fecha de Egreso!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbFechaEgreso.Focus();
                return true;
            }

            return false;
        }
        #endregion

        
    }
}
