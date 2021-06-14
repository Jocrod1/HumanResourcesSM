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

using Metodos;
using Datos;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace HumanResourcesSM.Windows
{
    /// <summary>
    /// Interaction logic for SeleccionarMenu.xaml
    /// </summary>
    public partial class DespidoVista : Window
    {
        public DespidoVista(int idempleado)
        {
            InitializeComponent();

            idEmpleado = idempleado;
            txtHorasTrabajadas.KeyDown += new KeyEventHandler(Validaciones.TextBox_KeyDown);
            txtHorasExtras.KeyDown += new KeyEventHandler(Validaciones.TextBox_KeyDown);
        }

        int idEmpleado;

        DEmpleado EmpleadoEntrevistado;
        DSeleccion EmpleadoSelEntrevistado;

        DContrato Contrato;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FetchEmpleado();
        }

        int AñosAntiguedad = 0, MesesAntiguedad = 0;

        void FetchEmpleado()
        {
            MSeleccion SelMetodo = new MSeleccion();

            var Empleado = SelMetodo.EncontrarEmpleado(idEmpleado)[0];

            var DatosSeleccion = SelMetodo.EncontrarSeleccion(Empleado.idEmpleado)[0];

            var contrato = new MContrato().Encontrar(Empleado.idEmpleado);

            if (contrato.Count > 0)
            {
                Contrato = contrato[0];
                TimeSpan yearsOld = (Empleado.fechaCulminacion ?? DateTime.Now) - contrato[0].fechaContratacion;
                int years = (int)(yearsOld.TotalDays / 365.25);
                int months = (int)(((yearsOld.TotalDays / 365.25) - years) * 12);

                AñosAntiguedad = years;
                MesesAntiguedad = months;

                string antiguedad = "";

                if (years > 0)
                {
                    antiguedad = years + " años " + (months > 0 ? months + "y  meses" : "");
                }
                else
                {
                    antiguedad = months + " meses";
                }

                txtAntiguedad.Text = antiguedad;
            }
            else
            {
                GridAntiguedad.Visibility = Visibility.Collapsed;
            }

            EmpleadoEntrevistado = Empleado;
            EmpleadoSelEntrevistado = DatosSeleccion;

            txtNombre.Text = Empleado.nombre;
            txtApellido.Text = Empleado.apellido;
            txtDireccion.Text = Empleado.direccion;

            if (!Regex.IsMatch(Empleado.curriculum, @"^https?:\/\/", RegexOptions.IgnoreCase))
                Empleado.curriculum = "http://" + Empleado.curriculum;

            UrlCurriculo.NavigateUri = new Uri(Empleado.curriculum);
            txtDocumento.Text = Empleado.cedula;
            txtFechaNac.Text = Empleado.fechaNacimiento.ToString();
            txtPaisNac.Text = Empleado.nacionalidad; // por cambiar, debe verse "España - ES"
            txtEstadoLegal.Text = Empleado.estadoLegal;
            txtEmail.Text = Empleado.email;
            txtTelf.Text = Empleado.telefono;


            txtNombrePosicion.Text = DatosSeleccion.nombrePuesto;
            txtDepartamento.Text = Empleado.idDepartamento.ToString(); // actualmente se está viendo el id y no el nombre del departamento
            txtFechaApl.Text = DatosSeleccion.fechaAplicacion.ToString();


            RefreshDGIdiomas();
            RefreshDGEducacion();
        }

        //***********************CRUD IDIOMAS*********************************

        public List<DIdiomaHablado> IdiomaHablado = new List<DIdiomaHablado>();
        public List<ModelIdiomaHablado> ModelHI = new List<ModelIdiomaHablado>();

        MIdiomaHablado methodIH = new MIdiomaHablado();

        public void RefreshDGIdiomas()
        {
            IdiomaHablado.Clear();
            ModelHI.Clear();

            

            var IdiomasHablados = methodIH.Mostrar(EmpleadoEntrevistado.idEmpleado);
            IdiomaHablado = IdiomasHablados;

            foreach(DIdiomaHablado item in IdiomasHablados)
            {
                var idi = methodIH.EncontrarIdioma(item.idIdioma)[0];
                ModelHI.Add(new ModelIdiomaHablado(idi.nombre, item.nivel));
            }

            IdiomaList.Children.Clear();

            for (int i = 0; i < IdiomaHablado.Count; i++)
            {
                IdiomaList.Children.Add(CreateRow(ModelHI[i], IdiomaHablado[i]));
            }


        }

        Border CreateRow(ModelIdiomaHablado Model, DIdiomaHablado DIH)
        {
            string Nivel = DIH.nivel == 0 ? "Básico" :
                           DIH.nivel == 1 ? "Intermedio" :
                           DIH.nivel == 2 ? "Avanzado" :
                           DIH.nivel == 3 ? "Fluido" : "ERROR";



            Border MainBord = new Border();
            MainBord.BorderBrush = Brushes.Black;
            MainBord.BorderThickness = new Thickness(0);

            Grid Divider = new Grid();
            Divider.ColumnDefinitions.Add(
                new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Auto)
                }
            );
            Divider.ColumnDefinitions.Add(
                new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                }
            );
            Divider.ColumnDefinitions.Add(
                new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Auto)
                }
            );


            StackPanel SPContent = new StackPanel();
            SPContent.SetValue(Grid.ColumnProperty, 1);
            SPContent.MaxHeight = 50;

            Divider.Children.Add(SPContent);

            TextBlock NameText = new TextBlock();
            NameText.Text = Model.nombreIdioma.ToUpper();
            NameText.MaxHeight = 35;
            NameText.TextWrapping = TextWrapping.Wrap;
            NameText.TextTrimming = TextTrimming.CharacterEllipsis;
            NameText.FontWeight = FontWeights.Bold;
            NameText.FontSize = 12;
            NameText.Margin = new Thickness(5, 0, 5, 0);

            SPContent.Children.Add(NameText);

            TextBlock LevelText = new TextBlock();
            LevelText.Text = Nivel;
            LevelText.FontSize = 10;
            LevelText.TextWrapping = TextWrapping.Wrap;
            LevelText.TextTrimming = TextTrimming.CharacterEllipsis;
            LevelText.Margin = new Thickness(5, 0, 5, 0);

            SPContent.Children.Add(LevelText);

            MainBord.Child = Divider;

            return MainBord;

        }

        //*********************** END CRUD IDIOMAS*********************************

        //***********************CRUD EDUCACIÓN*********************************

        public List<DEducacion> ListaEducacion = new List<DEducacion>();

        MEducacion methodEdu = new MEducacion();

        public void RefreshDGEducacion()
        {
            var Educacion = new MEducacion().Mostrar(EmpleadoEntrevistado.idEmpleado);
            ListaEducacion = Educacion;

            EducacionList.Children.Clear();

            for (int i = 0; i < ListaEducacion.Count; i++)
            {
                EducacionList.Children.Add(CreateRowEdu(ListaEducacion[i]));
            }


        }

        Border CreateRowEdu(DEducacion DED)
        {

            Border MainBord = new Border();
            MainBord.BorderBrush = Brushes.Black;
            MainBord.BorderThickness = new Thickness(0);

            Grid Divider = new Grid();
            Divider.ColumnDefinitions.Add(
                new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Auto)
                }
            );
            Divider.ColumnDefinitions.Add(
                new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                }
            );
            Divider.ColumnDefinitions.Add(
                new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Auto)
                }
            );


            StackPanel SPContent = new StackPanel();
            SPContent.SetValue(Grid.ColumnProperty, 1);
            SPContent.MaxHeight = 50;

            Divider.Children.Add(SPContent);

            TextBlock NameText = new TextBlock();
            NameText.Text = DED.nombreCarrera;
            NameText.MaxHeight = 35;
            NameText.TextWrapping = TextWrapping.Wrap;
            NameText.TextTrimming = TextTrimming.CharacterEllipsis;
            NameText.FontWeight = FontWeights.Bold;
            NameText.FontSize = 12;
            NameText.Margin = new Thickness(5, 0, 5, 0);

            SPContent.Children.Add(NameText);

            string egreso = DED.fechaEgreso == null ? "Actualidad" : DED.fechaEgreso?.Year.ToString();

            TextBlock DateText = new TextBlock();
            DateText.Text = DED.fechaIngreso.Year + " - " + egreso;
            DateText.FontSize = 10;
            DateText.TextWrapping = TextWrapping.Wrap;
            DateText.TextTrimming = TextTrimming.CharacterEllipsis;
            DateText.Margin = new Thickness(5, 0, 5, 0);

            SPContent.Children.Add(DateText);

            MainBord.Child = Divider;

            return MainBord;

        }

        //*********************** END CRUD EDUCACIÓN*********************************


        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Hyperlink hl = (Hyperlink)sender;
            string navigateUri = hl.NavigateUri.ToString();
            Process.Start(new ProcessStartInfo(navigateUri));
            e.Handled = true;
        }

        private void BtnFire_Click(object sender, RoutedEventArgs e)
        {
            ContratoFrm frm = new ContratoFrm(EmpleadoEntrevistado);
            bool resp = frm.ShowDialog() ?? false;

            if (resp)
                this.Close();
            
        }

        double SalariosPendientes = 0, Vacaciones = 0, Utilidades = 0,
                PrestacionesSociales = 0, Preavisto = 0, indemnización = 0, Total = 0;

        void CalcularLiquidación()
        {
            if (CbRazonDespido.SelectedIndex == -1)
            {
                return;
            }

            SalariosPendientes = Vacaciones = Utilidades = PrestacionesSociales =
                Preavisto = indemnización = Total = 0;


            //Calculo Horas trabajadas
            double SueldoBase = Contrato.sueldo * HorasTrabajadas;

            double SueldoExtra = Contrato.sueldo * 2 * HorasExtras;

            SalariosPendientes = SueldoBase + SueldoExtra;

            //Calculo de Vacaciones
            if(AñosAntiguedad > 0)
            {
                int DiasPagar = 15 + (1 * AñosAntiguedad);
                double pagoVacaciones = Contrato.sueldo * 8 * DiasPagar;


                int DiasBono = 7 + (1 * AñosAntiguedad);
                double bonoVacaciones = Contrato.sueldo * 8 * DiasBono;

                Vacaciones = pagoVacaciones + bonoVacaciones;
            }
            else
            {
                //No merece Vacaciones
                SlotVacaciones.Visibility = Visibility.Collapsed;
            }

            //Calculo de Utilidades

            double SueldoMensual = Contrato.sueldo * 8 * 30;

            TimeSpan yearsOld = DateTime.Today - new DateTime(DateTime.Today.AddYears(-1).Year, 12, 15);
            int Days = (int)yearsOld.TotalDays;

            double MontoaPagar = (SueldoMensual * Days) / 360;

            //double PagoUtilidades = double.Parse(txtPagoUtilidades.Text);

            //if(txt)

            Utilidades = MontoaPagar;

            //Prestaciones Sociales
            int years = AñosAntiguedad;
            int months = MesesAntiguedad;
            if (years > 0)
            {
                PrestacionesSociales = years * 30 * Contrato.sueldo * 8;
            }
            else
            {
                PrestacionesSociales = months * 5 * Contrato.sueldo * 8;
            }

            //indemnización
            if(CbRazonDespido.SelectedIndex == 1 || CbRazonDespido.SelectedIndex == 2)
            {
                SlotIndemnizacion.Visibility = Visibility.Collapsed;
                SlotPreaviso.Visibility = Visibility.Collapsed;

            }
            else
            {
                SlotIndemnizacion.Visibility = Visibility.Visible;
                SlotPreaviso.Visibility = Visibility.Visible;
                Preavisto = Contrato.sueldo * 8 * 30;
                indemnización = Contrato.sueldo * 9 * 30;
            }

            Total = SalariosPendientes + Vacaciones + Utilidades + PrestacionesSociales + Preavisto + indemnización;

            txtSueldosPendientes.Text = SalariosPendientes.ToString("0.00") + " €";
            txtVacaciones.Text = Vacaciones.ToString("0.00") + " €";
            txtUtilidades.Text = Utilidades.ToString("0.00") + " €";
            txtPrestaciones.Text = PrestacionesSociales.ToString("0.00") + " €";
            txtPreaviso.Text = Preavisto.ToString("0.00") + " €";
            txtIndemnización.Text = indemnización.ToString("0.00") + " €";
            txtTotal.Text = Total.ToString("0.00") + " €";

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
            CalcularLiquidación();
        }


        private void CbRazonDespido_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CalcularLiquidación();
        }

    }
}
