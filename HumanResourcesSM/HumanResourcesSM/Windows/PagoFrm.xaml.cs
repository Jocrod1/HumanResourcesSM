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
        }

        private void BtnSeleccionarEmpleado_Click(object sender, RoutedEventArgs e)
        {
            PagoSeleccionVista Frm = new PagoSeleccionVista(this);
            Frm.ShowDialog();
        }

        private void BtnAccept_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        //Refresh Bonificaciones y Deducciones

        List<DDeuda> Deudas = new List<DDeuda>();

        void RefreshDeuda()
        {;
            RefreshBonificaciones(Deudas.FindAll(x => x.tipoDeuda == 0));
            RefreshDeducciones(Deudas.FindAll(x => x.tipoDeuda == 1));
        }

        void RefreshBonificaciones(List<DDeuda> Bonificaciones)
        {
            foreach(DDeuda bonificacion in Bonificaciones)
            {
                StackBonificaciones.Children.Add(ConstruirDeuda(bonificacion));
            }
        }

        void RefreshDeducciones(List<DDeuda> Deducciones)
        {
            foreach (DDeuda deduccion in Deducciones)
            {
                StackDeducciones.Children.Add(ConstruirDeuda(deduccion));
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chB = (CheckBox)sender;
            if(chB.IsChecked ?? false)
            {
                var parent = VisualTreeHelper.GetParent(chB) as Grid;
                var Title = parent.Children[1] as TextBlock;
                Title.Foreground = Brushes.Gray;
            }
            else
            {
                var parent = VisualTreeHelper.GetParent(chB) as Grid;
                var Title = parent.Children[1] as TextBlock;
                Title.Foreground = Brushes.Black;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox CB = (ComboBox)sender;
            if(CB.SelectedIndex == 0)
            {
                var parent = VisualTreeHelper.GetParent(CB) as StackPanel;
                var PanelInput = parent.Children[1] as StackPanel;
                PanelInput.IsEnabled = false;
                var MoneyText = PanelInput.Children[1] as TextBlock;
                MoneyText.Foreground = Brushes.Gray;
            }
            else
            {
                var parent = VisualTreeHelper.GetParent(CB) as StackPanel;
                var PanelInput = parent.Children[1] as StackPanel;
                PanelInput.IsEnabled = true;
                var MoneyText = PanelInput.Children[1] as TextBlock;
                MoneyText.Foreground = Brushes.Black;
            }
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
                Margin = new Thickness(10, 5, 10, 5)
            });
            MarcoTitulo.Child = Titulo;

            //Seccion Inferior

            Grid Cuerpo = new Grid();
            Cuerpo.ColumnDefinitions.Add(
                new ColumnDefinition()
                {
                    Width = new GridLength(100, GridUnitType.Pixel)
                }
            );
            Cuerpo.ColumnDefinitions.Add(
                new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                }
            );

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


            Grid GridMontoInicial = new Grid();
            GridMontoInicial.Children.Add(
                new TextBlock()
                {
                    Text = "Inicial",
                    FontSize = 8,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.Gray
                }
            );
            GridMontoInicial.Children.Add(
                new TextBlock()
                {
                    Text = Deuda.monto.ToString("0.00") + " €",
                    FontSize = 8,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Right
                }
            );

            Grid GridMontoPagado = new Grid();
            GridMontoPagado.Children.Add(
                new TextBlock()
                {
                    Text = "Pagado",
                    FontSize = 8,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.Gray
                }
            );
            GridMontoPagado.Children.Add(
                new TextBlock()
                {
                    Text = Deuda.pagado.ToString("0.00") + " €",
                    FontSize = 8,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Right
                }
            );

            Grid GridMontoRestante = new Grid();
            GridMontoRestante.Children.Add(
                new TextBlock()
                {
                    Text = "Restante",
                    FontSize = 8,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.Black
                }
            );
            GridMontoRestante.Children.Add(
                new TextBlock()
                {
                    Text = (Deuda.monto - Deuda.pagado).ToString("0.00") + " €",
                    FontSize = 8,
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
                FontSize = 9,
                Height = 20,
                Width = 90,
                Padding = new Thickness(0),
                Margin = new Thickness(0, 0, 0, 3)
            };
            SeleccionarCantidad.Items.Add("Pagar Todo");
            SeleccionarCantidad.Items.Add("Pagar Cantidad");
            SeleccionarCantidad.SelectionChanged += new SelectionChangedEventHandler(ComboBox_SelectionChanged);
            CuerpoInputs.Children.Add(SeleccionarCantidad);

            StackPanel PanelInputCantidad = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                IsEnabled = false
            };
            PanelInputCantidad.Children.Add(
                new TextBox()
                {
                    FontSize = 9,
                    Text = (Deuda.monto - Deuda.pagado).ToString("0.00"),
                    Height = 19,
                    Width = 60,
                    Padding = new Thickness(0),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Right,
                    FontWeight = FontWeights.Bold
                }    
            );
            PanelInputCantidad.Children.Add(
                new TextBlock()
                {
                    Text = "€",
                    FontSize = 14,
                    FontWeight = FontWeights.Bold,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(3, 0, 0, 0),
                    Foreground = Brushes.Gray
                }
            );
            CuerpoInputs.Children.Add(PanelInputCantidad);






            return MarcoPrincipal;
        }

        
    }
}
