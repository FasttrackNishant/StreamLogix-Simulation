using System.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShipmentService.Application.UseCases.Shipments.GetById;
using ShipmentService.UseCases.Shipments.Dtos;

namespace ShipmentService.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ShipmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShipmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("/getById/{id}")]
    public async Task<ActionResult<ShipmentDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetShipmentByIdQuery(id));

        return Ok(result);
    }    
}
