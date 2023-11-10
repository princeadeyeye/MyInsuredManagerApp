using CoverGo.Task.Application;
using CoverGo.Task.Application.Models;
using CoverGo.Task.Domain;
using CoverGo.Task.Infrastructure.Persistence.InMemory;
using Moq;
using Xunit;

namespace CoverGo.Task.Test
{
    public class ProposalRepositoryTest
    {
  

        [Fact]
        public async void CreateNewProposal_CreatesAndFetchesProposalById()
        {
            var clientId = 1;
            var proposalPlanIds = new List<int> { 1, 2, 3 };
            CreateProposalModel createProposalModel = new CreateProposalModel()
            {
                Client = clientId,
                InsuredGroups = proposalPlanIds
            };
            // Arrange
            var proposalService = new InMemoryProposalRespository();
            var newProposal = await proposalService.CreateNewProposal(createProposalModel);
            Assert.NotNull(newProposal);
            var fetchedProposal = proposalService.GetProposalByIdFromCache(newProposal.Id);

            // Assert
            Assert.NotNull(fetchedProposal);
            Assert.Equal(newProposal.Id, fetchedProposal.Id);
            Assert.Equal(newProposal.ClientId, fetchedProposal.ClientId);

            bool deleteProposal = proposalService.DeleteProposal(newProposal.Id);
            var refetchedProposal = proposalService.GetProposalByIdFromCache(newProposal.Id);
            Assert.Null(refetchedProposal);
            Assert.True(deleteProposal);
        }


        [Fact]
        public async void FetchesProposalPremiumById()
        {
            var clientId = 1;
            var proposalPlanIds = new List<int> { 1, 2, 3 };
            CreateProposalModel createProposalModel = new CreateProposalModel()
            {
                Client = clientId,
                InsuredGroups = proposalPlanIds
            };
            // Arrange
            var proposalService = new InMemoryProposalRespository();
            var newProposal = await proposalService.CreateNewProposal(createProposalModel);
            Assert.NotNull(newProposal);
            var fetchedProposalPremium = proposalService.GetProporsalAmount(newProposal.Id);
            var refetchedProposal = proposalService.GetProposalByIdFromCache(newProposal.Id);

            // Assert
            Assert.Equal(newProposal.ProporsalCost, fetchedProposalPremium);
            Assert.Equal(refetchedProposal?.ProporsalCost, fetchedProposalPremium);

        }

        [Fact]
        public async void CreateProposal_Discount()
        {
            var clientId = 1;
            var proposalPlanIds = new List<int> { 1, 2, 3 };
            CreateProposalModel createProposalModel = new CreateProposalModel()
            {
                Client = clientId,
                InsuredGroups = proposalPlanIds
            };
            // Arrange
            var proposalService = new InMemoryProposalRespository();
            var newProposal = await proposalService.CreateNewProposal(createProposalModel);
            Assert.NotNull(newProposal);
            var fetchedProposalDiscount = proposalService.CreateDiscount(newProposal.Id);
            var refetchedProposal = proposalService.GetProposalByIdFromCache(newProposal.Id);

            // Assert
            Assert.NotEqual(newProposal.ProporsalCost, fetchedProposalDiscount);

        }

    }
}
