using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoverGo.Task.Application.Models
{
    public class CreateInsuredGroupModel
    {
        public int MembersCount { get; set; }
        public int Plan { get; set; }
        public string? Name { get; set; }
        public int Cost { get; set; }
    }
}
