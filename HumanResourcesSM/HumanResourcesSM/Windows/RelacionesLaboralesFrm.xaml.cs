using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Datos;
using Metodos;

namespace HumanResourcesSM.Windows
{

    /// <summary>
    /// Interaction logic for DepartamentoFrm.xaml
    /// </summary>
    public partial class RelacionesLaboralesFrm : Window
    {
        public RelacionesLaboralesFrm()
        {
            InitializeComponent();

            MSeleccion MSel = new MSeleccion();
            var res = MSel.MostrarEmpleado("");

            foreach(DEmpleado item in res)
            {
                item.nombre = item.nombre + " " + item.apellido;
            }

            CbEmpleado.ItemsSource = res;
            CbEmpleado.DisplayMemberPath = "nombre";
            CbEmpleado.SelectedValuePath = "idEmpleado";



            MTipoTramite MTTR = new MTipoTramite();
            var resp = MTTR.Mostrar("");

            CbTipoTramite.ItemsSource = resp;
            CbTipoTramite.DisplayMemberPath = "nombre";
            CbTipoTramite.SelectedValuePath = "idTipoTramite";
        }


        public TypeForm Type;
        public DRelacionesLaborales DataFill;

        public DRelacionesLaborales UForm;

        public MRelacionesLaborales Metodos = new MRelacionesLaborales();

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
        }



        void fillData()
        {
            if (Validate())
            {
                UForm = null;
                return;
            }
            int idEmpleado = (int)CbEmpleado.SelectedValue;
            int idTipoTramite = (int)CbTipoTramite.SelectedValue;
            DateTime FechaTramite = DPFechaTramite.SelectedDate ?? DateTime.Now;
            string Url = txtUrl.txt.Text;

            UForm = new DRelacionesLaborales(0,
                                             idEmpleado,
                                             idTipoTramite,
                                             FechaTramite,
                                             Url);
        }

        void Create()
        {
            fillData();
            if (UForm == null)
                return;
            string response = Metodos.Insertar(UForm);
            MessageBox.Show(response);
            if (response == "OK")
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
            UForm.idTipoTramite = DataFill.idTipoTramite;
            //string response = Metodos.Editar(UForm);
            //MessageBox.Show(response);
            //if (response == "OK")
            //{
            //    this.DialogResult = true;
            //    this.Close();
            //}
        }

        void SetEnable(bool Enable)
        {
            CbEmpleado.IsEnabled = Enable;
            CbTipoTramite.IsEnabled = Enable;
            DPFechaTramite.IsEnabled = Enable;
            txtUrl.IsEnabled = Enable;
        }
        void fillForm(DRelacionesLaborales Data)
        {
            if (Data != null)
            {
                CbEmpleado.SelectedValue = Data.idEmpleado;
                CbTipoTramite.SelectedValue = Data.idTipoTramite;
                txtUrl.SetText(Data.documentoUrl);
            }
        }
        #region Validation
        bool Validate()
        {

            if (CbEmpleado.SelectedIndex == -1)
            {
                MessageBox.Show("Debes seleccionar un Empleado!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                CbEmpleado.Focus();
                return true;
            }

            if (CbTipoTramite.SelectedIndex == -1)
            {
                MessageBox.Show("Debes seleccionar un Rol!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                CbTipoTramite.Focus();
                return true;
            }

            if (DPFechaTramite.SelectedDate == null)
            {
                MessageBox.Show("Debes seleccionar una Fecha de Tramite!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                DPFechaTramite.Focus();
                return true;
            }

            if (txtUrl.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el campo URL!", "Magicolor", MessageBoxButton.OK, MessageBoxImage.Error);
                txtUrl.txt.Focus();
                return true;
            }

            return false;
        }
        #endregion

        private void CbEmpleado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbEmpleado.SelectedIndex > -1)
            {
                PlaceEmpleado.Text = "";
            }
            else
            {
                PlaceEmpleado.Text = "Empleado";
            }
        }

        private void CbTipoTramite_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbTipoTramite.SelectedIndex > -1)
            {
                PlaceTipoTramite.Text = "";
            }
            else
            {
                PlaceTipoTramite.Text = "Tipo de Tramite";
            }
        }

        private void DPFechaTramite_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DPFechaTramite.SelectedDate != null)
            {
                PlaceFechaTramite.Text = "";
            }
            else
            {
                PlaceFechaTramite.Text = "Fecha de Tramite";
            }
        }
    }
}
