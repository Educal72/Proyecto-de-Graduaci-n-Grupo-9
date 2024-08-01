using FrontEndWPF.Index;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FrontEndWPF
{
    /// <summary>
    /// Lógica de interacción para editarPerfilCompetencial.xaml
    /// </summary>
    public partial class editarPerfilCompetencial : Window
    {
        public string? titulo_editarPerfilCompetencial { get; set; } // Título del perfil de empleo.
        public string? descripcion_editarPerfilCompetencial { get; set; } // Descripción del perfil de empleo.
        public string? experiencia_editarPerfilCompetencial { get; set; } // Nivel de experiencia requerido.
        public string? requisitos_editarPerfilCompetencial { get; set; } // Requisitos del perfil de empleo (Básico, Intermedio o Avanzado).
        public string? ubicacion_editarPerfilCompetencial { get; set; } // Ubicación del trabajo.
        public double salario_editarPerfilCompetencial { get; set; } // Salario mínimo ofrecido.


        public editarPerfilCompetencial()
        {
            InitializeComponent();
        }


        /* Método relacionado al botón: Guardar.
         * El cual se encarga de guardar los datos que están siendo -
         * ingresados por el usuario.*/
        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Quieres editar este perfil competencial?",
                "¡Confirmación!", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                titulo_editarPerfilCompetencial = Titulo_TextBox.Text;
                descripcion_editarPerfilCompetencial = Descripcion_TextBox.Text;
                experiencia_editarPerfilCompetencial = Experiencia_TextBox.Text;
                requisitos_editarPerfilCompetencial = Requisitos_TextBox.Text;
                ubicacion_editarPerfilCompetencial = Ubicacion_TextBox.Text;
                salario_editarPerfilCompetencial = double.Parse(Salario_TextBox.Text);
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Se cancelo la solicitud, muchas gracias",
                    "¡Aviso!", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = false;
            }

        }


        /* Método relacionado al botón: Cancelar.
         * El cual se encarga de cancelar los datos que están siendo -
         * ingresados por el usuario.*/
        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Se cancelo la solicitud, muchas gracias",
                "¡Aviso!", MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = false;
        }

    }
}
