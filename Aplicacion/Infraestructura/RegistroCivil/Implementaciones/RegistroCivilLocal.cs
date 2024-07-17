using Aplicacion.Infraestructura.Persistencia;
using Aplicacion.Infraestructura.RegistroCivil.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Infraestructura.RegistroCivil.Implementaciones
{
    public class RegistroCivilLocal : IRegistroCivil
    {
        private readonly ContextoDB contexto;
        public RegistroCivilLocal(ContextoDB contexto)
        {
            this.contexto = contexto;
        }

        public async Task<bool> ValidarDatosCedula(string cedula, string codigoDactilar)
        {
            var ecuatoriano = await contexto.Ecuatoriano.FirstOrDefaultAsync(x => x.Cedula == cedula && x.CodigoDactilar == codigoDactilar);
            return ecuatoriano != null;
        }

        private bool ValidarCedula(string cedula)
        {
            if (!SoloNumeros(cedula))
            {
                return false;
            }

            if (cedula.Length != 10 && cedula.Length != 13)
            {
                return false;
            }

            List<int> digitos = new();
            for (int i = 0; i < cedula.Length; i++)
            {
                digitos.Add(int.Parse(cedula[i].ToString()));
            }
            List<int> posicionesImpares = new List<int>();
            List<int> posicionesPares = new List<int>();

            for (int i = 0; i < 9; i++)
            {
                if (i % 2 == 0)
                {
                    if ((digitos[i] * 2) > 9)
                    {
                        posicionesImpares.Add((digitos[i] * 2) - 9);
                    }
                    else
                    {
                        posicionesImpares.Add(digitos[i] * 2);
                    }

                }
                else
                {
                    posicionesPares.Add(digitos[i]);
                }
            }

            int suma = posicionesPares.Sum() + posicionesImpares.Sum();
            int modulo = suma % 10;
            int digitoVerificador = 0;
            if (modulo > 0)
            {
                digitoVerificador = 10 - modulo;
            }

            if (digitos[9] != digitoVerificador)
            {
                return false;
            }

            if (cedula.Length == 13)
            {
                if (digitos[10] != 0 || digitos[11] != 0 || digitos[12] != 1)
                {
                    return false;
                }
            }

            return true;
        }

        private bool ValidarCodigoDactilar(string codigoDactilar)
        {
            if (codigoDactilar.IsNullOrEmpty()) return false;
            if (codigoDactilar.Length != 10) return false;
            return true;
        }

        private bool SoloNumeros(string cadena)
        {
            for (int i = 0; i < cadena.Length; i++)
            {
                if (!char.IsNumber(cadena[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> ValidarRostro(string base64)
        {
            if (base64.Length % 4 != 0) return false;
            try
            {
                byte[] bytes = Convert.FromBase64String(base64);
                string base64String = Convert.ToBase64String(bytes);
                return base64String.TrimEnd('=') == base64.TrimEnd('=');
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
