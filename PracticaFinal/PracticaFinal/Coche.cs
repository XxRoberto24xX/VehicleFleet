using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaFinal
{
    public class Coche
    {
        /* Atributos */
        ObservableCollection<Repostaje> repostajes = new ObservableCollection<Repostaje>();

        /* Propiedades */
        public string matricula { get; set; }
        public string marca { get; set; }
        public int kilometros 
        {
            get
            {
                int total = 0;

                foreach(Repostaje r in repostajes)
                {
                    total += r.kilometrosRep;
                }

                return total + kilometrosIniciales;
            }
        }
        public int kilometrosIniciales { get; set; }
        public double consumoMedio
        {
            get
            {
                double media = 0;
                double totall = 0;
                double totalk = 0;

                foreach (Repostaje r in repostajes)
                {
                    totall += r.litros;
                    totalk += r.kilometrosRep;
                }

                media = totall / totalk; // media de gasto por cada kilometro

                return media * 100;
            }
        }

        public double costeMedio
        {
            get
            {
                double media = 0;
                double totall = 0;
                double totalk = 0;

                foreach (Repostaje r in repostajes)
                {
                    totall += r.coste;
                    totalk += r.kilometrosRep;
                }

                media = totall / totalk; // media de gasto por cada kilometro

                return media * 100;
            }
        }

        public ObservableCollection<Repostaje> lista
        {
            get
            {
                return repostajes;
            }
        }

        /* Constructor */
        public Coche(string mat, string marca, int kilometros, ObservableCollection<Repostaje> r) 
        { 
            this.matricula = mat;   
            this.marca = marca;
            this.kilometrosIniciales = kilometros;
            this.repostajes = r;


            for (int i = 0; i < repostajes.Count; i++)
            {
                repostajes[i].cuentaKilometros = kilometrosIniciales;
                for (int j = 0; j < i; j++)
                {
                    repostajes[i].cuentaKilometros += repostajes[j].kilometrosRep;
                }
            }
        }

        public Coche(string matricula, string marca, Random rand)
        {
            this.matricula = matricula;
            this.marca = marca;
            this.kilometrosIniciales = rand.Next(50000, 100000);

            for (int i = 0; i < 4; i++)
            {
                int kilometrosRep = rand.Next(100, 300);
                double litros = rand.Next(15, 50);
                double precio = rand.NextDouble();

                Repostaje rp = new Repostaje(i, litros, kilometrosRep, precio);
                repostajes.Add(rp);
            }

            for (int i = 0; i < repostajes.Count; i++)
            {
                repostajes[i].cuentaKilometros = kilometrosIniciales;
                for (int j = 0; j < i; j++)
                {
                    repostajes[i].cuentaKilometros += repostajes[j].kilometrosRep;
                }
            }
        }
    }
}
