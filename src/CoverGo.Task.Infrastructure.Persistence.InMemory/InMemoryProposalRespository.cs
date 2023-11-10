using CoverGo.Task.Application;
using CoverGo.Task.Application.Models;
using CoverGo.Task.Domain;
using System;
using System.Runtime.Caching;

namespace CoverGo.Task.Infrastructure.Persistence.InMemory
{
    public class InMemoryProposalRespository: IProposalWriteRepository, IProposalQuery
    {
        private readonly ObjectCache proposalCache = MemoryCache.Default;
        string cacheKey = "proposal_";
        string insuredCacheKey = "insuredGroup_";


        public ValueTask<Proposal> CreateNewProposal(CreateProposalModel createProposalModel, CancellationToken cancellationToken = default)
        {
            // Get the last proposal ID and increment it
            int lastProposalId = GetLastProposalId();
            int newProposalId = lastProposalId + 1;


            Proposal newProposal = new Proposal()
            {
                Id = newProposalId,
                ProporsalCost = CalculateProporsalPremium(createProposalModel.InsuredGroups),
                ClientId = createProposalModel.Client,
                InsuredGroupsId = createProposalModel.InsuredGroups
            };
            string proposalCacheKey = $"{cacheKey}{newProposalId}";

            proposalCache.Add(proposalCacheKey, newProposal, new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30) // Cache for 30 minutes
            });
            return ValueTask.FromResult(newProposal);
        }

        public double GetProporsalAmount(int proposalId)
        {
            double proposalCost = 0;
            string proposalCacheKey = $"{cacheKey}{proposalId}";
            if (proposalCache.Contains(proposalCacheKey))
            {
                var cachedProposal = proposalCache.Get(proposalCacheKey) as Proposal;
                if (cachedProposal != null) proposalCost = cachedProposal.ProporsalCost;
            }
            return proposalCost;
        }

        private int CalculateProporsalPremium(List<int>? requestList)
        {
            int totalCost = 0;
            ObjectCache insureGroupCache = MemoryCache.Default;
            string insuredCacheKey = "insuredGroup_";

            var allCacheItems = insureGroupCache.Cast<KeyValuePair<string, object>>().ToList();
            var insureGroupItems = allCacheItems
                .Where(item => item.Key.StartsWith(insuredCacheKey) && item.Value is InsuredGroup)
                .Select(item => item.Value as InsuredGroup);

            if(requestList is null  || insureGroupItems is null) return totalCost;
            var selectedPlans = insureGroupItems.Where(plan => requestList.Contains(plan?.Id ?? 0)).ToList();
            totalCost = selectedPlans.Sum(plan => plan?.Cost ?? 0);

            return totalCost;

        }

        public IEnumerable<Proposal?> GetAllProposalsFromCache()
        {
            var allCacheItems = proposalCache.Cast<KeyValuePair<string, object>>().ToList();
            var proposalItems = allCacheItems
                .Where(item => item.Key.StartsWith(cacheKey) && item.Value is Proposal)
                .Select(item => item.Value as Proposal);

            return proposalItems;
        }

        public Proposal? GetProposalByIdFromCache(int proposalId)
        {
            Proposal? proposal = null;
            string proposalCacheKey = $"{cacheKey}{proposalId}";
            if (proposalCache.Contains(proposalCacheKey))
            {
                var cachedProposal = proposalCache.Get(proposalCacheKey) as Proposal;
                if (cachedProposal != null) proposal = cachedProposal;
            }  
            return proposal;

        }
        private int GetLastProposalId()
        {

            // Get all items from the cache
            var allCacheItems = proposalCache.Cast<KeyValuePair<string, object>>().ToList();

            // Filter out items of type Proposal
            var proposalItems = allCacheItems
                .Where(item => item.Key.StartsWith(cacheKey) && item.Value is Proposal)
                .Select(item => item.Value as Proposal);

            // Find the maximum proposal ID
            int lastProposalId = proposalItems.Any() ? proposalItems.Max(p => p?.Id ?? 0) : 0;

            return lastProposalId;
        }

        public bool DeleteProposal(int proposalId)
        {
            bool isDeleted = false;
            string proposalCacheKey = $"{cacheKey}{proposalId}";

            if (proposalCache.Get(proposalCacheKey) is not null)
            {
                proposalCache.Remove(proposalCacheKey);
                isDeleted = true;
            }
            return isDeleted;
        }

        public int CreateDiscount(int proposalId)
        {
            var discount = 0;
            Proposal? proposal = GetProposalByIdFromCache(proposalId);
            if(proposal is null) return discount;
            var proposalInsuredGroups = proposal.InsuredGroupsId;
            if (proposalInsuredGroups != null)
            {
                var allCacheItems = proposalCache.Cast<KeyValuePair<string, object>>().ToList();
                var insureGroupItems = allCacheItems
                    .Where(item => item.Key.StartsWith(insuredCacheKey) && item.Value is InsuredGroup)
                    .Select(item => item.Value as InsuredGroup);
                if (insureGroupItems is null) return discount;

                var selectedPlans = insureGroupItems.Where(plan => proposalInsuredGroups.Contains(plan?.Id ?? 0)).ToList();

                discount = selectedPlans.Sum(plan => plan != null ? plan.Cost - CalculateSingleCost(plan) : 0);

            }
            return discount;

        }

        public int CalculateSingleCost(InsuredGroup insuredGroup)
        {
            int cost = 0;
            if (insuredGroup.Cost == 0) cost = 0;
            else cost = insuredGroup.Cost / insuredGroup.CoveredMembers;
            return cost;
        }
    }
}
