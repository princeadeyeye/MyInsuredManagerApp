namespace CoverGo.Task.Domain
{
    public class InsuredGroup
    {
        public int Id { get; set; }
        public int CoveredMembers { get; set; }
        public int InsurancePlan { get; set; }
        public string? Name { get; set; }
        public int Cost { get; set; }
    }
}
