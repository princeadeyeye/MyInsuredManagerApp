using System.ComponentModel.DataAnnotations;

namespace CoverGo.Task.Application.Models
{
    public class CreateProposalModel
    {
        [Required]
        public int Client { get; set; }
        public List<int>? InsuredGroups { get; set; }
    }
}
