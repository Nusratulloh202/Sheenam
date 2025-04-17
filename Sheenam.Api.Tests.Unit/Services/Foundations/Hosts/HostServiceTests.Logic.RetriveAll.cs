//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Sheenam.Api.Models.Foundations.Hosts;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Hosts
{
    public partial class HostServiceTests
    {
        [Fact]

        public void ShouldRetriveAllHosts()
        {
            //given 
            IQueryable<Host> randomHosts = CreateRandomHosts();
            IQueryable<Host> persistedHosts = randomHosts;
            IQueryable<Host> expectedHosts = persistedHosts.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllHosts()).Returns(persistedHosts);
            //when
            IQueryable<Host> actualHosts =
                this.hostService.RetriveAllHosts();
            //then
            actualHosts.Should().BeEquivalentTo(expectedHosts);
            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllHosts(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }
    }
}
