using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Datos;
using Metodos;

namespace HumanResourcesSM.Windows
{
    /// <summary>
    /// Interaction logic for PagoFrm.xaml
    /// </summary>
    public partial class PagoFrm : Page
    {
        MPago met = new MPago();

        public PagoFrm()
        {
            InitializeComponent();

            txtHorasTrabajadas.KeyDown += new KeyEventHandler(Validaciones.TextBox_KeyDown);
            txtHorasTrabajadas.KeyDown += new KeyEventHandler(
                (s,t) =>
                {
                    if(t.Key == Key.Enter)
                    {
                        BtnAccept.Focus();
                    }
                }
            );
        }
        private void BtnReporte_Click(object sender, RoutedEventArgs e)
        {

            SeleccionarPago dialog = new SeleccionarPago();
            if(dialog.ShowDialog() ?? false)
            {
                var id = dialog.PagoSeleccionado.idPago;

                var PagoSeleccionado = dialog.PagoSeleccionado;
                Reports.Reporte reporte = new Reports.Reporte();
                var pagol = met.Mostrar(PagoSeleccionado.idPago);
                var detalle = met.MostrarDetalle(PagoSeleccionado.idPago);
                reporte.ExportPDFTwoArguments(detalle, "Pago", pagol, "PagoGeneral", true, PagoSeleccionado.idPago.ToString());
            }

            
        }

        void limpiar()
        {
            BordEmpleado.Visibility = Visibility.Collapsed;

            EmpleadoSeleccionado = null;

            Deudas.Clear();
            modelo.Clear();
            dgOperaciones.ItemsSource = null;
            dgOperaciones.Visibility = Visibility.Collapsed;

            Sueldo = Bonificaciones = Deducciones = Total = 0;

            txtHorasTrabajadas.Text = "0";
            txtHorasExtras.Text = "0";

            txtTotalSueldo.Text = Sueldo.ToString("0.00") + " €";
            txtMontoBonificaciones.Text = Bonificaciones.ToString("0.00") + " €";
            txtMontoDeducciones.Text = "-" + Deducciones.ToString("0.00") + " €";
            txtMontoTotal.Text = Total.ToString("0.00") + " €";

            

        }

        private void BtnSeleccionarEmpleado_Click(object sender, RoutedEventArgs e)
        {
            PagoSeleccionVista Frm = new PagoSeleccionVista(this);
            Frm.ShowDialog();
        }

        private void BtnAccept_Click(object sender, RoutedEventArgs e)
        {
            if(EmpleadoSeleccionado == null)
            {
                return;
            }
            if(HorasTrabajadas == 0)
            {
                return;
            }

            PagoRealizadoVista frm = new PagoRealizadoVista(this);
            frm.ShowDialog();
 
        }

        public void RealizarPago(DPago PagoData)
        {
            if (EmpleadoSeleccionado == null)
            {
                return;
            }
            if (HorasTrabajadas == 0)
            {
                return;
            }

            DPago pago = new DPago(0,
                        EmpleadoSeleccionado.idEmpleado,
                        DateTime.Now,
                        PagoData.banco,
                        PagoData.numeroReferencia,
                        HorasTrabajadas,
                        PagoData.periodoInicio,
                        PagoData.periodoFinal,
                        Total,
                        TotalAsignaciones,
                        TotalDeducciones,
                        Sueldo,
                        1);

            List<DDetallePago> detallepagos = new List<DDetallePago>();

            foreach (var item in modelo)
            {


                detallepagos.Add(
                    new DDetallePago(0, 0,
                                     item.Concepto,
                                     item.CantidadString,
                                     item.SalarioString,
                                     item.AsignacionesString,
                                     item.DeduccionesString)
                );
            }
            
            var resp = met.Insertar(pago, detallepagos);

            if (resp.Equals("OK"))
            {
                MAuditoria.Insertar(new DAuditoria(
                                    Menu.ActUsuario.idUsuario,
                                    DAuditoria.Registrar,
                                    "Se ha registrado un Pago para el empleado Nº" + pago.idEmpleado));

                var msgResp = MessageBox.Show("¡Pago Procesado!" + Environment.NewLine + "¿Desea Guardar el Comprobante de Pago?", "SwissNet", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (msgResp == MessageBoxResult.Yes)
                {
                    var ultimopago = met.MostrarUltimo()[0];
                    Reports.Reporte reporte = new Reports.Reporte();
                    var pagol = met.Mostrar(ultimopago.idPago);
                    var detalle = met.MostrarDetalle(ultimopago.idPago);
                    reporte.ExportPDFTwoArguments(detalle, "Pago", pagol, "PagoGeneral", true, ultimopago.idPago.ToString());
                }
                limpiar();
            }
            else
            {
                MessageBox.Show(resp);
                limpiar();
            }
        }

        int HorasTrabajadas = 0;
        int HorasExtras = 0;

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtHorasTrabajadas.Text == "")
                HorasTrabajadas = 0;
            if (txtHorasExtras.Text == "")
                HorasExtras = 0;

            HorasTrabajadas = int.Parse(txtHorasTrabajadas.Text);
            HorasExtras = int.Parse(txtHorasExtras.Text);


            txtHorasTrabajadas.Text = HorasTrabajadas.ToString();
            txtHorasExtras.Text = HorasExtras.ToString();
            RefreshMoney();
        }

        public DEmpleado EmpleadoSeleccionado;

