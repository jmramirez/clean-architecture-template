using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Travel.WebApi.Controllers;

[ApiController]
[Route("api/controller")]
public class ApiController : ControllerBase
{
    private IMediator _mediator = null!;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;
}