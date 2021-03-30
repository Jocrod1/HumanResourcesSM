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
using System.Globalization;

using Datos;
using Metodos;

namespace HumanResourcesSM.Windows
{
    /// <summary>
    /// Interaction logic for SeleccionFrm.xaml
    /// </summary>
    public partial class SeleccionFrm : Page
    {
        public SeleccionFrm()
        {
            InitializeComponent();
        }

        public TypeForm Type = TypeForm.Create;
        public FormSeleccion DataFill;

        public FormSeleccion UForm;
        public MSeleccion Metodos = new MSeleccion();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Create();
        }

        void Create()
        {
            fillData();
            if (UForm == null)
                return;
            var resp = Metodos.Insertar(UForm.empleado,
                                        UForm.seleccion,
                                        UForm.Idiomas,
                                        UForm.Educacion);
            MessageBox.Show(resp);
            if(resp == "OK")
            {
                //LO QUE SE HARÁ
            }
        }

        void fillData()
        {
            if (Validate())
            {
                UForm = null;
                return;
            }

            /*         EMPLEADO           */
            int idDepartamento = (int)CbDepartamento.SelectedValue;
            string nombre = txtNombre.txt.Text;
            string apellido = txtApellido.txt.Text;
            string DNI = txtDNI.txt.Text;
            DateTime fechaNacimiento = CbFechaNac.SelectedDate ?? DateTime.Now;
            if (fechaNacimiento == DateTime.Now)return;
            string nacionalidad = (string)CbPaisNac.SelectedValue;
            string direccion = txtDireccion.txt.Text;
            string email = txtEmail.txt.Text;
            string telefono = txtTelefono.txt.Text;
            string URLCV = txtURLCV.txt.Text;
            string estadoLegal = CbEstadoLegal.Text;
            string nombrePuesto = txtNombrePuesto.txt.Text;
            /*         END EMPLEADO           */

            /*         SELECCIÓN           */
            int idSeleccionador = Menu.ActUsuario.idUsuario;
            DateTime fechaAplicacion = CbFechaApl.SelectedDate ?? DateTime.Now;
            if (fechaAplicacion == DateTime.Now) return;
            /*         END SELECCIÓN           */



            DEmpleado Empleado = new DEmpleado(0,
                                                idDepartamento,
                                                nombre,
                                                apellido,
                                                DNI,
                                                fechaNacimiento,
                                                nacionalidad,
                                                direccion,
                                                email,
                                                telefono,
                                                URLCV,
                                                estadoLegal,
                                                0);

            DSeleccion Seleccion = new DSeleccion(0,
                                                    0,
                                                    0,
                                                    idSeleccionador,
                                                    fechaAplicacion,
                                                    0,
                                                    DateTime.Now,
                                                    nombrePuesto);

            UForm = new FormSeleccion(Empleado, Seleccion, IdiomaHablado, ListaEducacion);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MSeleccion Metd = new MSeleccion();

            var resp = Metd.MostrarPaises("");

            CbPaisNac.ItemsSource = resp;
            CbPaisNac.DisplayMemberPath = "Pais";
            CbPaisNac.SelectedValuePath = "Codigo";

            var resp2 = new MDepartamento().Mostrar("");

            CbDepartamento.ItemsSource = resp2;
            CbDepartamento.DisplayMemberPath = "nombre";
            CbDepartamento.SelectedValuePath = "idDepartamento";

            var resp3 = new MTipoTramite().MostrarStatus();

            CbEstadoLegal.ItemsSource = resp3;
            CbEstadoLegal.DisplayMemberPath = "statusCambio";
            CbEstadoLegal.SelectedValuePath = "statusCambio";

            RefreshDGIdiomas();
            RefreshDGEducacion();
        }

        //***********************CRUD IDIOMAS*********************************

        public List<DIdiomaHablado> IdiomaHablado = new List<DIdiomaHablado>();
        public List<ModelIdiomaHablado> ModelHI = new List<ModelIdiomaHablado>();

        public void InsertIdioma(DIdiomaHablado idiomaH)
        {
            var Idi = new MIdiomaHablado().EncontrarIdioma(idiomaH.idIdioma)[0];

            //idiomaH.idIdiomaHablado = IdiomaHablado.Count > 0 ?
            //                            IdiomaHablado[IdiomaHablado.Count - 1].idIdiomaHablado + 1 : 1;

            IdiomaHablado.Add(idiomaH);

            ModelHI.Add(new ModelIdiomaHablado(Idi.nombre, idiomaH.nivel));

            RefreshDGIdiomas();
        }

        public void EditIdioma(DIdiomaHablado idiomaHablado, int id)
        {
            var Idi = new MIdiomaHablado().EncontrarIdioma(idiomaHablado.idIdioma)[0];

            IdiomaHablado[id] = idiomaHablado;

            ModelHI[id] = new ModelIdiomaHablado(Idi.nombre, idiomaHablado.nivel);

            RefreshDGIdiomas();
        }