        public void SetEmpleado(DEmpleado empleado)
        {
            BordEmpleado.Visibility = Visibility.Visible;

            EmpleadoSeleccionado = empleado;

            txtNombre.Text = empleado.nombre;
            txtApellido.Text = empleado.apellido;
            txtSueldo.Text = empleado.SueldoString;
            txtDepartamento.Text = empleado.nombreDepartamento;

            txtEmail.Text = empleado.email;
            txtTelf.Text = empleado.telefono;
            txtUltimoPago.Text = empleado.ultimoPagoFecha;
            TxtUltimoPeriodo.Text = empleado.periodo;

            var resp = new MDeuda().MostrarDeudaEmpleado(empleado.idEmpleado, 1);

            Deudas = resp;

            //ConstruirDeudas();

            txtHorasTrabajadas.Focus();

            //RefreshMoney();
        }

        double Sueldo = 0, Bonificaciones = 0, Deducciones = 0, Total = 0;

        double TotalAsignaciones = 0, TotalDeducciones = 0;

        void RefreshMoney()
        {
            if (EmpleadoSeleccionado == null)
                return;

            modelo.Clear();


            Sueldo = Bonificaciones = Deducciones = Total = TotalAsignaciones = TotalDeducciones = 0;


            double SueldoBase = EmpleadoSeleccionado.sueldo * HorasTrabajadas;

            modelo.Add(new ModeloDetallePago()
            {
                Concepto = "Sueldo por horas trabajadas",
                Cantidad = HorasTrabajadas,
                Salario = EmpleadoSeleccionado.sueldo,
                Asignaciones = SueldoBase,
                Deducciones = 0,
            });

            double multi = 2;

            double SueldoExtra = EmpleadoSeleccionado.sueldo * 2;

            double PagoHxtra = SueldoExtra * HorasExtras;

            if (HorasExtras > 0)
            {
                modelo.Add(new ModeloDetallePago()
                {
                    Concepto = "Pago por Horas Extras",
                    Cantidad = HorasExtras,
                    Salario = SueldoExtra,
                    Asignaciones = PagoHxtra,
                    Deducciones = 0,
                });
            }

            Sueldo = SueldoBase + PagoHxtra;

            foreach (var item in Deudas)
            {
                if (item.tipoDeuda == 1)
                    continue;
                double cantidad = 0;
                double asignaciones = 0;

                if(item.tipoPago == 1)
                {
                    asignaciones = item.monto;
                }
                else
                {
                    cantidad = item.monto;
                    asignaciones = Sueldo * (cantidad / 100);
                }

                var Con = new ModeloDetallePago()
                {
                    Concepto = item.concepto,
                    Cantidad = cantidad,
                    Salario = 0,
                    Asignaciones = asignaciones,
                    Deducciones = 0
                };

                modelo.Add(Con);
            }

            foreach (var item in Deudas)
            {
                if (item.tipoDeuda == 0)
                    continue;
                double cantidad = 0;
                double deducciones = 0;

                if (item.tipoPago == 1)
                {
                    deducciones = item.monto;
                }
                else
                {
                    cantidad = item.monto;
                    deducciones = Sueldo * (cantidad / 100);
                }

                var Con = new ModeloDetallePago()
                {
                    Concepto = item.concepto,
                    Cantidad = cantidad,
                    Salario = 0,
                    Asignaciones = 0,
                    Deducciones = deducciones
                };
                modelo.Add(Con);
            }

            foreach (var item in modelo)
            {
                TotalAsignaciones += item.Asignaciones;
                TotalDeducciones += item.Deducciones;
            }


            Total = TotalAsignaciones - TotalDeducciones;

            txtTotalSueldo.Text = Sueldo.ToString("0.00") + " €";
            txtMontoBonificaciones.Text = TotalAsignaciones.ToString("0.00") + " €";
            txtMontoDeducciones.Text = "-" + TotalDeducciones.ToString("0.00") + " €";
            txtMontoTotal.Text = Total.ToString("0.00") + " €";
            dgOperaciones.ItemsSource = null;
            dgOperaciones.ItemsSource = modelo;
            dgOperaciones.Visibility = Visibility.Visible;
        }

        //Refresh Bonificaciones y Deducciones

        List<DDeuda> Deudas = new List<DDeuda>();
        List<ModeloDetallePago> modelo = new List<ModeloDetallePago>();


        class ModeloDetallePago
        {

            public ModeloDetallePago()
            {

            }
            public ModeloDetallePago(string concepto, double cantidad, double salario, double asignaciones, double deducciones)
            {
                this.Concepto = concepto;
                this.Cantidad = cantidad;
                this.Salario = salario;
                this.Asignaciones = asignaciones;
                this.Deducciones = deducciones;
            }
            public string Concepto { get; set; }
            public double Cantidad { get; set; }
            public double  Salario { get; set; }
            public double Asignaciones { get; set; }
            public double Deducciones { get; set; }
            public string CantidadString
            {
                get
                {
                    if(Cantidad > 0)
                    {
                        if(Salario == 0)
                        {
                            return Cantidad + "%";
                        }
                        return Cantidad.ToString();
                    }
                    else
                    {
                        return "-";
                    }
                }
            }
            public string SalarioString
            {
                get
                {
                    if(Salario > 0)
                    {
                        return Salario + " €";
                    }
                    else
                    {
                        return "-";
                    }
                }
            }
            public string AsignacionesString
            {
                get
                {
                    if (Asignaciones > 0)
                    {
                        return Asignaciones + " €";
                    }
                    else
                    {
                        return "-";
                    }
                }
            }
            public string DeduccionesString
            {
                get
                {
                    if (Deducciones > 0)
                    {
                        return Deducciones + " €";
                    }
                    else
                    {
                        return "-";
                    }
                }
            }
        }
        
    }
}
