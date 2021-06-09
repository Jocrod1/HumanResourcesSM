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
    public partial class RespuestaPrestacionFrm : Window
    {
        DPrestacion Prestacion = new DPrestacion();

        public RespuestaPrestacionFrm(DPrestacion prestacion)
        {
            InitializeComponent();

            Prestacion = prestacion;
        }

        public MPrestacion Metodos = new MPrestacion();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                return;
            }

            //HACER CALCULO DE MONTOTOTAL

            //ejecutar prestacion aca
            //string respuesta = Metodos.AsignarPrestacion(new DPrestacion);
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtFecha.Text = "Solicitado el 88 / 88 / 8888 " + Prestacion.fechaSolicitudString;
            txtEmpleado.Text = Prestacion.nombreEmpleado;
            txtCargo.Text = Prestacion.nombrePuesto;

            TimeSpan yearsOld = Prestacion.fechaSolicitud - Prestacion.fechaContratacion;
            int years = (int)(yearsOld.TotalDays / 365.25);
            int months = (int)(((yearsOld.TotalDays / 365.25) - years) * 12);

            string antiguedad = "";

            if (years > 0)
                antiguedad = years + " años y " + (months > 0 ? months + " meses" : "");
            else
                antiguedad = months + " meses";

            txtAntiguedad.Text = antiguedad;

            txtMonto.Text = Prestacion.montoPresupuesto.ToString();
            txtRazonPrestacion.Text = Prestacion.razon;


            txtPorcentajePrestacion.IsEnabled = false;
        }



        #region Validation
        bool Validate()
        {
            if (txtPorcentajePrestacion.Text == "")
            {
                MessageBox.Show("Debes llenar el campo Monto Presupuesto!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                txtPorcentajePrestacion.Focus();
                return true;
            }

            if (txtRazonResultado.Text == "")
            {
                MessageBox.Show("Debes llenar el campo Razón!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                txtRazonResultado.Focus();
                return true;
            }

            return false;
        }
        #endregion

        private void RBNoOtorgado_Checked(object sender, RoutedEventArgs e)
        {
            txtPorcentajePrestacion.Text = "";
            txtPorcentajePrestacion.IsEnabled = false;

            txtRazonResultado.Focus();
        }

        private void RBOtorgado_Checked(object sender, RoutedEventArgs e)
        {
            txtPorcentajePrestacion.IsEnabled = true;
            txtPorcentajePrestacion.Focus();
        }

        private int RespuestaPrestacion ()
        {
            if(RBOtorgado.IsChecked == true)
            {
                return 2;
            }
            else if (RBOtorgado.IsChecked == true)
            {
                return 3;
            }

            return 1;
        }
    }
}
