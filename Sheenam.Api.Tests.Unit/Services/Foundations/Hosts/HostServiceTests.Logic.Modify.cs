﻿//==================================================
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
        public async Task ShouldModifyHostAsync()
        {
            //given
            Host randomHost = CreateRandomHost();
            Host inputHost = randomHost;
            Host persistedHost = inputHost.DeepClone();
            Host updateHost = inputHost;
            Host expectedHost = updateHost.DeepClone();

            Guid InputHostId = inputHost.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectByIdHostAsync(InputHostId))
                .ReturnsAsync(persistedHost);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateHostAsync(inputHost))
                .ReturnsAsync(updateHost);

            //when
            Host actualHost = await this.hostService.ModifyHostAsync(inputHost);
            //then

            actualHost.Should().BeEquivalentTo(expectedHost);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectByIdHostAsync(InputHostId),
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateHostAsync(inputHost),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }
    }
}
