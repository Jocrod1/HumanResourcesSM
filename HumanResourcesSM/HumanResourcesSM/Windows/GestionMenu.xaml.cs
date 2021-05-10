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
    /// Interaction logic for SeleccionFrm.xaml
    /// </summary>
    public partial class GestionMenu : Page
    {

        public DUsuario ActUsuario;
        public GestionMenu()
        {
            InitializeComponent();

            SeleccionFrm frm = new SeleccionFrm();
            ContentFrame.Content = frm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button thisbtn = (Button)e.Source;

            int index = int.Parse(thisbtn.Uid);

            var par = VisualTreeHelper.GetParent((Button)e.Source) as UIElement;
            Grid PGrid = (par as Grid);

            var ParBoord = VisualTreeHelper.GetParent(SelectedBord);
            Grid LastPGrid = ParBoord as Grid;

            LastPGrid.Children.Remove(SelectedBord);

            PGrid.Children.Add(SelectedBord);

            switch (index)
            {
                case 0:
                    SeleccionFrm frm = new SeleccionFrm();
                    ContentFrame.Content = frm;
                    break;
                case 1:
                    AsignacionFrm frm1 = new AsignacionFrm();
                    ContentFrame.Content = frm1;
                    break;
                case 2:
                    EntrevistarFrm frm2 = new EntrevistarFrm();
                    ContentFrame.Content = frm2;
                    break;
                case 3:
                    DespidoDG dg = new DespidoDG();
                    ContentFrame.Content = dg;
                    break;
                case 4:
                    DepartamentoDG dgD = new DepartamentoDG();
                    ContentFrame.Content = dgD;
                    break;
                case 5:
                    EmpleadoDG dgE = new EmpleadoDG();
                    ContentFrame.Content = dgE;
                    break;
            }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Menu.ActUsuario.idRol != 1 && Menu.ActUsuario.idRol != 2)
            {
                GridDepartamentos.Visibility = Visibility.Collapsed;
                GridEmpleados.Visibility = Visibility.Collapsed;
                GridAsignacion.Visibility = Visibility.Collapsed;
                if (Menu.ActUsuario.entrevistando != 1)
                {
                    GridContratacion.Visibility = Visibility.Collapsed;
                }
            }
            
        }
    }
}
