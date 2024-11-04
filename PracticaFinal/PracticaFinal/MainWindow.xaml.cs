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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.XPath;

namespace PracticaFinal
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /* Atributos */
        ObservableCollection<Coche> listaCoches = new ObservableCollection<Coche>();
        Secundaria sc;

        int indice; //tendra el indice del ultimo repostaja que se manda representar
        int grafico = 1; //si es 1 se representara el grafico general si es 2 el de repostajes

        Polyline costes, consumo, kilometros;

        public MainWindow()
        {
            InitializeComponent();

            /* Añadimos unos cuantos vehiculos para poder ver los datos */
            Random rand = new Random();

            Coche cc1 = new Coche("5674mgh", "Seat", rand);
            Coche cc2 = new Coche("7685sdr", "Renault", rand);
            Coche cc3 = new Coche("1199jgd", "Toyota", rand);

            listaCoches.Add(cc1);
            listaCoches.Add(cc2);
            listaCoches.Add(cc3);

            dibujaBarras();
        }

        /* Manejador apertura ventana secundaria */
        private void abrirSecundaria_Click(object sender, RoutedEventArgs e)
        {
            if (sc == null)
            {
                sc = new Secundaria(listaCoches);
                sc.Title = "Secundaria";
                sc.Owner = this;
                sc.Closed += Sc_Closed;
                sc.Representa += Sc_RepresentaRepostaje;
                sc.RepresentaBarras += Sc_RepresentaBarras;
                sc.Show();
            }
        }

        /* Manejador cerrado ventana secundaria */
        private void Sc_Closed(object sender, EventArgs e)
        {
            sc = null;
        }

        /* Manejador evento representacion de repostajes */
        private void Sc_RepresentaRepostaje(object sender, RepresentaEventArgs e)
        {
            indice = listaCoches.IndexOf(e.cc);
            verTodo.IsEnabled = true;

            dibujaRep();
        }

        private void Sc_RepresentaBarras(object sender, RepresentaBarrasEventArgs e)
        {
            verTodo.IsEnabled = false;
            dibujaBarras();
        }

        void dibujaRep()
        {
            grafico = 2;

            double xrealmax, xrealmin, yrealmax, yrealmin, xreal, yreal;
            double xpantmax, xpantmin, ypantmax, ypantmin, xpant, ypant;

            double maximo;

            int numpuntos = listaCoches[indice].lista.Count;  // el numero de puntos sera el numero de repostajes

            lienzoPequeño.Children.Clear();

            /* Añadimos las lineas */
            costes = new Polyline();
            consumo = new Polyline();
            kilometros = new Polyline();

            costes.Stroke = Brushes.Red;
            consumo.Stroke = Brushes.Green;
            kilometros.Stroke = Brushes.Blue;

            costes.StrokeThickness = 3;
            consumo.StrokeThickness = 3;
            kilometros.StrokeThickness = 3;

            lienzoPequeño.Children.Add(costes);
            lienzoPequeño.Children.Add(consumo);
            lienzoPequeño.Children.Add(kilometros);

            /* Dimensiones de pantalla de las que disponemos */
            xpantmax = lienzoPequeño.ActualWidth - 60;
            xpantmin = 60;
            ypantmax = lienzoPequeño.ActualHeight - 20;
            ypantmin = 20;

            /* Dimensiones en la recta real */
            xrealmax = numpuntos-1;  
            xrealmin = 0;
            yrealmin = 0;

            /* Calculamos los valores de Coste */
            maximo = 0;
            foreach (Repostaje r in listaCoches[indice].lista)
            {
                if (r.coste > maximo)
                {
                    maximo = r.coste;
                }
            }

            yrealmax = maximo;

            for (int i = 0; i < numpuntos; i++)
            {
                xreal = i;
                yreal = listaCoches[indice].lista[i].coste;

                /* Ahora tenemos que hacer las transformaciones a la pantalla de la que disponemos */
                xpant = (xpantmax - xpantmin) * (xreal - xrealmin) / (xrealmax - xrealmin) + xpantmin;
                ypant = (ypantmin - ypantmax) * (yreal - yrealmin) / (yrealmax - yrealmin) + ypantmax;

                /* Añadimos los puntos */
                Point pt = new Point(xpant, ypant);
                costes.Points.Add(pt);

                /* Ponemos los las etiquetas */
                /* Tiempo */  // vale con ponerlas una vez
                Label etq = new Label();
                etq.Content = listaCoches[indice].lista[i].dateString;
                etq.HorizontalAlignment = HorizontalAlignment.Center;
                etq.FontSize =10;

                lienzoPequeño.Children.Add(etq);
                Canvas.SetLeft(etq, xpant);
                Canvas.SetBottom(etq, ypantmin - 20);  // para nivelar el ancho de la  etiqueta y que no se metan en el dibujo
            }

            /* Leyenda */  //vale con ponerlas una vez
            for (int i = 0; i < 3; i++)
            {
                Label etq2 = new Label();
                switch (i)
                {
                    case 0:
                        etq2.Content = "Coste (€)";
                        etq2.Foreground = Brushes.Red;
                        break;
                    case 1:
                        etq2.Content = "Consumo (litros)";
                        etq2.Foreground = Brushes.Green;
                        break;
                    case 2:
                        etq2.Content = "Kilometros";
                        etq2.Foreground = Brushes.Blue;
                        break;
                }
                etq2.HorizontalAlignment = HorizontalAlignment.Center;
                etq2.FontSize = 10;

                lienzoPequeño.Children.Add(etq2);
                Canvas.SetLeft(etq2, (xpantmax / 7 * (1 + i*2)));
                Canvas.SetTop(etq2, lienzoPequeño.ActualHeight + 10);
            }

            /* Escalas */  //tenemos que poner una por cada linea
            double zonaEscribir = ypantmax - ypantmin;
            double pos = zonaEscribir / 8;
            double valor = yrealmax / 8;

            for (int i = 0; i < 9; i++)
            {
                Label etq3 = new Label();
                etq3.Content = ((int)(yrealmax - (valor * i))).ToString() + "...";  /* Ya que le valor preciso lo tenemos en la tabla pondremos los valores aproximados con fin comparativo */
                etq3.Foreground = Brushes.Red;
                etq3.FontSize = 10;

                lienzoPequeño.Children.Add(etq3);
                Canvas.SetLeft(etq3, -25);
                Canvas.SetTop(etq3, (pos * i) - 3);
            }

            /* Calculamos los valores de Consumo */
            maximo = 0;
            foreach (Repostaje r in listaCoches[indice].lista)
            {
                if (r.litros > maximo)
                {
                    maximo = r.litros;
                }
            }

            yrealmax = maximo;

            consumo.Points.Clear();

            for (int i = 0; i < numpuntos; i++)
            {
                xreal = i;
                yreal = listaCoches[indice].lista[i].litros;

                /* Ahora tenemos que hacer las transformaciones a la pantalla de la que disponemos */
                xpant = (xpantmax - xpantmin) * (xreal - xrealmin) / (xrealmax - xrealmin) + xpantmin;
                ypant = (ypantmin - ypantmax) * (yreal - yrealmin) / (yrealmax - yrealmin) + ypantmax;

                /* Añadimos los puntos */
                Point pt = new Point(xpant, ypant);
                consumo.Points.Add(pt);
            }

            /* Escalas */  //tenemos que poner una por cada linea
            zonaEscribir = ypantmax - ypantmin;
            pos = zonaEscribir / 8;
            valor = yrealmax / 8;

            for (int i = 0; i < 9; i++)
            {
                Label etq3 = new Label(); 
                etq3.Content =((int)(yrealmax - (valor * i))).ToString() + "...";  /* Ya que le valor preciso lo tenemos en la tabla pondremos los valores aproximados con fin comparativo */
                etq3.Foreground = Brushes.Green;
                etq3.FontSize = 10;

                lienzoPequeño.Children.Add(etq3);
                Canvas.SetLeft(etq3, 2);
                Canvas.SetTop(etq3, (pos * i) - 3);
            }

            /* Calculamos los valores de Kilometros */
            maximo = 0;
            foreach (Repostaje r in listaCoches[indice].lista)
            {
                if (r.kilometrosRep > maximo)
                {
                    maximo = r.kilometrosRep;
                }
            }

            yrealmax = maximo;

            kilometros.Points.Clear();

            for (int i = 0; i < numpuntos; i++)
            {
                xreal = i;
                yreal = listaCoches[indice].lista[i].kilometrosRep;

                /* Ahora tenemos que hacer las transformaciones a la pantalla de la que disponemos */
                xpant = (xpantmax - xpantmin) * (xreal - xrealmin) / (xrealmax - xrealmin) + xpantmin;
                ypant = (ypantmin - ypantmax) * (yreal - yrealmin) / (yrealmax - yrealmin) + ypantmax;

                /* Añadimos los puntos */
                Point pt = new Point(xpant, ypant);
                kilometros.Points.Add(pt);
            }

            /* Escalas */  //tenemos que poner una por cada linea
            zonaEscribir = ypantmax - ypantmin;
            pos = zonaEscribir / 8;
            valor = yrealmax / 8;

            for (int i = 0; i < 9; i++)
            {
                Label etq3 = new Label();
                etq3.Content = "..." + ((int)(yrealmax - (valor * i))).ToString();  /* Ya que le valor preciso lo tenemos en la tabla pondremos los valores aproximados con fin comparativo */
                etq3.Foreground = Brushes.Blue;
                etq3.FontSize = 10;

                lienzoPequeño.Children.Add(etq3);
                Canvas.SetLeft(etq3, lienzoPequeño.ActualWidth);
                Canvas.SetTop(etq3, (pos * i) - 3);
            }

        }

        void dibujaBarras()
        {
            grafico = 1;

            double xrealmax, yrealmax, yrealmin,yreal;
            double xpantmax, xpantmin, ypantmax, ypantmin,ypant;

            double maximo;

            int numpuntos = listaCoches.Count;  // el numero de puntos sera el numero de repostajes

            lienzoPequeño.Children.Clear();

            /* Dimensiones de pantalla de las que disponemos */
            xpantmax = lienzoPequeño.ActualWidth;
            xpantmin = 0;
            ypantmax = lienzoPequeño.ActualHeight - 30;
            ypantmin = 10;

            /* Dimensiones en la recta real */
            xrealmax = numpuntos - 1;
            yrealmin = 0;



            /* Calculamos los valores de Coste */
            maximo = 0;
            foreach (Coche c in listaCoches)
            {
                if (c.costeMedio > maximo)
                {
                    maximo = c.costeMedio;
                }
            }

            yrealmax = maximo;

            double tamZona = (xpantmax - xpantmin) / numpuntos;
            double widthBarra = tamZona / 9; //suponemos dos barras que no crearemos y haran de separacion entre elementos

            for (int i = 0; i < numpuntos; i++)
            {
                yreal = listaCoches[i].costeMedio;

                /* Ahora tenemos que hacer las transformaciones a la pantalla de la que disponemos */
                ypant = (ypantmin - ypantmax) * (yreal - yrealmin) / (yrealmax - yrealmin) + ypantmax;

                /* Añadimos los rectangulos */
                Rectangle rc = new Rectangle();
                rc.Width = widthBarra;
                if (ypantmax - ypant > 0)
                {
                    rc.Height = ypantmax - ypant;
                }
                
                rc.Fill = Brushes.Red;

                lienzoPequeño.Children.Add(rc);
                Canvas.SetLeft(rc, xpantmin + (tamZona * i) + widthBarra * 2);
                Canvas.SetTop(rc, ypant);

                /* Ponemos los las etiquetas */
                /* Matricula */  // vale con ponerlas una vez
                Label etq = new Label();
                etq.Content = listaCoches[i].matricula;
                etq.HorizontalAlignment = HorizontalAlignment.Center;
                etq.FontSize = 10;

                lienzoPequeño.Children.Add(etq);
                Canvas.SetLeft(etq, xpantmin + (tamZona * i) + widthBarra * 2);
                Canvas.SetBottom(etq, ypantmin);  
            }

            /* Leyenda */  //vale con ponerlas una vez
            for (int i = 0; i < 3; i++)
            {
                Label etq2 = new Label();
                switch (i)
                {
                    case 0:
                        etq2.Content = "Coste (€)";
                        etq2.Foreground = Brushes.Red;
                        break;
                    case 1:
                        etq2.Content = "Consumo (litros)";
                        etq2.Foreground = Brushes.Green;
                        break;
                    case 2:
                        etq2.Content = "Kilometros";
                        etq2.Foreground = Brushes.Blue;
                        break;
                }
                etq2.HorizontalAlignment = HorizontalAlignment.Center;
                etq2.FontSize = 10;

                lienzoPequeño.Children.Add(etq2);
                Canvas.SetLeft(etq2, (xpantmax / 7 * (1 + i * 2)));
                Canvas.SetTop(etq2, lienzoPequeño.ActualHeight + 10);
            }

            /* Escalas */  //tenemos que poner una por cada linea
            double zonaEscribir = ypantmax - ypantmin;
            double pos = zonaEscribir / 8;
            double valor = yrealmax / 8;

            for (int i = 0; i < 9; i++)
            {
                Label etq3 = new Label();
                etq3.Content =((int)(yrealmax - (valor * i))).ToString() + "...";  /* Ya que le valor preciso lo tenemos en la tabla pondremos los valores aproximados con fin comparativo */
                etq3.Foreground = Brushes.Red;
                etq3.FontSize = 10;

                lienzoPequeño.Children.Add(etq3);
                Canvas.SetLeft(etq3, -25);
                Canvas.SetTop(etq3, (pos * i) - 7);
            }



            /* Calculamos los valores de Consumo */
            maximo = 0;
            foreach (Coche c in listaCoches)
            {
                if (c.consumoMedio > maximo)
                {
                    maximo = c.consumoMedio;
                }
            }

            yrealmax = maximo;

            for (int i = 0; i < numpuntos; i++)
            {
                yreal = listaCoches[i].consumoMedio;

                /* Ahora tenemos que hacer las transformaciones a la pantalla de la que disponemos */
                ypant = (ypantmin - ypantmax) * (yreal - yrealmin) / (yrealmax - yrealmin) + ypantmax;

                /* Añadimos los rectangulos */
                Rectangle rc = new Rectangle();
                rc.Width = widthBarra;
                if (ypantmax - ypant > 0)
                {
                    rc.Height = ypantmax - ypant;
                }
                rc.Fill = Brushes.Green;

                lienzoPequeño.Children.Add(rc);
                Canvas.SetLeft(rc, xpantmin + (tamZona * i) + widthBarra * 4);
                Canvas.SetTop(rc, ypant);
            }

            /* Escalas */  //tenemos que poner una por cada linea
            zonaEscribir = ypantmax - ypantmin;
            pos = zonaEscribir / 8;
            valor = yrealmax / 8;

            for (int i = 0; i < 9; i++)
            {
                Label etq3 = new Label();
                etq3.Content = ((int)(yrealmax - (valor * i))).ToString() + "...";  /* Ya que le valor preciso lo tenemos en la tabla pondremos los valores aproximados con fin comparativo */
                etq3.Foreground = Brushes.Green;
                etq3.FontSize = 10;

                lienzoPequeño.Children.Add(etq3);
                Canvas.SetLeft(etq3, 2);
                Canvas.SetTop(etq3, (pos * i) - 7);
            }



            /* Calculamos los valores de Kilometros */
            maximo = 0;
            foreach (Coche c in listaCoches)
            {
                if (c.kilometros > maximo)
                {
                    maximo = c.kilometros;
                }
            }

            yrealmax = maximo;

            for (int i = 0; i < numpuntos; i++)
            {
                yreal = listaCoches[i].kilometros;

                /* Ahora tenemos que hacer las transformaciones a la pantalla de la que disponemos */
                ypant = (ypantmin - ypantmax) * (yreal - yrealmin) / (yrealmax - yrealmin) + ypantmax;

                /* Añadimos los rectangulos */
                Rectangle rc = new Rectangle();
                rc.Width = widthBarra;
                if (ypantmax - ypant > 0)
                {
                    rc.Height = ypantmax - ypant;
                }
                rc.Fill = Brushes.Blue;

                lienzoPequeño.Children.Add(rc);
                Canvas.SetLeft(rc, xpantmin + (tamZona * i) + widthBarra * 6);
                Canvas.SetTop(rc, ypant);
            }

            /* Escalas */  //tenemos que poner una por cada linea
            zonaEscribir = ypantmax - ypantmin;
            pos = zonaEscribir / 8;
            valor = yrealmax / 8;

            for (int i = 0; i < 9; i++)
            {
                Label etq3 = new Label();
                etq3.Content = "..." + ((int)(yrealmax - (valor * i))).ToString();  /* Ya que le valor preciso lo tenemos en la tabla pondremos los valores aproximados con fin comparativo */
                etq3.Foreground = Brushes.Blue;
                etq3.FontSize = 10;

                lienzoPequeño.Children.Add(etq3);
                Canvas.SetLeft(etq3, lienzoPequeño.ActualWidth);
                Canvas.SetTop(etq3, (pos * i) - 7); // el -7 es para nivelar la altura de la etiqueta ya que el posicionamiento se hace desde al esquina superior izquierda
            }


        }

        /* Manejador del boton */
        private void verTodo_Click(object sender, RoutedEventArgs e)
        {
            grafico = 1;
            lienzoPequeño.Children.Clear();
            verTodo.IsEnabled= false;
            dibujaBarras();
        }

        /* Manejador Redimension de Ventana */
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            switch (grafico)
            {
                case 1:
                    dibujaBarras();
                    break;
                case 2:
                    dibujaRep();
                    break;
            }
        }

        /* Manejador Ventana Añadir coche */
        private void añadirVehiculo_Click(object sender, RoutedEventArgs e)
        {
            Añadir add = new Añadir();

            add.Owner = this;
            add.ShowDialog();
            if (add.DialogResult == true)
            {
                Coche ch = new Coche(add.matriculaMandar, add.marcaMandar, add.kilometrosInicialesMandar, add.listaRep);
                listaCoches.Add(ch);
                if (grafico == 1)
                {
                    dibujaBarras();
                }
            }
        }
    }
}
