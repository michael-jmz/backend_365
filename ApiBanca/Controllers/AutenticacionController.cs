using ApiBanca.Controllers.Interfaces;
using Aplicacion.Caracteristicas.Autenticacion;
using Aplicacion.Caracteristicas.Usuario;
using Aplicacion.Dominio.Entidades.Usuario;
using Aplicacion.Helper.Comunes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiBanca.Controllers
{
    public class AutenticacionController : ApiBaseController
    {
        private readonly IConfiguration configuracion;

        public AutenticacionController(IConfiguration configuracion)
        {
            this.configuracion = configuracion;
        }
        [HttpPost("login")]
        public async Task<ActionResult> InciarSesion(IniciarSesion.IniciarSesionDTO request)
        {
            try
            {
                var resultado = await Mediator.Send(new IniciarSesion.Comando(request));
                return Ok(new { usuario= resultado, JWT = GenerarToken(resultado)});
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

        private string GenerarToken(IniciarSesion.RespuestaDTO usuario)
        {

            var claims = new[]
            {
                    new Claim("id", usuario.Id.ToString()),
                    new Claim("email", usuario.Email),
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuracion["SECRETO"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddDays(7);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
