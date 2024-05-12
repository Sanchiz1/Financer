using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Result;
using Web.Infrastructure;

namespace Web.Controllers;
[ApiController]
[Route("/api/[controller]")]
public class BaseApiController : ControllerBase
{
    private IMediator _mediator;

    protected IMediator Mediator => this._mediator ??= this.HttpContext.RequestServices.GetService<IMediator>()!;

    protected string UserId => this.User.GetUserId().ToString();

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

    protected ActionResult HandleResult(Result result)
    {
        if (result is null)
        {
            return NotFound();
        }

        if (result.IsSuccess)
        {
            return Ok();
        }

        return BadRequest(result.Error);
    }
}