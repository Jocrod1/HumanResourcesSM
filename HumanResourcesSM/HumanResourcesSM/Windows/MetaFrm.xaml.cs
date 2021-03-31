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
    public partial class MetaFrm : Page
    {
        public MetaFrm()
        {
            InitializeComponent();
            txtValorMeta.txt.KeyDown += new KeyEventHandler(Validaciones.TextBoxValidatePrices);
        }
        public DMeta UForm;
        public MMeta Metodos = new MMeta();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Create();
        }

        void Create()
        {
            fillData();
            if (UForm == null)
                return;
            var resp = Metodos.Insertar(UForm);
            MessageBox.Show(resp);
            if (resp == "OK")
            {
                //LO QUE SE HARÁ
                Limpiar();
            }
        }

        void fillData()
        {
            if (Validate())
            {
                UForm = null;
                return;
            }


            int idSeleccion = (int)CbSeleccion.SelectedValue;

            int idEmpleado = RBEmpleado.IsChecked ?? false ? idSeleccion : 1;
            int idDepartamento = RBDepartamento.IsChecked ?? false ? idSeleccion : 1;
            int idTipoMetrica = (int)CbTipoMetrica.SelectedValue;
            double ValorMeta = double.Parse(txtValorMeta.txt.Text);
            DateTime FechaInicio = CbFechaInicio.SelectedDate ?? DateTime.Now;
            DateTime FechaFinal = CbFechaFinal.SelectedDate ?? DateTime.Now;


            UForm = new DMeta(0,
                              idTipoMetrica,
                              ValorMeta,
                              idEmpleado,
                              idDepartamento,
                              1,
                              FechaInicio,
                              FechaFinal,
                              Menu.ActUsuario.idUsuario);
        }

        void Limpiar()
        {
            UForm = null;

            CbSeleccion.SelectedIndex = -1;
            CbTipoMetrica.SelectedIndex = -1;
            txtValorMeta.SetText("");
            CbFechaInicio.SelectedDate = null;
            CbFechaFinal.SelectedDate = null;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            RBEmpleado.IsChecked = true;

            


        }

        private void RBEmpleado_Checked(object sender, RoutedEventArgs e)
        {
            MSeleccion MSel = new MSeleccion();
            var res = MSel.MostrarEmpleado("");

            foreach (DEmpleado item in res)
            {
                item.nombre = item.nombre + " " + item.apellido;
            }

            CbSeleccion.ItemsSource = res;
            CbSeleccion.DisplayMemberPath = "nombre";
            CbSeleccion.SelectedValuePath = "idEmpleado";

            CbSeleccion.SelectedIndex = -1;
            PlaceSeleccion.Text = "Seleccionar Empleado";
        }

        private void RBDepartamento_Checked(object sender, RoutedEventArgs e)
        {
            var res = new MDepartamento().Mostrar("");

            CbSeleccion.ItemsSource = res;
            CbSeleccion.DisplayMemberPath = "nombre";
            CbSeleccion.SelectedValuePath = "idDepartamento";

            CbSeleccion.SelectedIndex = -1;
            PlaceSeleccion.Text = "Seleccionar Departamento";
        }

        private void CbFechaInicio_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbFechaInicio.SelectedDate != null)
            {
                PlaceFechaInicio.Text = "";

                CbFechaFinal.DisplayDateStart = CbFechaInicio.SelectedDate?.Date;
            }
            else
            {
                PlaceFechaInicio.Text = "Fecha de Inicio";

                CbFechaFinal.DisplayDateStart = null;
            }
        }

        private void CbFechaFinal_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbFechaFinal.SelectedDate != null)
            {
                PlaceFechaFinal.Text = "";

                CbFechaInicio.DisplayDateEnd = CbFechaFinal.SelectedDate?.Date;
            }
            else
            {
                PlaceFechaFinal.Text = "Fecha Final";

                CbFechaInicio.DisplayDateEnd = null;
            }
        }

        private void CbSeleccion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbSeleccion.SelectedIndex > -1)
            {
                PlaceSeleccion.Visibility = Visibility.Hidden;

                

                int id = 0;
                if(RBEmpleado.IsChecked ?? false)
                {
                    var Empleado = (DEmpleado)CbSeleccion.SelectedItem;
                    id = Empleado.idDepartamento;
                }
                else
                {
                    id = (int)CbSeleccion.SelectedValue;
                }

                var resp = new MTipoMetrica().Mostrar(id);

                CbTipoMetrica.ItemsSource = resp;
                CbTipoMetrica.DisplayMemberPath = "nombre";
                CbTipoMetrica.SelectedValuePath = "idTipoMetrica";

                CbTipoMetrica.IsEnabled = true;

            }
            else
            {
                PlaceSeleccion.Visibility = Visibility.Visible;

                CbTipoMetrica.SelectedIndex = -1;
                CbTipoMetrica.IsEnabled = false;


            }
        }

        private void CbTipoMetrica_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbTipoMetrica.SelectedIndex > -1)
            {
                PlaceTipoMetrica.Text = "";
            }
            else
            {
                PlaceTipoMetrica.Text = "Tipo de Métrica";
            }
        }

        #region Validation
        bool Validate()
        {
            if (CbSeleccion.SelectedIndex == -1)
            {
                string Val = RBEmpleado.IsChecked ?? false ? "Empleado" : RBDepartamento.IsChecked ?? false ? "Departamento" : "[VALOR NULO]";
                MessageBox.Show("Debes Seleccionar un " + Val + "!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbSeleccion.Focus();
                return true;
            }
            if (CbTipoMetrica.SelectedIndex == -1)
            {
                MessageBox.Show("Debes Seleccionar un tipo de Métrica!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbTipoMetrica.Focus();
                return true;
            }
            if (txtValorMeta.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el campo Valor Meta!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtValorMeta.txt.Focus();
                return true;
            }
            if (CbFechaInicio.SelectedDate == null)
            {
                MessageBox.Show("Debes Seleccionar la Fecha de Inicio!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbFechaInicio.Focus();
                return true;
            }
            if (CbFechaFinal.SelectedDate == null)
            {
                MessageBox.Show("Debes Seleccionar la Fecha Final!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbFechaFinal.Focus();
                return true;
            }
            if (CbFechaInicio.SelectedDate?.Date > CbFechaFinal.SelectedDate?.Date)
            {
                MessageBox.Show("La Fecha de Inicio no puede ser después de la fecha Final!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbFechaInicio.Focus();
                return true;
            }

            return false;
        }


        #endregion
        
        
    }
}
