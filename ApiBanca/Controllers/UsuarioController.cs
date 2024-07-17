using ApiBanca.Controllers.Interfaces;
using Aplicacion.Caracteristicas.RegistroCivil;
using Aplicacion.Caracteristicas.Usuario;
using Aplicacion.Helper.Comunes;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace ApiBanca.Controllers
{
    public class UsuarioController : ApiBaseController
    {
        [HttpPost("crear-cuenta")]
        public async Task<ActionResult> CrearUsuario(CrearUsuario.CrearUsuarioDTO request)
        {
            try
            {
                var resultado = await Mediator.Send(new CrearUsuario.Comando(request));
                return Ok(resultado);
            }
            catch (ExcepcionValidacion error)
            {
                return BadRequest(new { Mensaje = error.Message, error.Errors });
            }
            catch (Exception error)
            {
                return BadRequest(new { Mensaje = error.Message });
            }
        }

        [HttpPut("verificar-cuenta")]
        public async Task<ActionResult> VerificarCuenta(VerificarCuenta.VerificarCuentaDTO request)
        {
            try
            {
                var resultado = await Mediator.Send(new VerificarCuenta.Comando(request));
                return Ok(resultado);
            }
            catch (ExcepcionValidacion error)
            {
                return BadRequest(new { Mensaje = error.Message, error.Errors });
            }
            catch (Exception error)
            {
                return BadRequest(new { Mensaje = error.Message });
            }
        }

        [Authorize]
        [HttpGet("obtener-cuentas")]
        public async Task<ActionResult> ObtenerCuentas()
        {
            try
            {
                var resultado = await Mediator.Send(new ObtenerCuentasPorUsuario.Consulta(int.Parse(User.Claims.First(x => x.Type == "id").Value)));
                return Ok(resultado);
            }
            catch (ExcepcionValidacion error)
            {
                return BadRequest(new { Mensaje = error.Message, error.Errors });
            }
            catch (Exception error)
            {
                return BadRequest(new { Mensaje = error.Message });
            }
        }
    }
}
