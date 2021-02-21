using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeguimientoProspectos.Models
{
    class Estatus
    {
        private int id_estatus;
        private string desc_estatus;
        private List<Estatus> estatusList = new List<Estatus>();

        public Estatus(int id_estatus, string desc_estatus)
        {
            this.id_estatus = id_estatus;
            this.desc_estatus = desc_estatus;
        }

        public int Id_estatus { get => id_estatus; set => id_estatus = value; }
        public string Desc_estatus { get => desc_estatus; set => desc_estatus = value; }
        internal List<Estatus> EstatusList { get => estatusList; set => estatusList = value; }
    }
}
