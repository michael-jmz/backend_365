using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Infraestructura.RegistroCivil.Interfaces
{
    public interface IRegistroCivil
    {
        Task<bool> ValidarDatosCedula(string cedula, string codigoDactilar);

        Task<bool> ValidarRostro(string rostroBase64);
    }
}
