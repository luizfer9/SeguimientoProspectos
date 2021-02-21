using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeguimientoProspectos.Models
{
    class Documentos
    {
        private string nombreDoc;
        private string archivoDoc;
        private byte[] streamData;

        public Documentos(string nombreDoc, string archivoDoc, byte[] streamData)
        {
            this.nombreDoc = nombreDoc;
            this.archivoDoc = archivoDoc;
            this.streamData = streamData;
        }

        public string NombreDoc { get => nombreDoc; set => nombreDoc = value; }
        public string ArchivoDoc { get => archivoDoc; set => archivoDoc = value; }
        public byte[] StreamData { get => streamData; set => streamData = value; }
    }
}
