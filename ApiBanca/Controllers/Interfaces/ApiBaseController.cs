using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiBanca.Controllers.Interfaces
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiBaseController: ControllerBase
    {
        private ISender mediator = null!;

        protected ISender Mediator => this.mediator ??= HttpContext.RequestServices.GetService<ISender>() ?? null!;
    }
}
