using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaFinal
{
    public class Fecha
    {
        /* Propiedades */
        public int dia { get; set; }
        public int mes { get; set; }
        public int año { get; set; }

        /* Constructor */
        public Fecha(int dia, int mes, int año)
        {
            this.dia = dia;
            this.mes = mes;
            this.año = año;
        }

        public override string ToString()
        {
            return dia.ToString() + "/" + mes.ToString() + "/" + año.ToString();
        }
    }
}
