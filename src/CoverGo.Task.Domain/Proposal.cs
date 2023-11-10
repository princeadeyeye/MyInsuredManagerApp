namespace CoverGo.Task.Domain
{
    public class Proposal
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public double ProporsalCost { get; set; }
        public List<int>? InsuredGroupsId { get; set; }
    }
}
