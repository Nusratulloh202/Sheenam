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
        public async Task ShouldRetrieveByIdHostAsync()
        {
            //given
            Guid randomHostId = Guid.NewGuid();
            Guid inputHostId = randomHostId;
            Host randomHost = CreateRandomHost();
            Host persistedHost = randomHost;
            Host expectedHost = persistedHost.DeepClone();

            this.storageBrokerMock.Setup(broker =>
            broker.SelectByIdHostAsync(inputHostId))
                .ReturnsAsync(persistedHost);
            //when
            Host actualHost = await this.hostService.RetrieveByIdHostAsync(inputHostId);
            //then
            actualHost.Should().BeEquivalentTo(expectedHost);

            this.storageBrokerMock.Verify(broker =>
            broker.SelectByIdHostAsync(inputHostId), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        } 
    }
}
