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
        public async Task ShouldAddHostAsync()
        {
            //given
            Host randomHost = CreateRandomHost();
            Host inputHost = randomHost;
            Host returningHost = inputHost;
            Host expectedHost = returningHost.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                 broker.InsertHostAsync(inputHost))
                .ReturnsAsync(returningHost);

            //when
            Host actualHost = await this.hostService.AddHostAsync(inputHost);

            //then
            actualHost.Should().BeEquivalentTo(expectedHost);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertHostAsync(inputHost),
                Times.Once);
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
