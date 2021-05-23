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
            //if (dgOperaciones.Items.Count == 0)
            //{
            //    MessageBox.Show("No se puede realizar un Reporte vacio!", "SwissNet", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            //    return;
            //}

            //Reports.Reporte reporte = new Reports.Reporte();
            //reporte.ExportPDF(Metodos.MostrarReporte(), "RelacionesLaborales");
        }

        void limpiar()
        {
            BordEmpleado.Visibility = Visibility.Collapsed;

            EmpleadoSeleccionado = null;

            Deudas.Clear();
            modelo.Clear();
            StackBonificaciones.Children.Clear();
            StackDeducciones.Children.Clear();

            Sueldo = Bonificaciones = Deducciones = Total = 0;

            txtHorasTrabajadas.Text = "0";

            txtMontoSueldo.Text = Sueldo.ToString("0.00") + " €";
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
                        1);

            List<DDetallePago> detallepagos = new List<DDetallePago>();
            detallepagos.Add(
                new DDetallePago(0, 0, 0, "Sueldo", Sueldo)
            );
            foreach (var item in modelo)
            {
                if (!item.Enabled)
                    continue;

                double multiplier = item.deuda.tipoDeuda == 0 ? 1 : -1;

                detallepagos.Add(
                    new DDetallePago(0, 0,
                                     item.deuda.idDeuda,
                                     item.deuda.concepto,
                                     item.Pagando * multiplier)
                );
            }
            var met = new MPago();
            var resp = met.Insertar(pago, detallepagos);

            if (resp.Equals("OK"))
            {
                var msgResp = MessageBox.Show("¡Pago Procesado!" + Environment.NewLine + "¿Desea Guardar el Comprobante de Pago?", "SwissNet", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (msgResp == MessageBoxResult.Yes)
                {
                    var ultimopago = met.MostrarUltimo()[0];
                    Reports.Reporte reporte = new Reports.Reporte();
                    reporte.ExportPDFTwoArguments(new MPago().MostrarDetalle(ultimopago.idPago), "Pago", new MPago().Mostrar(ultimopago.idPago), "PagoGeneral", true, ultimopago.idPago.ToString());
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

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text == "")
                HorasTrabajadas = 0;

            HorasTrabajadas = int.Parse(txtHorasTrabajadas.Text);


            ((TextBox)sender).Text = HorasTrabajadas.ToString();
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

            ConstruirDeudas();

            txtHorasTrabajadas.Focus();

            RefreshMoney();
        }

        double Sueldo = 0, Bonificaciones = 0, Deducciones = 0, Total = 0;

        void RefreshMoney()
        {
            if (EmpleadoSeleccionado == null)
                return;

            Sueldo = Bonificaciones = Deducciones = Total = 0;

            Sueldo = EmpleadoSeleccionado.sueldo * HorasTrabajadas;

            foreach(var item in modelo)
            {
                if (!item.Enabled)
                    continue;

                if(item.deuda.tipoDeuda == 0)
                {
                    Bonificaciones += item.Pagando;
                }
                else
                {
                    Deducciones += item.Pagando;
                }
            }

            Total = Sueldo + Bonificaciones - Deducciones;

            txtMontoSueldo.Text = Sueldo.ToString("0.00") + " €";
            txtMontoBonificaciones.Text = Bonificaciones.ToString("0.00") + " €";
            txtMontoDeducciones.Text = "-" + Deducciones.ToString("0.00") + " €";
            txtMontoTotal.Text = Total.ToString("0.00") + " €";

        }

        //Refresh Bonificaciones y Deducciones

        List<DDeuda> Deudas = new List<DDeuda>();
        List<ModeloDetallePago> modelo = new List<ModeloDetallePago>();

        

        void ConstruirDeudas()
        {
            modelo.Clear();

            RefreshBonificaciones(Deudas.FindAll(x => x.tipoDeuda == 0));
            RefreshDeducciones(Deudas.FindAll(x => x.tipoDeuda == 1));
        }

        void RefreshBonificaciones(List<DDeuda> Bonificaciones)
        {
            StackBonificaciones.Children.Clear();
            

            foreach (DDeuda bonificacion in Bonificaciones)
            {
                StackBonificaciones.Children.Add(ConstruirDeuda(bonificacion));
                modelo.Add(new ModeloDetallePago(bonificacion, (bonificacion.monto - bonificacion.pagado), true));
            }
        }

        void RefreshDeducciones(List<DDeuda> Deducciones)
        {
            StackDeducciones.Children.Clear();

            foreach (DDeuda deduccion in Deducciones)
            {
                StackDeducciones.Children.Add(ConstruirDeuda(deduccion));
                modelo.Add(new ModeloDetallePago(deduccion, (deduccion.monto - deduccion.pagado), true));
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chB = (CheckBox)sender;
            int id = (int)chB.CommandParameter;
            int index = modelo.FindIndex(x => x.deuda.idDeuda == id);

            if (!(chB.IsChecked ?? false))
            {
                var parent = VisualTreeHelper.GetParent(chB) as Grid;
                var Title = parent.Children[1] as TextBlock;
                Title.Foreground = Brushes.Gray;
                modelo[index].Enabled = false;
            }
            else
            {
                var parent = VisualTreeHelper.GetParent(chB) as Grid;
                var Title = parent.Children[1] as TextBlock;
                Title.Foreground = Brushes.Black;
                modelo[index].Enabled = true;

            }
            RefreshMoney();

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox CB = (ComboBox)sender;

            var parent = VisualTreeHelper.GetParent(CB) as StackPanel;
            var PanelInput = parent.Children[1] as StackPanel;
            var MoneyText = PanelInput.Children[1] as TextBlock;

            int id = GetIDFromStackPanel(parent);
            int index = modelo.FindIndex(x => x.deuda.idDeuda == id);

            if (CB.SelectedIndex == 0)
            {   
                PanelInput.IsEnabled = false;
                MoneyText.Foreground = Brushes.Gray;
                modelo[index].Pagando = (modelo[index].deuda.monto - modelo[index].deuda.pagado);
            }
            else
            {
                PanelInput.IsEnabled = true;
                MoneyText.Foreground = Brushes.Black;
                var MoneyBox = PanelInput.Children[0] as TextBox;
                modelo[index].Pagando = double.Parse(MoneyBox.Text);
            }
            RefreshMoney();

        }

        private void TextPrecio_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txt = (TextBox)sender;

            var HorStack = VisualTreeHelper.GetParent(txt) as StackPanel;
            var parent = VisualTreeHelper.GetParent(HorStack) as StackPanel;
            int id = GetIDFromStackPanel(parent);
            int index = modelo.FindIndex(x => x.deuda.idDeuda == id);

            double CantidadEspecifica = 0;
            if (txt.Text == "")
                CantidadEspecifica = 0;
            else
            {
                CantidadEspecifica = double.Parse(txt.Text);

                
                if(CantidadEspecifica < 0 || CantidadEspecifica > modelo[index].deuda.monto)
                {
                    //error
                    CantidadEspecifica = modelo[index].deuda.monto - modelo[index].deuda.pagado;
                }
            }
            txt.Text = CantidadEspecifica.ToString("0.00");
            modelo[index].Pagando = CantidadEspecifica;
            RefreshMoney();
        }

        int GetIDFromStackPanel(StackPanel SP)
        {
            var Cuerpo = VisualTreeHelper.GetParent(SP) as Grid;
            var Marcototal = VisualTreeHelper.GetParent(Cuerpo) as StackPanel;
            var marctoTitulo = Marcototal.Children[0] as Border;
            var gridtitulo = marctoTitulo.Child as Grid;
            CheckBox CB = gridtitulo.Children[0] as CheckBox;
            int id = (int)CB.CommandParameter;

            return id;
        }



        Border ConstruirDeuda(DDeuda Deuda)
        {
            Border MarcoPrincipal = new Border()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Background = Brushes.WhiteSmoke
            };

            StackPanel PilarPrincipal = new StackPanel();
            MarcoPrincipal.Child = PilarPrincipal;

            Border MarcoTitulo = new Border()
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(0, 0, 0, 1)
            };
            PilarPrincipal.Children.Add(MarcoTitulo);

            //Seccion Superior

            CheckBox ChBEnable = new CheckBox()
            {
                CommandParameter = Deuda.idDeuda,
                IsChecked = true
            };
            ChBEnable.Checked += new RoutedEventHandler(CheckBox_Checked);
            ChBEnable.Unchecked += new RoutedEventHandler(CheckBox_Checked);

            Grid Titulo = new Grid();
            Titulo.Children.Add(ChBEnable);
            Titulo.Children.Add(new TextBlock()
            {
                Text = Deuda.concepto,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontWeight = FontWeights.Bold,
                FontSize = 15,
                Margin = new Thickness(10, 5, 10, 5)
            });
            MarcoTitulo.Child = Titulo;

            //Seccion Inferior

            Grid Cuerpo = new Grid();
            Cuerpo.ColumnDefinitions.Add(
                new ColumnDefinition()
                {
                    Width = new GridLength(180, GridUnitType.Pixel)
                }
            );
            Cuerpo.ColumnDefinitions.Add(
                new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                }
            );
            PilarPrincipal.Children.Add(Cuerpo);

            //Montos

            Border MarcoMontos = new Border()
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(0, 0, 1, 0)
            };
            Cuerpo.Children.Add(MarcoMontos);

            StackPanel PanelMontos = new StackPanel()
            {
                Margin = new Thickness(5)
            };
            MarcoMontos.Child = PanelMontos;


            int FontSizeMontos = 12;

            Grid GridMontoInicial = new Grid();
            GridMontoInicial.Children.Add(
                new TextBlock()
                {
                    Text = "Inicial",
                    FontSize = FontSizeMontos,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.Gray
                }
            );
            GridMontoInicial.Children.Add(
                new TextBlock()
                {
                    Text = Deuda.monto.ToString("0.00") + " €",
                    FontSize = FontSizeMontos,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Right
                }
            );

            Grid GridMontoPagado = new Grid();
            GridMontoPagado.Children.Add(
                new TextBlock()
                {
                    Text = "Pagado",
                    FontSize = FontSizeMontos,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.Gray
                }
            );
            GridMontoPagado.Children.Add(
                new TextBlock()
                {
                    Text = Deuda.pagado.ToString("0.00") + " €",
                    FontSize = FontSizeMontos,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Right
                }
            );

            Grid GridMontoRestante = new Grid();
            GridMontoRestante.Children.Add(
                new TextBlock()
                {
                    Text = "Restante",
                    FontSize = FontSizeMontos,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.Black
                }
            );
            GridMontoRestante.Children.Add(
                new TextBlock()
                {
                    Text = (Deuda.monto - Deuda.pagado).ToString("0.00") + " €",
                    FontSize = FontSizeMontos,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Right
                }
            );

            PanelMontos.Children.Add(GridMontoInicial);
            PanelMontos.Children.Add(GridMontoPagado);
            PanelMontos.Children.Add(
                new Separator()
                {
                    Margin = new Thickness(0)
                }    
            );
            PanelMontos.Children.Add(GridMontoRestante);

            //Derecha Selección Monto


            StackPanel CuerpoInputs = new StackPanel()
            {
                Margin = new Thickness(5, 3, 5, 3)
            };
            CuerpoInputs.SetValue(Grid.ColumnProperty, 1);
            Cuerpo.Children.Add(CuerpoInputs);

            ComboBox SeleccionarCantidad = new ComboBox()
            {
                FontSize = 15,
                Width = 130,
                Padding = new Thickness(0),
                Margin = new Thickness(0, 0, 0, 3)
            };
            SeleccionarCantidad.Items.Add("Pagar Todo");
            SeleccionarCantidad.Items.Add("Pagar Cantidad");
            SeleccionarCantidad.SelectedIndex = 0;
            SeleccionarCantidad.SelectionChanged += new SelectionChangedEventHandler(ComboBox_SelectionChanged);
            CuerpoInputs.Children.Add(SeleccionarCantidad);

            StackPanel PanelInputCantidad = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                IsEnabled = false
            };

            TextBox textPrecio = new TextBox()
            {
                FontSize = 12,
                Text = (Deuda.monto - Deuda.pagado).ToString("0.00"),
                Width = 60,
                Padding = new Thickness(0),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Right,
                FontWeight = FontWeights.Bold
            };
            textPrecio.LostFocus += new RoutedEventHandler(TextPrecio_LostFocus);
            textPrecio.KeyDown += new KeyEventHandler(Validaciones.TextBoxValidatePrices);
            textPrecio.KeyDown += new KeyEventHandler(
                (s, t) =>
                {
                    if (t.Key == Key.Enter)
                    {
                        BtnAccept.Focus();
                    }
                }
            );
            PanelInputCantidad.Children.Add(
                textPrecio
            );
            PanelInputCantidad.Children.Add(
                new TextBlock()
                {
                    Text = "€",
                    FontSize = 15,
                    FontWeight = FontWeights.Bold,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(3, 0, 0, 0),
                    Foreground = Brushes.Gray
                }
            );
            CuerpoInputs.Children.Add(PanelInputCantidad);






            return MarcoPrincipal;
        }


        class ModeloDetallePago
        {

            public ModeloDetallePago()
            {

            }
            public ModeloDetallePago(DDeuda deuda, double pagando, bool enabled)
            {
                this.deuda = deuda;
                Pagando = pagando;
                Enabled = enabled;
            }

            public DDeuda deuda { get; set; }
            public double Pagando { get; set; }
            public bool Enabled { get; set; }
        }
        
    }
}
