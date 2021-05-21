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
using System.Windows.Shapes;

using Datos;
using Metodos;

namespace HumanResourcesSM.Windows
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public static DUsuario ActUsuario;

        public void RefreshUsuario()
        {
            var res = new MUsuario().Encontrar(ActUsuario.idUsuario);

            var Roles = new MRol().Mostrar();

            ActUsuario = res[0];

            TxtUserName.Text = ActUsuario.usuario;
            txtRol.Text = Roles.Find(x => x.idRol == ActUsuario.idRol).nombre;
        }

        public Menu(DUsuario User)
        {
            InitializeComponent();
            ActUsuario = User;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshUsuario();
            if(ActUsuario.idRol == 3)
            {
                btnAdministracionMenu.Visibility = Visibility.Collapsed;
                btnGestionMenu.Visibility = Visibility.Collapsed;
            }
            else if(ActUsuario.idRol == 4)
            {
                btnAdministracionMenu.Visibility = Visibility.Collapsed;
                btnActividadMenu.Visibility = Visibility.Collapsed;
            }
        }

        public Border LastSelected;
        public int LastIndex = -1;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);

            if (index == LastIndex)
                return;

            var par = VisualTreeHelper.GetParent((Button)e.Source) as UIElement;
            Border PBord = (par as Border);

            PBord.BorderThickness = new Thickness(0, 0, 0, 5);
            
            if(LastIndex > -1)
            {
                LastSelected.BorderThickness = new Thickness(0, 0, 0, 0);
            }

            LastIndex = index;
            LastSelected = PBord;

            switch (index)
            {
                case 0:
                    GestionMenu frm2 = new GestionMenu();
                    ContentFrame.Content = frm2;
                    
                    break;
                case 1:
                    ActividadMenu frm1 = new ActividadMenu();
                    ContentFrame.Content = frm1;
                    break;
                case 2:
                    AdministracionMenu frm = new AdministracionMenu();
                    ContentFrame.Content = frm;
                    break;
                case 3:
                    UsuarioDG DG = new UsuarioDG();
                    ContentFrame.Content = DG;
                    break;
                case 4:
                    break;

            }

            
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            if (LastIndex > -1)
            {
                LastSelected.BorderThickness = new Thickness(0, 0, 0, 0);
            }

            LastIndex = -1;
            LastSelected = null;

            AjustesMenu frm = new AjustesMenu(this);
            ContentFrame.Content = frm;
        }

        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        
    }
}
