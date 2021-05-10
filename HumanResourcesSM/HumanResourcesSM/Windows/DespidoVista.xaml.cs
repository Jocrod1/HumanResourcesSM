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
        }

        int idEmpleado;

        DEmpleado EmpleadoEntrevistado;
        DSeleccion EmpleadoSelEntrevistado;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FetchEmpleado();
        }

        void FetchEmpleado()
        {
            MSeleccion SelMetodo = new MSeleccion();

            var Empleado = SelMetodo.EncontrarEmpleado(idEmpleado)[0];

            var DatosSeleccion = SelMetodo.EncontrarSeleccion(Empleado.idEmpleado)[0];

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
            var resp = new MSeleccion().Despido(idEmpleado);

            if (resp.Equals("OK"))
                this.Close();
            else
                MessageBox.Show(resp);
        }
    }
}
