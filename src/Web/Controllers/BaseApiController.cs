using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Result;

namespace Web.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => this._mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result is null)
            {
                return NotFound();
            }

            if (result.IsSuccess && result.Value != null)
            {
                return Ok(result.Value);
            }

            if (result.IsSuccess && result.Value == null)
            {
                return NotFound();
            }

            return BadRequest(result.Error);
        }
    }
}