using BusTrackBackEnd.API.Companies.Domain.Model.Queries;
using BusTrackBackEnd.API.Companies.Domain.Services;
using BusTrackBackEnd.API.Companies.Domain.Model.Aggregates;
using BusTrackBackEnd.API.Companies.Interfaces.REST.Resources;
using BusTrackBackEnd.API.Companies.Interfaces.REST.Transform;
using BusTrackBackEnd.API.Shared.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BusTrackBackEnd.API.Companies.Interfaces.REST;

[ApiController]
[Route("api/v1/companies")]
[Route("companies")]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyCommandService _commandService;
    private readonly ICompanyQueryService _queryService;
    private readonly ICompanyRepository _companyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CompaniesController(
        ICompanyCommandService commandService,
        ICompanyQueryService queryService,
        ICompanyRepository companyRepository,
        IUnitOfWork unitOfWork)
    {
        _commandService = commandService;
        _queryService = queryService;
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
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

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCompany(int id, [FromBody] CreateCompanyResource resource)
    {
        var company = await _companyRepository.FindByIdAsync(id);
        if (company == null) return NotFound();

        company.UpdateDetails(resource.Name, resource.Email, resource.Ruc, resource.Phone, resource.Address);
        _companyRepository.Update(company);
        await _unitOfWork.CompleteAsync();

        return Ok(CompanyResourceFromEntityAssembler.ToResource(company));
    }
}