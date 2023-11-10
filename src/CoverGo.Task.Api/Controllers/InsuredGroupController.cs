using CoverGo.Task.Application;
using CoverGo.Task.Application.Models;
using CoverGo.Task.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CoverGo.Task.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class InsuredGroupController : ControllerBase
{
    private readonly IInsuredGroupQuery _insuredGroupQuery;
    private readonly IInsuredWriteRepository _insuredWriteRepository;


    public InsuredGroupController(IInsuredWriteRepository insuredWriteRepository, IInsuredGroupQuery insuredGroupQuery)
    {
        _insuredGroupQuery = insuredGroupQuery;
        _insuredWriteRepository = insuredWriteRepository;
    }

    [HttpPost(Name = "CreateInsuredGroup")]
    public async ValueTask<ActionResult<InsuredGroup>> CreateInsuredGroup(CreateInsuredGroupModel createInsuredGroupModel)
    {
        return await _insuredWriteRepository.CreateInsuredGroup(createInsuredGroupModel);
    }
}
