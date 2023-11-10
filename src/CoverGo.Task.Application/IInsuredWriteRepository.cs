using CoverGo.Task.Application.Models;
using CoverGo.Task.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoverGo.Task.Application
{
    public interface IInsuredWriteRepository
    {
        public ValueTask<InsuredGroup> CreateInsuredGroup(CreateInsuredGroupModel createInsuredGroupModel, CancellationToken cancellationToken = default);

    }
}
