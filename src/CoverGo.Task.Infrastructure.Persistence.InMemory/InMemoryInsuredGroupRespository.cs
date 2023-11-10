using CoverGo.Task.Application;
using CoverGo.Task.Application.Models;
using CoverGo.Task.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace CoverGo.Task.Infrastructure.Persistence.InMemory
{
    public class InMemoryInsuredGroupRespository: IInsuredWriteRepository, IInsuredGroupQuery
    {
        private readonly ObjectCache insureGroupCache = MemoryCache.Default;

        string cacheKey = "insuredGroup_";

        public ValueTask<InsuredGroup> CreateInsuredGroup(CreateInsuredGroupModel createInsuredGroupModel, CancellationToken cancellationToken = default)
        {
            int lastCacheInsuredGroupId = GetLastInsuredGroupById();
            int newInsuredGroupId = lastCacheInsuredGroupId + 1;

            InsuredGroup insuredGroup = new InsuredGroup()
            {
                Id = newInsuredGroupId,
                Cost = createInsuredGroupModel.Cost,
                InsurancePlan = createInsuredGroupModel.Plan,
                Name = createInsuredGroupModel.Name,
                CoveredMembers = createInsuredGroupModel.MembersCount
                
            };
            string insuredGroupCacheKey = $"{cacheKey}{newInsuredGroupId}";

            insureGroupCache.Add(insuredGroupCacheKey, insuredGroup, new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30) // Cache for 30 minutes
            });
            return ValueTask.FromResult(insuredGroup);
        }

        public InsuredGroup? GetInsuredByIdFromCache(int induredGroupId)
        {
            InsuredGroup? insuredGroup = null;
            string groupedInsuredCacheKey = $"{cacheKey}{induredGroupId}";
            if (insureGroupCache.Contains(groupedInsuredCacheKey))
            {
                var cachedInsuredGroup = insureGroupCache.Get(groupedInsuredCacheKey) as InsuredGroup;
                if (cachedInsuredGroup != null) insuredGroup = cachedInsuredGroup;
            }
            return insuredGroup;

        }
        public int GetLastInsuredGroupById()
        {

            var allCacheItems = insureGroupCache.Cast<KeyValuePair<string, object>>().ToList();
            var insuredGroupCahe= allCacheItems
                .Where(item => item.Key.StartsWith(cacheKey) && item.Value is InsuredGroup)
                .Select(item => item.Value as InsuredGroup);

            int lastCahcheId = insuredGroupCahe.Any() ? insuredGroupCahe.Max(p => p?.Id ?? 0) : 0;

            return lastCahcheId;
        }

        public bool DeleteInsuredGroupById(int insuredGroupId)
        {
            bool isDeleted = false;
            string insuredGroupCacheKey = $"{cacheKey}{insuredGroupId}";

            if (insureGroupCache.Get(insuredGroupCacheKey) is not null)
            {
                insureGroupCache.Remove(insuredGroupCacheKey);
                isDeleted = true;
            }
            return isDeleted;
        }

      
        

    }
}
