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
    public partial class EvaluacionFrm : Page
    {
        public EvaluacionFrm()
        {
            InitializeComponent();
            txtValorEvaluado.txt.KeyDown += new KeyEventHandler(Validaciones.TextBoxValidatePrices);
        }
        public DEvaluacion UForm;
        public MEvaluacion Metodos = new MEvaluacion();

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

            int idMeta = Meta.idMeta;
            double ValorEvaluado = double.Parse(txtValorEvaluado.txt.Text);
            string observaciones = txtobservacion.txt.Text;


            UForm = new DEvaluacion(0,
                                    Menu.ActUsuario.idUsuario,
                                    idMeta,
                                    ValorEvaluado,
                                    observaciones,
                                    1,
                                    DateTime.Now);
        }

        void Limpiar()
        {
            UForm = null;

            Meta = null;
            MetaSelected = false;
            BordMeta.Visibility = Visibility.Collapsed;
            txtTipoMetrica.Visibility = Visibility.Collapsed;
            txtValorEvaluado.SetText("");
            txtobservacion.SetText("");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
           

        }

        private void BtnSeleccionarMeta_Click(object sender, RoutedEventArgs e)
        {
            MetaVista Frm = new MetaVista(this);
            Frm.ShowDialog();
        }

        DMeta Meta = null;
        bool MetaSelected = false;

        public void SeleccionarMeta(DMeta meta, bool isEmpleado)
        {
            Meta = meta;
            MetaSelected = true;
            BordMeta.Visibility = Visibility.Visible;
            txtTipoMetrica.Visibility = Visibility.Visible;

            if(!isEmpleado)
            {
                RBDepartamento.IsChecked = true;
                RBEmpleado.IsChecked = false;

                txtEmpleado.Visibility = Visibility.Collapsed;

                txtDepartamento.Text = "Departamento: " + meta.departamento;
            }
            else{
                RBEmpleado.IsChecked = true;
                RBDepartamento.IsChecked = false;

                txtEmpleado.Visibility = Visibility.Visible;

                txtEmpleado.Text = "Empleado: " + meta.empleado;
                txtDepartamento.Text = "Departamento: " + meta.departamento;
            }

            txtValorMeta.Text = "Valor Meta: " + meta.valorMeta;
            txtTipoMetrica.Text = "Métricas: " + meta.nombreMetrica;

        }

        #region Validation
        bool Validate()
        {
            if (!MetaSelected)
            {
                MessageBox.Show("Debe Seleccionar una meta a Evaluar!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                BtnSeleccionarMeta.Focus();
                return true;
            }
            if (txtValorEvaluado.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el campo Valor Evaluado!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtValorEvaluado.txt.Focus();
                return true;
            }


            return false;
        }



        #endregion

       
    }
}
