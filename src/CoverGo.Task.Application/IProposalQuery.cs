using CoverGo.Task.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoverGo.Task.Application
{
    public interface IProposalQuery
    {
        public double GetProporsalAmount(int proposalId);
        public IEnumerable<Proposal?> GetAllProposalsFromCache();
        public Proposal? GetProposalByIdFromCache(int proposalId);
        public bool DeleteProposal(int proposalId);
        public int CreateDiscount(int proposalId);

    }
}
