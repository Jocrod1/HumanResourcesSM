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

        class Pais
        {
            public string DisplayNombre { get; set; }
            public string Nombre { get; set; }
            public string CodIso { get; set; }

            public Pais(string nombre, string codIso)
            {
                Nombre = nombre;
                CodIso = codIso;

                DisplayNombre = nombre + " - " + CodIso;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<Pais> paises = new List<Pais>();

            HashSet<Pais> pash = new HashSet<Pais>();

            List<Pais> nodupes = new List<Pais>();

            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.InstalledWin32Cultures);


            foreach (CultureInfo info in cultures)
            {
                try
                {
                    RegionInfo info2 = new RegionInfo(info.LCID);

                    Pais pais = new Pais(info2.DisplayName, info2.TwoLetterISORegionName);
                    pash.Add(pais);
                    pash.OrderBy(s => s.Nombre);
                    
                }
                catch { 
                }
            }

            CbPaisNac.ItemsSource = pash;
            CbPaisNac.DisplayMemberPath = "DisplayNombre";
            CbPaisNac.SelectedValuePath = "CodIso";
        }

        private void CbDepartamento_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void CbFechaNac_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CbPaisNac_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    }
}
