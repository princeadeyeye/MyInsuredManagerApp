using CoverGo.Task.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoverGo.Task.Application
{
    public interface IInsuredGroupQuery
    {
        public InsuredGroup? GetInsuredByIdFromCache(int induredGroupId);
        public int GetLastInsuredGroupById();
        public bool DeleteInsuredGroupById(int insuredGroupId);
    }
}
