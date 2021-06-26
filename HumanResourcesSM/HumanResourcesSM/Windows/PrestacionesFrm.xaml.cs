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
    public partial class PrestacionesFrm : Window
    {
        public PrestacionesFrm()
        {
            InitializeComponent();
        }

        public MPrestacion Metodos = new MPrestacion();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                return;
            }

            DPrestacion prestacion = new DPrestacion(
                                            0,
                                            int.Parse(CbEmpleado.SelectedValue.ToString()),
                                            double.Parse(txtMontoPresupuesto.Text),
                                            0,
                                            0,
                                            txtRazon.Text,
                                            "",
                                            DPFechaSolicitud.DisplayDate,
                                            1
                                         );

            string respuesta = Metodos.Insertar(prestacion);

            if(respuesta.Equals("OK"))
            {
                MessageBox.Show("Prestación Registrada Correctamente", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Hide();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DPFechaSolicitud.DisplayDateEnd = DateTime.Today;

            var resp = new MSeleccion().MostrarEmpleado();


            CbEmpleado.ItemsSource = resp;
            CbEmpleado.DisplayMemberPath = "nombre";
            CbEmpleado.SelectedValuePath = "idEmpleado";
        }



        #region Validation
        bool Validate()
        {
            if (CbEmpleado.Text == "")
            {
                MessageBox.Show("Debes seleccionar un empleado!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtMontoPresupuesto.Focus();
                return true;
            }

            if (txtMontoPresupuesto.Text == "")
            {
                MessageBox.Show("Debes llenar el campo Monto Presupuesto!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtMontoPresupuesto.Focus();
                return true;
            }

            if (txtRazon.Text == "")
            {
                MessageBox.Show("Debes llenar el campo Razón!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtRazon.Focus();
                return true;
            }
            if (DPFechaSolicitud.SelectedDate == null)
            {
                MessageBox.Show("Debes seleccionar la fecha de Solicitud!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                DPFechaSolicitud.Focus();
                return true;
            }

            return false;
        }
        #endregion
    }
}
