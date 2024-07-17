using Aplicacion.Dominio.Comunes;
using Aplicacion.Dominio.Entidades.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Dominio.Entidades.Cuenta
{
    public class Cuenta:EntidadBase
    {
        public string Numero { get; set; } = string.Empty;
        public double Saldo { get; set; }
        public int IdUsuario { get; set; }
        public bool Inactivo { get; set; } = false;
        public Usuario.Usuario Usuario { get; set; } = null!;

    }
}
