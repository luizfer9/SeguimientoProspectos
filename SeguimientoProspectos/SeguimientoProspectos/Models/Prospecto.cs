using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeguimientoProspectos.Models
{
    class Prospecto
    {
        private string nombrePros = "";
        private string primerApe = "";
        private string segundoApe = "";
        private string callePros = "";
        private string numeroCasa = "";
        private string colonia = "";
        private Int64 codePost = 0;
        private Int64 telPros = 0;
        private string rfc = "";
        private List<Models.Documentos> docsList = null;
        private int numEstatus = 0;
        private int numDocs = 0;
        private string obsrv = "";

        public Prospecto(string nombrePros, string primerApe, string segundoApe, string callePros, string numeroCasa, string colonia, Int64 codePost, Int64 telPros, string rfc, List<Models.Documentos> docsList)
        {
            this.nombrePros = nombrePros;
            this.primerApe = primerApe;
            this.segundoApe = segundoApe;
            this.callePros = callePros;
            this.numeroCasa = numeroCasa;
            this.colonia = colonia;
            this.codePost = codePost;
            this.telPros = telPros;
            this.rfc = rfc;
            this.docsList = docsList;
        }

        public string NombrePros { get => nombrePros; set => nombrePros = value; }
        public string PrimerApe { get => primerApe; set => primerApe = value; }
        public string SegundoApe { get => segundoApe; set => segundoApe = value; }
        public string CallePros { get => callePros; set => callePros = value; }
        public string NumeroCasa { get => numeroCasa; set => numeroCasa = value; }
        public string Colonia { get => colonia; set => colonia = value; }
        public Int64 CodePost { get => codePost; set => codePost = value; }
        public Int64 TelPros { get => telPros; set => telPros = value; }
        public string rfc1 { get => rfc; set => rfc = value; }
        public int NumEstatus { get => numEstatus; set => numEstatus = value; }
        public int NumDocs { get => numDocs; set => numDocs = value; }
        public string Obsrv { get => obsrv; set => obsrv = value; }
        internal List<Documentos> DocsList { get => docsList; set => docsList = value; }
        public void clearProspecto()
        {
            NombrePros = "";
            PrimerApe = "";
            SegundoApe = "";
            CallePros = "";
            NumeroCasa = "";
            Colonia = "";
            CodePost = 0;
            TelPros = 0;
            rfc1 = "";
            DocsList = null;
            numDocs = 0;
            numEstatus = 0;
            obsrv = "";
        }
    }
}
