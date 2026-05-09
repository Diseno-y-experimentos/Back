using BusTrackBackEnd.API.Companies.Domain.Model.Queries;
using BusTrackBackEnd.API.Companies.Domain.Services;
using BusTrackBackEnd.API.Companies.Interfaces.REST.Resources;
using BusTrackBackEnd.API.Companies.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace BusTrackBackEnd.API.Companies.Interfaces.REST;

[ApiController]
[Route("api/v1/companies")]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyCommandService _commandService;
    private readonly ICompanyQueryService _queryService;

    public CompaniesController(
        ICompanyCommandService commandService,
        ICompanyQueryService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompany(
        [FromBody] CreateCompanyResource resource)
    {
        var command = CreateCompanyCommandFromResourceAssembler.ToCommand(resource);
        await _commandService.Handle(command);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _queryService.Handle(new GetAllCompaniesQuery());
        return Ok(list.Select(CompanyResourceFromEntityAssembler.ToResource));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var company = await _queryService.Handle(new GetCompanyByIdQuery(id));
        if (company == null) return NotFound();
        return Ok(CompanyResourceFromEntityAssembler.ToResource(company));
    }
}