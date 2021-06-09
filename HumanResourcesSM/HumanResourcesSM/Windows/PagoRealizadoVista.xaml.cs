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
    public partial class PagoRealizadoVista : Window
    {

        public PagoRealizadoVista(PagoFrm Par)
        {
            InitializeComponent();

            ParentFrm = Par;
        }


        public TypeForm Type;
        public DPago DataFill;

        public DPago UForm;

        public MPago Metodos = new MPago();

        public PagoFrm ParentFrm;

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

            string Banco = txtBanco.txt.Text;
            string Referencia = txtReferencia.txt.Text;
            DateTime PeriodoInicio = CbPeriodoInicio.SelectedDate ?? DateTime.Now.AddYears(1);
            DateTime PeriodoFinal = CbPeriodoFinal.SelectedDate ?? DateTime.Now.AddYears(1);

            UForm = new DPago(0,
                              0,
                              DateTime.Now,
                              Banco,
                              Referencia,
                              0,
                              PeriodoInicio,
                              PeriodoFinal,
                              0,
                              0,
                              0,
                              0,
                              1);
        }

        void Create()
        {
            fillData();
            if (UForm == null)
                return;

            ParentFrm.RealizarPago(UForm);

            this.DialogResult = true;
            this.Close();

        }

        private void CbPeriodoInicio_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbPeriodoInicio.SelectedDate != null)
            {
                PlacePeriodoInicio.Text = "";

                CbPeriodoFinal.DisplayDateStart = CbPeriodoInicio.SelectedDate?.Date;
            }
            else
            {
                PlacePeriodoInicio.Text = "Periodo Inicio";

                CbPeriodoFinal.DisplayDateStart = null;
            }
        }

        private void CbPeriodoFinal_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbPeriodoFinal.SelectedDate != null)
            {
                PlacePeriodoFinal.Text = "";

                CbPeriodoInicio.DisplayDateEnd = CbPeriodoFinal.SelectedDate?.Date;
            }
            else
            {
                PlacePeriodoFinal.Text = "Periodo Final";

                CbPeriodoInicio.DisplayDateEnd = null;
            }
        }


        #region Validation
        bool Validate()
        {
            if (txtBanco.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el Campo Banco!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtBanco.txt.Focus();
                return true;
            }
            if (txtReferencia.txt.Text == "")
            {
                MessageBox.Show("Debes llenar el Campo Referencia!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                txtReferencia.txt.Focus();
                return true;
            }
            if (CbPeriodoInicio.SelectedDate == null || CbPeriodoFinal.SelectedDate == null)
            {
                MessageBox.Show("Debes Seleccionar un periodo!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Error);
                CbPeriodoInicio.Focus();
                return true;
            }

            return false;
        }

        #endregion

        
    }
}
