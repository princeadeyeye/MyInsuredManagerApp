using CoverGo.Task.Application;
using CoverGo.Task.Application.Models;
using CoverGo.Task.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CoverGo.Task.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProposalController : ControllerBase
{
    private readonly IProposalQuery _proposalQuery;
    private readonly IProposalWriteRepository _proposalWriteRepository;


    public ProposalController(IProposalWriteRepository proposalWriteRepository, IProposalQuery proposalQuery)
    {
        _proposalQuery = proposalQuery;
        _proposalWriteRepository = proposalWriteRepository;
    }

    [HttpPost(Name = "AddProposal")]
    public async ValueTask<ActionResult<Proposal>> AddProposal(CreateProposalModel createProposalModel)
    {
        return await _proposalWriteRepository.CreateNewProposal(createProposalModel);
    }

    [HttpGet("Premium")]
    public ActionResult<double> GetProposalAmount([FromQuery] int proposalId)
    {
        return _proposalQuery.GetProporsalAmount(proposalId);
    }


    [HttpGet("Discount")]
    public ActionResult<int> CreateDiscount([FromQuery] int proposalId)
    {
        return _proposalQuery.CreateDiscount(proposalId);
    }
}
