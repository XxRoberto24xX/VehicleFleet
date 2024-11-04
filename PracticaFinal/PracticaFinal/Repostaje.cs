using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaFinal
{
    public class Repostaje
    {
        /* Atributos */
        Fecha datePriv;

        /* Propiedades */
        public Fecha date
        {
            get
            {
                return datePriv;
            }

            set
            {
                datePriv = value;
            }
        }
        public int kilometrosRep { get; set; } //kilometros hechos con el repostaje
        public int cuentaKilometros { get; set; }  //kilometros que tenia el coche antes del repostaje
        public double coste { get; set; }
        public double litros { get; set; }

        /* Necesario Para El Binding */
        public string dateString
        {
            get
            {
                return datePriv.ToString();
            }
        }

        /* Contructor */
        public Repostaje(Fecha date, int kilometros, double coste, double litros)
        {
            this.datePriv = date;
            this.kilometrosRep= kilometros;
            this.coste = coste;
            this.litros = litros;
        }

        public Repostaje(int i, double litros, int kilometrosRep, double precio)
        {
            this.kilometrosRep = kilometrosRep;
            this.litros = litros;
            coste = litros * (1 + precio);

            Fecha dt = new Fecha(i*5+1, 12, 2022);
            this.datePriv = dt;
        }
    }
}
