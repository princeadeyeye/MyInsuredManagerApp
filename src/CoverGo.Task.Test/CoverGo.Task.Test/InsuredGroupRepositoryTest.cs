using CoverGo.Task.Application.Models;
using CoverGo.Task.Infrastructure.Persistence.InMemory;
using Xunit;

namespace CoverGo.Task.Test
{
    public class InsuredGroupRepositoryTest
    {
        [Fact]
        public async void CreateNewInsuredGroup_DeleteAndFetchesInsuredGroupById()
        {
            CreateInsuredGroupModel createNewInsuredgroup = new CreateInsuredGroupModel()
            {
                Cost = 100,
                MembersCount = 23,
                Name = "Test-InsuredGroup",
                Plan = 1
            };
            var inMemoryInsureGroupService = new InMemoryInsuredGroupRespository();
            var newInsuredGroup = await inMemoryInsureGroupService.CreateInsuredGroup(createNewInsuredgroup);
            Assert.NotNull(newInsuredGroup);
            var fetchInsuredGroup = inMemoryInsureGroupService.GetInsuredByIdFromCache(newInsuredGroup.Id);

            // Assert
            Assert.NotNull(fetchInsuredGroup);
            Assert.Equal(newInsuredGroup.Id, fetchInsuredGroup.Id);
            Assert.Equal(newInsuredGroup.Cost, fetchInsuredGroup.Cost);

            bool deletedInsuredGroup = inMemoryInsureGroupService.DeleteInsuredGroupById(newInsuredGroup.Id);
            var refetchedInsuredGroup = inMemoryInsureGroupService.GetInsuredByIdFromCache(newInsuredGroup.Id);
            Assert.Null(refetchedInsuredGroup);
            Assert.True(deletedInsuredGroup);
        }


        [Fact]
        public async void CreateNewInsuredGroup()
        {
            CreateInsuredGroupModel createNewInsuredgroup = new CreateInsuredGroupModel()
            {
                Cost = 100,
                MembersCount = 23,
                Name = "Test-InsuredGroup I",
                Plan = 2
            };
            var inMemoryInsureGroupService = new InMemoryInsuredGroupRespository();
            var newInsuredGroup = await inMemoryInsureGroupService.CreateInsuredGroup(createNewInsuredgroup);
            Assert.NotNull(newInsuredGroup);
         
            }

        }
    }
