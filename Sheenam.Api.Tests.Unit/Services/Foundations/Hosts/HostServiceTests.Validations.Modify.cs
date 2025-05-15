﻿//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using FluentAssertions;
using Moq;
using Sheenam.Api.Models.Foundations.Hosts;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions.BigExceptions;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions.SmallExceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Hosts
{
    public partial class HostServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfHostIsNullAndLogItAsync()
        {
            //given
            Host nullHost = null;
            var nullHostExceptions =
                new NullHostException();
            var expectedHostValidationException =
                new HostValidationException(nullHostExceptions);
            //when
            ValueTask<Host> modifyHostTask = this.hostService.ModifyHostAsync(nullHost);

            HostValidationException actualHostValidationException = 
                await Assert.ThrowsAsync<HostValidationException>(modifyHostTask.AsTask);

            //then
            actualHostValidationException.Should()
                .BeEquivalentTo(expectedHostValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker=>
                broker.UpdateHostAsync(It.IsAny<Host>()), 
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
