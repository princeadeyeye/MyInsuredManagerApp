using CoverGo.Task.Application.Models;
using CoverGo.Task.Domain;

namespace CoverGo.Task.Application
{
    public interface IProposalWriteRepository
    {
        public ValueTask<Proposal> CreateNewProposal(CreateProposalModel createProposalModel, CancellationToken cancellationToken = default);

    }
}
