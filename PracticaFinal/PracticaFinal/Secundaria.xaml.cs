using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
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

namespace PracticaFinal
{
    /// <summary>
    /// Lógica de interacción para Secundaria.xaml
    /// </summary>
    /// 

    public class RepresentaEventArgs : EventArgs
    {
        public Coche cc { get; set; }

        public RepresentaEventArgs(Coche coche) 
        { 
            cc = coche;
        }
    }

    public class RepresentaBarrasEventArgs : EventArgs
    {
        public RepresentaBarrasEventArgs()
        {
            
        }
    }

    public partial class Secundaria : Window
    {
        /* Eventos */
        public event EventHandler<RepresentaEventArgs> Representa;
        public event EventHandler<RepresentaBarrasEventArgs> RepresentaBarras;

        /* Atributos */
        ObservableCollection<Coche> listaCoches;

        public Secundaria(ObservableCollection<Coche> l)
        {
            InitializeComponent();
            listaCoches = l;
            tablaCoches.ItemsSource = listaCoches;
        }

        /* Controlador evento tabla superior */
        private void tablaCoches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tablaCoches.SelectedItem != null)
            {
                Coche cc = (Coche)tablaCoches.SelectedItem;
                datosCoche.ItemsSource = cc.lista;
                OnRepresenta(cc);
                borrarCoche.IsEnabled = true;
                modificarCoche.IsEnabled = true;
            }

        }

        void OnRepresenta(Coche cc)
        {
            if (Representa != null)
            {
                Representa(this, new RepresentaEventArgs(cc));
            }
        }

        void OnRepresentaBarras()
        {
            if (RepresentaBarras != null)
            {
                RepresentaBarras(this, new RepresentaBarrasEventArgs());
            }
        }

        private void borrarCoche_Click(object sender, RoutedEventArgs e)
        {
            string msg = "El coche sera eliminado de forma permanente ¿Desea borrar el elemento?";
            string titulo = "Borrado";

            MessageBoxButton botones = MessageBoxButton.YesNo;
            MessageBoxImage icono = MessageBoxImage.Question;
            MessageBox.Show(msg, titulo, botones, icono);

            if (tablaCoches.SelectedItem != null)
            {
                Coche aBorrar = (Coche)tablaCoches.SelectedItem;
                for (int i = (aBorrar.lista.Count-1); i >= 0; i--)
                {
                    aBorrar.lista.RemoveAt(i);
                }
                listaCoches.Remove(aBorrar);
                tablaCoches.SelectedItem = null;
                datosCoche.SelectedItem = null;
                OnRepresentaBarras();
            }

            borrarCoche.IsEnabled = false;
            modificarCoche.IsEnabled = false;
        }

        private void modificarCoche_Click(object sender, RoutedEventArgs e)
        {
            Modificar add = new Modificar((Coche)tablaCoches.SelectedItem);

            add.Owner = this;
            add.ShowDialog();
            if (add.DialogResult == true)
            {
                Coche aBorrar = (Coche)tablaCoches.SelectedItem;
                listaCoches.Remove(aBorrar);
                Coche ch = new Coche(add.matriculaMandar, add.marcaMandar, add.kilometrosInicialesMandar, add.listaRep);
                listaCoches.Add(ch);
                datosCoche.ItemsSource = null;
                OnRepresentaBarras();
            }

            borrarCoche.IsEnabled = false;
            modificarCoche.IsEnabled = false;
        }
    }
}
