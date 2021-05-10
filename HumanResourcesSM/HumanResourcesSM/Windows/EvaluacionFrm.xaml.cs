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
    public partial class EvaluacionFrm : Window
    {
        void init()
        {
            InitializeComponent();
            txtValorEvaluado.txt.KeyDown += new KeyEventHandler(Validaciones.TextBoxValidatePrices);
        }
        public EvaluacionFrm()
        {
            init();
        }
        public EvaluacionFrm(TypeForm type, MetaType mtype, DEvaluacion evaluacion)
        {
            init();

            Type = type;
            MType = mtype;
            DataFill = evaluacion;
        }

        public TypeForm Type;
        public MetaType MType;
        public DEvaluacion DataFill;

        public DEvaluacion UForm;

        public MEvaluacion Metodos = new MEvaluacion();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Type == TypeForm.Update)
                Update();
            else
                Create();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Type == TypeForm.Read)
            {
                txtTitulo.Text = "Ver";
                fillForm(DataFill);
                SetEnable(false);
                btnEnviar.Visibility = Visibility.Collapsed;
            }
            else if (Type == TypeForm.Update)
            {
                txtTitulo.Text = "Editar";
                BgTitulo.Background = (Brush)new BrushConverter().ConvertFrom("#2A347B");
                btnEnviar.Content = "Editar";
                btnEnviar.Foreground = (Brush)new BrushConverter().ConvertFrom("#2A347B");
                btnEnviar.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#2A347B");
                fillForm(DataFill);
            }
            else if (Type == TypeForm.Create)
            {
                RBEmpleado.IsChecked = true;
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

        void Create()
        {
            fillData();
            if (UForm == null)
                return;
            var resp = Metodos.Insertar(UForm);
            MessageBox.Show(resp);
            if (resp == "OK")
            {
                this.DialogResult = true;
                this.Close();
            }
        }
        void Update()
        {
            fillData();
            if (UForm == null)
                return;
            UForm.idEvaluacion = DataFill.idEvaluacion;
            var resp = Metodos.Editar(UForm);
            MessageBox.Show(resp);
            if (resp == "OK")
            {
                this.DialogResult = true;
                this.Close();
            }
        }
        void SetEnable(bool Enable)
        {
            BtnSeleccionarMeta.IsEnabled = Enable;
            txtValorEvaluado.IsEnabled = Enable;
            txtobservacion.IsEnabled = Enable;
        }
        void fillForm(DEvaluacion Data)
        {
            if (Data != null)
            {
                DMeta Meta = new DMeta();
                if(MType == MetaType.Departamento)
                {
                    Meta = new MMeta().EncontrarByDepartamento(Data.idMeta)[0];
                }
                else if(MType == MetaType.Empleado)
                {
                    Meta = new MMeta().EncontrarByEmpleado(Data.idMeta)[0];
                }
                SeleccionarMeta(Meta, MType == MetaType.Empleado);
                BtnSeleccionarMeta.Visibility = Visibility.Collapsed;
                txtValorEvaluado.SetText(Data.valorEvaluado.ToString());
                txtobservacion.SetText(Data.observacion.ToString());
            }
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
