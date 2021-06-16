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

            txtPorcentajePrestacion.KeyDown += new KeyEventHandler(Validaciones.TextBox_KeyDown);
        }

        public MPrestacion Metodos = new MPrestacion();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                return;
            }

            //HACER CALCULO DE MONTOTOTAL
            
            string razonOtorgado = txtRazonResultado.Text;

            DPrestacion Res;

            if (RBOtorgado.IsChecked ?? false)
            {
                int porcentajeOtorgado = int.Parse(txtPorcentajePrestacion.Text);
                Res = new DPrestacion()
                {
                    idPrestacion = Prestacion.idPrestacion,
                    porcentajeOtorgado = porcentajeOtorgado,
                    montoOtorgado = montoPorcentual,
                    estado = 2
                };
            }
            else if (RBNoOtorgado.IsChecked ?? false)
            {
                Res = new DPrestacion()
                {
                    idPrestacion = Prestacion.idPrestacion,
                    porcentajeOtorgado = 0,
                    montoOtorgado = 0,
                    estado = 3
                };
            }
            else
                return;

            //ejecutar prestacion aca
            string respuesta = Metodos.AsignarPrestacion(Res);

            if (respuesta.Equals("OK"))
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show(respuesta);
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtFecha.Text = "Solicitado el " + Prestacion.fechaSolicitudString;
            txtEmpleado.Text = Prestacion.nombreEmpleado;
            txtCargo.Text = Prestacion.nombrePuesto;

            TimeSpan yearsOld = Prestacion.fechaSolicitud - Prestacion.fechaContratacion;
            int years = (int)(yearsOld.TotalDays / 365.25);
            int months = (int)(((yearsOld.TotalDays / 365.25) - years) * 12);

            string antiguedad = "";

            if(years == 1)
                antiguedad = years + " año ";
            if (years > 1)
                antiguedad = years + " años ";
            else
                antiguedad = months + " meses";

            if(years > 0)
            {
                valorCalculado = years * 30 * Prestacion.sueldo * 8;
            }
            else
            {
                valorCalculado = months * 5 * Prestacion.sueldo * 8;
            }

            double porcentajePresupuesto = ((Prestacion.montoPresupuesto / valorCalculado) * 100);

            porcentajePresupuesto = Math.Truncate(100 * porcentajePresupuesto) / 100;

            txtValorCalculado.Text = porcentajePresupuesto  + " %";

            txtAntiguedad.Text = antiguedad;

            txtMonto.Text = Prestacion.montoPresupuesto + " €";
            txtRazonPrestacion.Text = Prestacion.razon;


            txtPorcentajePrestacion.IsEnabled = false;
        }

        double valorCalculado = 0, montoPorcentual = 0;



        #region Validation
        bool Validate()
        {
            if(!(RBOtorgado.IsChecked == true || RBNoOtorgado.IsChecked == true))
            {
                MessageBox.Show("Debe Seleccionar Si se otorgará o no!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                txtPorcentajePrestacion.Focus();
                return true;
            }
            if (txtPorcentajePrestacion.Text == "" && RBOtorgado.IsChecked == true)
            {
                MessageBox.Show("Debes llenar el campo Monto Presupuesto!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                txtPorcentajePrestacion.Focus();
                return true;
            }

            //if (txtRazonResultado.Text == "")
            //{
            //    MessageBox.Show("Debes llenar el campo Razón!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
            //    txtRazonResultado.Focus();
            //    return true;
            //}

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


        private void txtPorcentajePrestacion_LostFocus(object sender, RoutedEventArgs e)
        {
            if(((TextBox)sender).Text == "")
            {
            }
            else
            {
                int porcentaje = int.Parse(((TextBox)sender).Text);

                if(porcentaje > 75)
                {
                    porcentaje = 75;
                    ((TextBox)sender).Text = porcentaje.ToString();
                }

                montoPorcentual = valorCalculado * ((double)porcentaje / 100);

                txtMontoPorcentual.Text = "Monto: " + montoPorcentual + " €";
            }
        }
    }
}
