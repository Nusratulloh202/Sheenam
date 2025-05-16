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
        public async Task ShouldRemoveHostAsync()
        {
            //given
            Host randomHost = CreateRandomHost();
            Host inputHost = randomHost;
            Host expectedRetrivedHostInput = inputHost;
            Host deletedHost = expectedRetrivedHostInput;
            Host expectedHost = deletedHost.DeepClone();
            Guid randomHostId = Guid.NewGuid();
            Guid inputHostId = randomHostId;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectByIdHostAsync(inputHostId))
                    .ReturnsAsync(inputHost);
            this.storageBrokerMock.Setup(broker =>
                broker.DeleteHostAsync(inputHost))
                    .ReturnsAsync(deletedHost);

            //when
            Host actualHost =
                await this.hostService.RemoveHostAsync(inputHostId);
            //then
            actualHost.Should().BeEquivalentTo(expectedHost);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectByIdHostAsync(inputHostId),
                Times.Once);
            this.storageBrokerMock.Verify(broker =>
                broker.DeleteHostAsync(expectedRetrivedHostInput),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }
    }
}
