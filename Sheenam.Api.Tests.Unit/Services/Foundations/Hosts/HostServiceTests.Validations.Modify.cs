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

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateHostAsync(It.IsAny<Host>()),
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task ShouldThrowValidationExceptionOnModifyIfHostIsInvalidAndLogItAsync(string invalidString)
        {
            //given
            Host invalidHost = new Host
            {
                FirstName = invalidString
            };
            var invalidHostException = new InvalidHostException();
            invalidHostException.AddData(
                key: nameof(Host.Id),
                values: "Id is required");
            invalidHostException.AddData(
                key: nameof(Host.FirstName),
                values: "Text is required");
            invalidHostException.AddData(
                key: nameof(Host.LastName),
                values: "Text is required");
            invalidHostException.AddData(
                key: nameof(Host.Email),
                values: "Text is required");
            invalidHostException.AddData(
                key: nameof(Host.PhoneNumber),
                values: "Text is required");
            invalidHostException.AddData(
                key: nameof(Host.DateOfBirth),
                values: "Date is required");

            var expectedHostValidationException =
                new HostValidationException(invalidHostException);
            //when
            ValueTask<Host> modifyHostTask = this.hostService.ModifyHostAsync(invalidHost);
            var actualHostValidationException = await
                Assert.ThrowsAsync<HostValidationException>(modifyHostTask.AsTask);
            //then
            actualHostValidationException.Should()
                .BeEquivalentTo(expectedHostValidationException);

            this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(
                SameExceptionAs(expectedHostValidationException))),
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
            broker.UpdateHostAsync(It.IsAny<Host>()),
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfHostDoesNotExistAndLogItAsync()
        {
            //given 
            Host randomHost = CreateRandomHost();
            Host notExistHost = randomHost;
            Host noHost = null;
            var notFoundHostException =
                new NotFoundHostException(notExistHost.Id);

            var expectedHostValidationException =
                new HostValidationException(notFoundHostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectByIdHostAsync(notExistHost.Id))
                    .ReturnsAsync(noHost);
            //when
            ValueTask<Host> modifyHostTask =
                this.hostService.ModifyHostAsync(notExistHost);
            HostValidationException actualHostValidationException =
                await Assert.ThrowsAsync<HostValidationException>(
                    modifyHostTask.AsTask);
            //then
            actualHostValidationException.Should()
                .BeEquivalentTo(expectedHostValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectByIdHostAsync(notExistHost.Id),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateHostAsync(It.IsAny<Host>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }
    }
}
