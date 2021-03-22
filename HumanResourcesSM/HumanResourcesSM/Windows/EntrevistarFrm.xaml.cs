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
    public partial class EntrevistarFrm : Page
    {
        public EntrevistarFrm()
        {
            InitializeComponent();
        }

        DEmpleado EmpleadoEntrevistado;
        DSeleccion EmpleadoSelEntrevistado;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FetchEmpleado();
        }

        void FetchEmpleado()
        {
            MSeleccion SelMetodo = new MSeleccion();

            var Empleado = SelMetodo.EmpleadoEntrevista()[0];

            Console.WriteLine("Hola");

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

        public void InsertIdioma(DIdiomaHablado idiomaHablado)
        {
            idiomaHablado.idEmpleado = EmpleadoEntrevistado.idEmpleado;
            var resp = methodIH.Insertar(idiomaHablado);

            RefreshDGIdiomas();
        }

        public void EditIdioma(DIdiomaHablado idiomaHablado)
        {
            idiomaHablado.idEmpleado = EmpleadoEntrevistado.idEmpleado;
            var resp = methodIH.Editar(idiomaHablado);

            RefreshDGIdiomas();
        }

        public void DeleteIdioma(int id)
        {
            var resp = methodIH.Eliminar(id);

            RefreshDGIdiomas();
        }

        private void BtnAgregarIdioma_Click(object sender, RoutedEventArgs e)
        {
            IdiomaHabladoFrm Frm = new IdiomaHabladoFrm(this);

            Frm.ShowDialog();
        }

        private void BtnIdiDelete_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("ELIMINANDO" + ((Button)e.Source).CommandParameter);
            DeleteIdioma((int)((Button)e.Source).CommandParameter);
        }

        private void BtnIdiUpdate_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("ACTUALIZANDO" + ((Button)e.Source).CommandParameter);

            DIdiomaHablado resp = methodIH.Encontrar((int)((Button)e.Source).CommandParameter)[0];

            IdiomaHabladoFrm frm = new IdiomaHabladoFrm(this);
            frm.Type = TypeForm.Update;
            frm.DataFill = resp;
            frm.ShowDialog();
        }

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

            Button DeleteButton = new Button();
            DeleteButton.SetValue(Grid.ColumnProperty, 0);
            DeleteButton.Background = DeleteButton.BorderBrush = Brushes.Transparent;
            DeleteButton.Margin = new Thickness(10, 2, 10, 2);
            DeleteButton.Padding = new Thickness(0);
            DeleteButton.Height = 17;
            Image delImg = new Image();
            string packUri = @"/Img/delete.png";
            delImg.Source = new BitmapImage(new Uri(packUri, UriKind.Relative));
            RenderOptions.SetBitmapScalingMode(delImg, BitmapScalingMode.HighQuality);
            DeleteButton.Click += new RoutedEventHandler(BtnIdiDelete_Click);
            DeleteButton.CommandParameter = DIH.idIdiomaHablado;
            DeleteButton.Content = delImg;

            Divider.Children.Add(DeleteButton);


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


            Button EditButton = new Button();
            EditButton.SetValue(Grid.ColumnProperty, 2);
            EditButton.Background = EditButton.BorderBrush = Brushes.Transparent;
            EditButton.Margin = new Thickness(10, 3, 10, 3);
            EditButton.Padding = new Thickness(0);
            EditButton.Height = 17;
            Image editImg = new Image();
            string packUri2 = @"/Img/edit.png";
            editImg.Source = new BitmapImage(new Uri(packUri2, UriKind.Relative));
            EditButton.Click += new RoutedEventHandler(BtnIdiUpdate_Click);
            EditButton.CommandParameter = DIH.idIdiomaHablado;
            EditButton.Content = editImg;

            Divider.Children.Add(EditButton);

            MainBord.Child = Divider;

            return MainBord;

        }

        //*********************** END CRUD IDIOMAS*********************************

        //***********************CRUD EDUCACIÓN*********************************

        public List<DEducacion> ListaEducacion = new List<DEducacion>();

        MEducacion methodEdu = new MEducacion();

        public void InsertEducacion(DEducacion Edu)
        {
            Edu.idEmpleado = EmpleadoEntrevistado.idEmpleado;
            var resp = methodEdu.Insertar(Edu);

            RefreshDGEducacion();
        }

        public void EditEducacion(DEducacion Edu)
        {
            Edu.idEmpleado = EmpleadoEntrevistado.idEmpleado;
            var resp = methodEdu.Editar(Edu);

            RefreshDGEducacion();
        }

        public void DeleteEducacion(int id)
        {
            var resp = methodEdu.Eliminar(id);

            RefreshDGEducacion();
        }

        private void BtnAgregarEducacion_Click(object sender, RoutedEventArgs e)
        {
            EducacionFrm Frm = new EducacionFrm(this);

            Frm.ShowDialog();
        }

        private void BtnEduDelete_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("ELIMINANDO" + ((Button)e.Source).CommandParameter);
            DeleteEducacion((int)((Button)e.Source).CommandParameter);
        }

        private void BtnEduUpdate_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("ACTUALIZANDO" + ((Button)e.Source).CommandParameter);

            DEducacion resp = methodEdu.Encontrar((int)((Button)e.Source).CommandParameter)[0];

            EducacionFrm frm = new EducacionFrm(this);
            frm.Type = TypeForm.Update;
            frm.DataFill = resp;
            frm.ShowDialog();
        }

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

            Button DeleteButton = new Button();
            DeleteButton.SetValue(Grid.ColumnProperty, 0);
            DeleteButton.Background = DeleteButton.BorderBrush = Brushes.Transparent;
            DeleteButton.Margin = new Thickness(10, 2, 10, 2);
            DeleteButton.Padding = new Thickness(0);
            DeleteButton.Height = 17;
            Image delImg = new Image();
            string packUri = @"/Img/delete.png";
            delImg.Source = new BitmapImage(new Uri(packUri, UriKind.Relative));
            RenderOptions.SetBitmapScalingMode(delImg, BitmapScalingMode.HighQuality);
            DeleteButton.Click += new RoutedEventHandler(BtnEduDelete_Click);
            DeleteButton.CommandParameter = DED.idEducacion;
            DeleteButton.Content = delImg;
            DeleteButton.Foreground = Brushes.Black;

            Divider.Children.Add(DeleteButton);

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


            Button EditButton = new Button();
            EditButton.SetValue(Grid.ColumnProperty, 3);
            EditButton.Background = EditButton.BorderBrush = Brushes.Transparent;
            EditButton.Margin = new Thickness(10, 3, 10, 3);
            EditButton.Padding = new Thickness(0);
            EditButton.Height = 23;
            Image editImg = new Image();
            string packUri2 = @"/Img/edit.png";
            editImg.Source = new BitmapImage(new Uri(packUri2, UriKind.Relative));
            EditButton.Click += new RoutedEventHandler(BtnEduUpdate_Click);
            EditButton.CommandParameter = DED.idEducacion;
            EditButton.Content = editImg;
            EditButton.Foreground = Brushes.Black;

            Divider.Children.Add(EditButton);

            MainBord.Child = Divider;

            return MainBord;

        }

        //*********************** END CRUD EDUCACIÓN*********************************

        private void BtnInfoUpdate_Click(object sender, RoutedEventArgs e)
        {
            SeleccionVista vista = new SeleccionVista(this, EmpleadoEntrevistado, EmpleadoSelEntrevistado);
            vista.ShowDialog();
            FetchEmpleado();
        }



        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAccept_Click(object sender, RoutedEventArgs e)
        {
            ContratoFrm frm = new ContratoFrm(this);
            frm.ShowDialog();
        }

        public void RegistrarContrato(DContrato contrato)
        {
            DContrato Data = new DContrato(0,
                                           EmpleadoEntrevistado.idEmpleado,
                                           DateTime.Now,
                                           EmpleadoSelEntrevistado.nombrePuesto,
                                           contrato.fechaCulminacion,
                                           contrato.sueldo,
                                           contrato.horasSemanales);

            var resp = new MContrato().Insertar(Data);
            MessageBox.Show(resp);
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Hyperlink hl = (Hyperlink)sender;
            string navigateUri = hl.NavigateUri.ToString();
            Process.Start(new ProcessStartInfo(navigateUri));
            e.Handled = true;
        }
    }
}
