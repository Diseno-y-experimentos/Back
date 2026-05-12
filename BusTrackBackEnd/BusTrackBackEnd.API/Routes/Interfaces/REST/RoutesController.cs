using BusTrackBackEnd.API.Routes.Domain.Model.Entities;
using BusTrackBackEnd.API.Routes.Domain.Model.Queries;
using BusTrackBackEnd.API.Routes.Domain.Repositories;
using BusTrackBackEnd.API.Routes.Domain.Services;
using BusTrackBackEnd.API.Routes.Interfaces.REST.Resources;
using BusTrackBackEnd.API.Routes.Interfaces.REST.Transform;
using BusTrackBackEnd.API.Shared.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BusTrackBackEnd.API.Routes.Interfaces.REST;

[ApiController]
[Route("api/v1/routes")]
[Route("routes")]
public class RoutesController : ControllerBase
{
    private readonly IRouteCommandService _commandService;
    private readonly IRouteQueryService _queryService;
    private readonly IRouteRepository _routeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RoutesController(
        IRouteCommandService commandService,
        IRouteQueryService queryService,
        IRouteRepository routeRepository,
        IUnitOfWork unitOfWork)
    {
        _commandService = commandService;
        _queryService = queryService;
        _routeRepository = routeRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRouteResource resource)
    {
        var command = CreateRouteCommandFromResourceAssembler.ToCommand(resource);
        var id = await _commandService.Handle(command);
        var createdRoute = await _queryService.Handle(new GetRouteByIdQuery(id));

        if (createdRoute == null) return NotFound();

        return Ok(RouteResourceFromEntityAssembler.ToResource(createdRoute));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery(Name = "name_like")] string? nameLike)
    {
        var list = await _queryService.Handle(new GetAllRoutesQuery());

        if (!string.IsNullOrWhiteSpace(nameLike))
        {
            list = list.Where(r => r.Name.Contains(nameLike, StringComparison.OrdinalIgnoreCase));
        }

        return Ok(list.Select(RouteResourceFromEntityAssembler.ToResource));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var route = await _queryService.Handle(new GetRouteByIdQuery(id));
        if (route == null) return NotFound();
        return Ok(RouteResourceFromEntityAssembler.ToResource(route));
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var route = await _routeRepository.FindByNameAsync(name);
        if (route == null) return NotFound();
        return Ok(RouteResourceFromEntityAssembler.ToResource(route));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateRouteResource resource)
    {
        var route = await _routeRepository.FindByIdAsync(id);
        if (route == null) return NotFound();

        route.UpdateDetails(resource.Name, resource.CompanyId, resource.EstimatedTime, resource.Frequency);

        var waypoints = (resource.Waypoints ?? new List<WaypointResource>())
            .Select(w => new Waypoint(w.Name, w.Latitude, w.Longitude, w.Order));

        route.ReplaceWaypoints(waypoints);

        await _unitOfWork.CompleteAsync();

        return Ok(RouteResourceFromEntityAssembler.ToResource(route));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var route = await _routeRepository.FindByIdAsync(id);
        if (route == null) return NotFound();

        _routeRepository.Remove(route);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }
}