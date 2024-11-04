using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PracticaFinal
{
    /// <summary>
    /// Lógica de interacción para Modificar.xaml
    /// </summary>
    public partial class Modificar : Window
    {
        /* Atributos */
        ObservableCollection<Repostaje> rep;

        /* Propiedades */
        public ObservableCollection<Repostaje> listaRep
        {
            get
            {
                return rep;
            }
        }

        public string matriculaMandar
        {
            get
            {
                return matricula.Text;
            }
        }
        public string marcaMandar
        {
            get
            {
                return marca.Text;
            }
        }
        public int kilometrosInicialesMandar
        {
            get
            {
                return Convert.ToInt32(kilometrosIni.Text);
            }
        }

        /* Propiedades */

        public Modificar(Coche c)
        {
            InitializeComponent();
            lista.ItemsSource = c.lista;
            rep = c.lista;

            matricula.Text = c.matricula.ToString();
            marca.Text = c.marca.ToString();
            kilometrosIni.Text = c.kilometrosIniciales.ToString();
        }

        private void modificar_Coche(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void valido_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (matricula.Text.Length > 0 && matricula.Text.Length < 8 && marca.Text.Length > 0 && kilometrosIni.Text.Length > 0)
            {
                modificarCoche.IsEnabled = true;
            }
            else
            {
                modificarCoche.IsEnabled = false;
            }
        }

        private void Rep_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (dia.Text.Length > 0 && mes.Text.Length > 0 && año.Text.Length > 0 && coste.Text.Length > 0 && litros.Text.Length > 0 && KmRep.Text.Length > 0)
            {
                if (Convert.ToInt32(dia.Text) > 0 && Convert.ToInt32(dia.Text) < 32 && Convert.ToInt32(mes.Text) > 0 && Convert.ToInt32(mes.Text) < 13 && Convert.ToInt32(año.Text) > 1980 && Convert.ToInt32(año.Text) < 2024)
                {
                    if (añadirRep.IsEnabled == false)
                    {
                        añadirRep.IsEnabled = true;
                    }

                    if (lista.SelectedItem != null)
                    {
                        if (borrarRep.IsEnabled == false)
                        {
                            borrarRep.IsEnabled = true;
                        }

                        if (modificarRep.IsEnabled == false)
                        {
                            modificarRep.IsEnabled = true;
                        }
                    }
                }
            }
            else
            {
                if (añadirRep.IsEnabled == true)
                {
                    añadirRep.IsEnabled = false;
                }

                if (lista.SelectedItem != null)
                {
                    if (borrarRep.IsEnabled == true)
                    {
                        borrarRep.IsEnabled = false;
                    }

                    if (modificarRep.IsEnabled == true)
                    {
                        modificarRep.IsEnabled = false;
                    }
                }
            }
        }

        private void añadirRep_Click(object sender, RoutedEventArgs e)
        {
            Fecha dt = new Fecha(Convert.ToInt32(dia.Text), Convert.ToInt32(mes.Text), Convert.ToInt32(año.Text));

            if (KmRep.Text == "")
            {
                Repostaje rp = new Repostaje(dt, 0, Convert.ToDouble(coste.Text), Convert.ToDouble(litros.Text));
                rep.Add(rp);
            }
            else
            {
                Repostaje rp = new Repostaje(dt, Convert.ToInt32(KmRep.Text), Convert.ToDouble(coste.Text), Convert.ToDouble(litros.Text));
                rep.Add(rp);
            }

        }

        private void borrarRep_Click(object sender, RoutedEventArgs e)
        {
            Repostaje borrarRepos = (Repostaje)lista.SelectedItem;
            rep.Remove(borrarRepos);

            lista.SelectedItem = null;
            modificarRep.IsEnabled = false;
            borrarRep.IsEnabled = false;
        }

        private void lista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lista.SelectedItem != null)
            {
                borrarRep.IsEnabled = true;
                modificarRep.IsEnabled = true;

                Repostaje r = (Repostaje)lista.SelectedItem;
                dia.Text = r.date.dia.ToString();
                mes.Text = r.date.mes.ToString();
                año.Text = r.date.año.ToString();

                KmRep.Text = r.kilometrosRep.ToString();
                coste.Text = r.coste.ToString();
                litros.Text = r.litros.ToString();
            }
        }

        private void modificarRep_Click(object sender, RoutedEventArgs e)
        {
            if (lista.SelectedItem != null)
            {
                Repostaje borrarRep = (Repostaje)lista.SelectedItem;
                rep.Remove(borrarRep);

                Fecha dt = new Fecha(Convert.ToInt32(dia.Text), Convert.ToInt32(mes.Text), Convert.ToInt32(año.Text));

                if (KmRep.Text == "")
                {
                    Repostaje rp = new Repostaje(dt, 0, Convert.ToDouble(coste.Text), Convert.ToDouble(litros.Text));
                    rep.Add(rp);
                }
                else
                {
                    Repostaje rp = new Repostaje(dt, Convert.ToInt32(KmRep.Text), Convert.ToDouble(coste.Text), Convert.ToDouble(litros.Text));
                    rep.Add(rp);
                }
            }

            lista.SelectedItem = null;
            modificarRep.IsEnabled = false;
            borrarRep.IsEnabled = false;
        }

        private void fecha_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.D1) || e.Key.Equals(Key.D2) || e.Key.Equals(Key.D3) || e.Key.Equals(Key.D4) || e.Key.Equals(Key.D5) || e.Key.Equals(Key.D6) || e.Key.Equals(Key.D7) || e.Key.Equals(Key.D8) || e.Key.Equals(Key.D9) || e.Key.Equals(Key.D0))
            {
                e.Handled = false;
            }
            else if (char.IsControl((char)e.Key))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