        public void DeleteIdioma(int id)
        {
            IdiomaHablado.RemoveAt(id);
            ModelHI.RemoveAt(id);

            RefreshDGIdiomas();
        }

        private void BtnAgregarIdioma_Click(object sender, RoutedEventArgs e)
        {
            IdiomaHabladoFrm Frm = new IdiomaHabladoFrm(this);

            Frm.ShowDialog();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("ELIMINANDO" + ((Button)e.Source).CommandParameter);
            DeleteIdioma((int)((Button)e.Source).CommandParameter);
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("ACTUALIZANDO" + ((Button)e.Source).CommandParameter);

            DIdiomaHablado resp = IdiomaHablado[(int)((Button)e.Source).CommandParameter];

            IdiomaHabladoFrm frm = new IdiomaHabladoFrm(this);
            frm.Type = TypeForm.Update;
            frm.DataFill = resp;
            frm.ShowDialog();
        }

        public void RefreshDGIdiomas()
        {

            IdiomaList.Children.Clear();

            for (int i = 0; i < IdiomaHablado.Count; i++)
            {
                IdiomaHablado[i].idIdiomaHablado = i;
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
            DeleteButton.Height = 23;
            Image delImg = new Image();
            string packUri = @"/Img/delete.png";
            delImg.Source = new BitmapImage(new Uri(packUri, UriKind.Relative));
            RenderOptions.SetBitmapScalingMode(delImg, BitmapScalingMode.HighQuality);
            DeleteButton.Click += new RoutedEventHandler(BtnDelete_Click);
            DeleteButton.CommandParameter = DIH.idIdiomaHablado;
            DeleteButton.Content = delImg;
            DeleteButton.Foreground = Brushes.Black;

            Divider.Children.Add(DeleteButton);



            TextBlock NameText = new TextBlock();
            NameText.SetValue(Grid.ColumnProperty, 1);
            NameText.Text = Model.nombreIdioma.ToUpper() + " - " + Nivel;
            NameText.VerticalAlignment = VerticalAlignment.Center;
            NameText.FontWeight = FontWeights.Bold;
            NameText.FontSize = 14;
            NameText.Margin = new Thickness(5, 0, 5, 0);

            Divider.Children.Add(NameText);



            Button EditButton = new Button();
            EditButton.SetValue(Grid.ColumnProperty, 2);
            EditButton.Background = EditButton.BorderBrush = Brushes.Transparent;
            EditButton.Margin = new Thickness(10, 3, 10, 3);
            EditButton.Padding = new Thickness(0);
            EditButton.Height = 23;
            Image editImg = new Image();
            string packUri2 = @"/Img/edit.png";
            editImg.Source = new BitmapImage(new Uri(packUri2, UriKind.Relative));
            EditButton.Click += new RoutedEventHandler(BtnUpdate_Click);
            EditButton.CommandParameter = DIH.idIdiomaHablado;
            EditButton.Content = editImg;
            EditButton.Foreground = Brushes.Black;

            Divider.Children.Add(EditButton);

            MainBord.Child = Divider;

            return MainBord;

        }

        //*********************** END CRUD IDIOMAS*********************************


        //***********************CRUD EDUCACIÓN*********************************

        public List<DEducacion> ListaEducacion = new List<DEducacion>();

        public void InsertEducacion(DEducacion Edu)
        {
            ListaEducacion.Add(Edu);


            RefreshDGEducacion();
        }

        public void EditEducacion(DEducacion Edu, int id)
        {
            ListaEducacion[id] = Edu;


            RefreshDGEducacion();
        }

        public void DeleteEducacion(int id)
        {
            ListaEducacion.RemoveAt(id);

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

            DEducacion resp = ListaEducacion[(int)((Button)e.Source).CommandParameter];

            EducacionFrm frm = new EducacionFrm(this);
            frm.Type = TypeForm.Update;
            frm.DataFill = resp;
            frm.ShowDialog();
        }

        public void RefreshDGEducacion()
        {

            EducacionList.Children.Clear();

            for (int i = 0; i < ListaEducacion.Count; i++)
            {
                ListaEducacion[i].idEducacion = i;
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
                    Width = new GridLength(150, GridUnitType.Pixel)
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
            DeleteButton.Height = 23;
            Image delImg = new Image();
            string packUri = @"/Img/delete.png";
            delImg.Source = new BitmapImage(new Uri(packUri, UriKind.Relative));
            RenderOptions.SetBitmapScalingMode(delImg, BitmapScalingMode.HighQuality);
            DeleteButton.Click += new RoutedEventHandler(BtnEduDelete_Click);
            DeleteButton.CommandParameter = DED.idEducacion;
            DeleteButton.Content = delImg;
            DeleteButton.Foreground = Brushes.Black;

            Divider.Children.Add(DeleteButton);



            TextBlock NameText = new TextBlock();
            NameText.SetValue(Grid.ColumnProperty, 1);
            NameText.Text = DED.nombreCarrera;
            NameText.TextTrimming = TextTrimming.CharacterEllipsis;
            NameText.VerticalAlignment = VerticalAlignment.Center;
            NameText.FontWeight = FontWeights.Bold;
            NameText.FontSize = 14;
            NameText.Margin = new Thickness(5, 0, 5, 0);

            Divider.Children.Add(NameText);

            string egreso = DED.fechaEgreso == null ? "Actualidad" : DED.fechaEgreso?.Year.ToString();

            TextBlock DateText = new TextBlock();
            DateText.SetValue(Grid.ColumnProperty, 2);
            DateText.Text = DED.fechaIngreso.Year + " - " + egreso;
            DateText.VerticalAlignment = VerticalAlignment.Center;
            DateText.FontWeight = FontWeights.Bold;
            DateText.FontSize = 14;
            DateText.Margin = new Thickness(5, 0, 5, 0);

            Divider.Children.Add(DateText);



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
        private void CbDepartamento_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbDepartamento.SelectedIndex > -1)
            {
                PlaceDepartamento.Text = "";
            }
            else
            {
                PlaceDepartamento.Text = "Departamento";
            }
        }

        private void CbFechaNac_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbFechaNac.SelectedDate != null)
            {
                PlaceFechaNac.Text = "";
            }
            else
            {
                PlaceFechaNac.Text = "Fecha de Nacimiento";
            }
        }

        private void CbPaisNac_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbPaisNac.SelectedIndex > -1)
            {
                PlacePaisNac.Text = "";
            }
            else
            {
                PlacePaisNac.Text = "País de Nacimiento";
            }
        }

        private void CbEstadoLegal_GotFocus(object sender, RoutedEventArgs e)
        {
            if(CbEstadoLegal.Text == "")
            {
                PlaceEstadoLegal.Visibility = Visibility.Collapsed;
            }
        }

        private void CbEstadoLegal_LostFocus(object sender, RoutedEventArgs e)
        {
            if (CbEstadoLegal.Text == "")
            {
                PlaceEstadoLegal.Visibility = Visibility.Visible;
            }
        }

        private void CbFechaApl_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbFechaApl.SelectedDate != null)
            {
                PlaceFechaApl.Text = "";
            }
            else
            {
                PlaceFechaApl.Text = "Fecha de Aplicación";
            }
        }

        public class FormSeleccion
        {
            public FormSeleccion(DEmpleado empleado, DSeleccion seleccion, List<DIdiomaHablado> idiomas, List<DEducacion> educacion)
            {
                this.empleado = empleado;
                this.seleccion = seleccion;
                Idiomas = idiomas;
                Educacion = educacion;
            }

            public FormSeleccion()
            {

            }

            public DEmpleado empleado { get; set; }
            public DSeleccion seleccion { get; set; }
            public List<DIdiomaHablado> Idiomas { get; set; }
            public List<DEducacion> Educacion { get; set; }
        }        

        #region Validation
        bool Validate()
        {
            if (txtNombre.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el campo Nombre!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtNombre.txt.Focus();
                return true;
            }
            if (txtApellido.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el campo Apellido!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtApellido.txt.Focus();
                return true;
            }
            if (txtDNI.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el campo DNI!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtDNI.txt.Focus();
                return true;
            }
            if (CbPaisNac.SelectedIndex == -1)
            {
                MessageBox.Show("Debes Seleccionar el Pais de Nacimiento!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbPaisNac.Focus();
                return true;
            }
            if (CbFechaNac.SelectedDate == null)
            {
                MessageBox.Show("Debes Seleccionar la Fecha de Nacimiento!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbFechaNac.Focus();
                return true;
            }
            if (txtEmail.txt.Text == "" && txtTelefono.txt.Text == "")
            {
                MessageBox.Show("Debes llenar al menos un campo de Email o Telefono!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtEmail.txt.Focus();
                return true;
            }
            if (CbEstadoLegal.Text == "")
            {
                MessageBox.Show("Debes llenar el campo Estado Legal!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbEstadoLegal.Focus();
                return true;
            }
            if (CbDepartamento.SelectedIndex == -1)
            {
                MessageBox.Show("Debes Seleccionar el Departamento!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbDepartamento.Focus();
                return true;
            }
            if (txtNombrePuesto.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el campo de Nombre la Posición!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtNombrePuesto.txt.Focus();
                return true;
            }
            if (CbFechaApl.SelectedDate == null)
            {
                MessageBox.Show("Debes Seleccionar la Fecha de Aplicación!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbFechaApl.Focus();
                return true;
            }
            if (txtURLCV.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el campo de URL del Currículo!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtURLCV.txt.Focus();
                return true;
            }

            return false;
        }


        #endregion

    }

    public class ModelIdiomaHablado
    {
        public ModelIdiomaHablado(string nombreIdioma, int nivel)
        {
            this.nombreIdioma = nombreIdioma;
            this.nivel = nivel;
        }

        public string nombreIdioma { get; set; }
        public int nivel { get; set; }
    }
}
