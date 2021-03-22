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
    public partial class ContratoFrm : Window
    {

        public ContratoFrm(EntrevistarFrm Par)
        {
            InitializeComponent();

            ParentFrm = Par;
        }


        public TypeForm Type;
        public DContrato DataFill;

        public DContrato UForm;

        public MContrato Metodos = new MContrato();

        public EntrevistarFrm ParentFrm;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Create();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
   
        }

        void fillData()
        {
            if (Validate())
            {
                UForm = null;
                return;
            }

            double Sueldo = double.Parse(txtSueldo.txt.Text);
            int HorasSemanales = int.Parse(txtHorasSemanales.txt.Text);
            DateTime FechaCulminacion = CbFechaCulminacion.SelectedDate ?? DateTime.Now.AddYears(1);

            UForm = new DContrato(0,
                                  0,
                                  DateTime.Now,
                                  "",
                                  FechaCulminacion,
                                  Sueldo,
                                  HorasSemanales);
        }

        void Create()
        {
            fillData();
            if (UForm == null)
                return;

            ParentFrm.RegistrarContrato(UForm);

            this.DialogResult = true;
            this.Close();

        }

        private void CbFechaCulminacion_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbFechaCulminacion.SelectedDate != null)
            {
                PlaceFechaCulminacion.Text = "";
            }
            else
            {
                PlaceFechaCulminacion.Text = "Culminación del Contrato";
            }
        }


        #region Validation
        bool Validate()
        {
            if (txtSueldo.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el Campo Sueldo!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtSueldo.txt.Focus();
                return true;
            }
            if (txtHorasSemanales.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el Campo de Horas Semanales!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtHorasSemanales.txt.Focus();
                return true;
            }
            if (CbFechaCulminacion.SelectedDate == null)
            {
                MessageBox.Show("Debes Seleccionar una Fecha de Culminación de Contrato!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbFechaCulminacion.Focus();
                return true;
            }

            return false;
        }
        #endregion

       
    }
}
