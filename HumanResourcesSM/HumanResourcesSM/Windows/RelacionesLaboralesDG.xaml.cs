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

using Datos;
using Metodos;

namespace HumanResourcesSM.Windows
{
    /// <summary>
    /// Interaction logic for DepartamentoDG.xaml
    /// </summary>
    public partial class RelacionesLaboralesDG : Page
    {
        MRelacionesLaborales Metodos = new MRelacionesLaborales();

        public RelacionesLaboralesDG()
        {
            InitializeComponent();

            MSeleccion MSel = new MSeleccion();
            var res = MSel.MostrarEmpleado("");

            foreach (DEmpleado item in res)
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

        public void Refresh()
        {
            int idEmpleado = CbEmpleado.SelectedIndex > -1 ? (int)CbEmpleado.SelectedValue : CbEmpleado.SelectedIndex;
            int idTipoTramite = CbTipoTramite.SelectedIndex > -1 ? (int)CbTipoTramite.SelectedValue : CbTipoTramite.SelectedIndex;

            List<DRelacionesLaborales> items = Metodos.Mostrar(idEmpleado, idTipoTramite);

            List<ModelRelLab> MRL = new List<ModelRelLab>();

            MTipoTramite MEtR = new MTipoTramite();

            var resp = MEtR.Mostrar("");

            MSeleccion MSEL = new MSeleccion();

            var Resp1 = MSEL.MostrarEmpleado("");

            foreach(DRelacionesLaborales item in items)
            {
                DTipoTramite DTT = resp.Find((TipoT) => TipoT.idTipoTramite == item.idTipoTramite);

                DEmpleado DEMP = Resp1.Find((Emp) => Emp.idEmpleado == item.idEmpleado);

                MRL.Add(new ModelRelLab(item.idRelacionesLaborales,
                                        DEMP.nombre + " " + DEMP.apellido, // TESTING
                                        DTT.nombre));
            }


            dgOperaciones.ItemsSource = MRL;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //contentsp.Children.Clear();

            Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            RelacionesLaboralesFrm frmTrab = new RelacionesLaboralesFrm();
            bool Resp = frmTrab.ShowDialog() ?? false;
            Refresh();
        }

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    int id = (int)((Button)sender).CommandParameter;
        //    var response = Metodos.Encontrar(id);

        //    RelacionesLaboralesFrm frm = new RelacionesLaboralesFrm();
        //    frm.Type = TypeForm.Update;
        //    frm.DataFill = response[0];
        //    bool Resp = frm.ShowDialog() ?? false;
        //    Refresh();
        //}

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult Resp = MessageBox.Show("¿Seguro que quieres eliminrar este item?", "Magicolor", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (Resp != MessageBoxResult.Yes)
                return;
            int id = (int)((Button)sender).CommandParameter;
            DRelacionesLaborales item = new DRelacionesLaborales()
            {
                idRelacionesLaborales = id
            };
            //Metodos.Eliminar(item);  //PENDIENTE POR HACER
            Refresh();
        }
        private void txtVer_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).CommandParameter;
            var response = Metodos.Encontrar(id);

            RelacionesLaboralesFrm frmTrab = new RelacionesLaboralesFrm();
            frmTrab.Type = TypeForm.Read;
            frmTrab.DataFill = response[0];
            bool Resp = frmTrab.ShowDialog() ?? false;
            Refresh();
        }

        public class ModelRelLab
        {
            public ModelRelLab(int idRelacionesLaborales, string nombreEmpleado, string tipoTramite)
            {
                this.idRelacionesLaborales = idRelacionesLaborales;
                NombreEmpleado = nombreEmpleado;
                TipoTramite = tipoTramite;
            }

            public int idRelacionesLaborales { get; set; }
            public string NombreEmpleado { get; set; }
            public string TipoTramite { get; set; }
        }

        private void CbEmpleado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
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
            Refresh();
            if (CbTipoTramite.SelectedIndex > -1)
            {
                PlaceTipoTramite.Text = "";
            }
            else
            {
                PlaceTipoTramite.Text = "Fecha Tramite";
            }
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            CbEmpleado.SelectedIndex = CbTipoTramite.SelectedIndex = -1;
        }
    }
}
