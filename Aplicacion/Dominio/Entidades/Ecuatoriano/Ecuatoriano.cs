using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Dominio.Entidades.RegistroCivil
{
    public class Ecuatoriano
    {
        public int Id { get; set; }
        public string Cedula { get; set; } = string.Empty;
        public string CodigoDactilar { get; set; } = string.Empty;
    }
}
