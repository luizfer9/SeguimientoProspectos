using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeguimientoProspectos.Models
{
    class ListaProspectos
    {
        private int idPros = 0;
        private string nombrePros = "";
        private string primerApe = "";
        private string segundoApe = "";
        private string estatus = "";

        public ListaProspectos(int idPros, string nombrePros, string primerApe, string segundoApe, string estatus)
        {
            this.idPros = idPros;
            this.nombrePros = nombrePros;
            this.primerApe = primerApe;
            this.segundoApe = segundoApe;
            this.estatus = estatus;
        }

        public int IdPros { get => idPros; set => idPros = value; }
        public string NombrePros { get => nombrePros; set => nombrePros = value; }
        public string PrimerApe { get => primerApe; set => primerApe = value; }
        public string SegundoApe { get => segundoApe; set => segundoApe = value; }
        public string Estatus { get => estatus; set => estatus = value; }
    }
}
