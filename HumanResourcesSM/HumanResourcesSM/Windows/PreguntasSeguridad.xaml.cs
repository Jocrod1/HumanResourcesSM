﻿using System;
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
    /// Interaction logic for PreguntasSeguridad.xaml
    /// </summary>
    public partial class PreguntasSeguridad : Window
    {
        Login ParentForm;

        public PreguntasSeguridad(Login parentfrm)
        {
            InitializeComponent();

            ParentForm = parentfrm;
        }



        public List<DSeguridad> DataFill;


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Create();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtTitulo.Text = DataFill[0].usuario;
            txtPregunta.Text = DataFill[0].pregunta;
            txtPregunta2.Text = DataFill[1].pregunta;
            txtPregunta3.Text = DataFill[2].pregunta;
        }

        private bool Validate()
        {
            if (txtRespuesta.Password == "")
            {
                MessageBox.Show("Debes responder la primera pregunta!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                txtRespuesta.Focus();
                return true;
            }
            if (txtRespuesta2.Password == "")
            {
                MessageBox.Show("Debes responder la segunda pregunta!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                txtRespuesta2.Focus();
                return true;
            }
            if (txtRespuesta3.Password == "")
            {
                MessageBox.Show("Debes responder la tercera pregunta!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                txtRespuesta3.Focus();
                return true;
            }

            return false;
        }

        void Create()
        {
            if (Validate()) return;

            if ((txtRespuesta.Password == DataFill[0].respuesta) && (txtRespuesta2.Password == DataFill[1].respuesta) && (txtRespuesta3.Password == DataFill[2].respuesta))
            {
                CambiarContraseña frmContraseña = new CambiarContraseña(this);
                frmContraseña.DataFill = DataFill[0];
                MessageBox.Show("Datos correctos, debes introducir una nueva contraseña!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Hide();
                bool Resp = frmContraseña.ShowDialog() ?? false;
            }
            else
            {
                MessageBox.Show("Datos Incorrectos!", "SwissNet", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void txtBuscar_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtRespuesta.Password == "")
            {
                txtBucarPlaceH.Text = "";
            }

        }

        private void txtBuscar_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtRespuesta.Password == "")
            {
                txtBucarPlaceH.Text = "Respuesta";
            }

        }


        private void txtBuscar2_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtRespuesta2.Password == "")
            {
                txtBucarPlaceH2.Text = "";
            }
        }

        private void txtBuscar2_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtRespuesta2.Password == "")
            {
                txtBucarPlaceH2.Text = "Respuesta";
            }
        }


        private void txtBuscar3_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtRespuesta3.Password == "")
            {
                txtBucarPlaceH3.Text = "";
            }
        }

        private void txtBuscar3_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtRespuesta3.Password == "")
            {
                txtBucarPlaceH3.Text = "Respuesta";
            }
        }
    }
}