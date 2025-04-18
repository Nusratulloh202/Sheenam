﻿//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using FluentAssertions;
using Moq;
using Sheenam.Api.Models.Foundations.Enums;
using Sheenam.Api.Models.Foundations.Hosts;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions.BigExceptions;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions.SmallExceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Hosts
{
    public partial class HostServiceTests
    {
        [Fact]
        public async Task ShoulThrowValidationExceptionOnAddIfHostIsNullAndLogItAsync()
        {
            // given
            Host nullHost = null;
            var nullHostException = new NullHostException();

            var expectedHostValidationException =
                new HostValidationException(nullHostException);

            //when
            ValueTask<Host> taskAddHost =
                this.hostService.AddHostAsync(nullHost);

            HostValidationException actualHostValidationException =
                await Assert.ThrowsAsync<HostValidationException>(taskAddHost.AsTask);

            //then
            actualHostValidationException.Should()
                .BeEquivalentTo(expectedHostValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedHostValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertHostAsync(It.IsAny<Host>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfHostIsInvalidAndLogItAsync(string invalidText)
        {
            //given
            var invalidHost = new Host
            {
                FirstName = invalidText
            };

            var invalidHostException =
                new InvalidHostException();

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
                key: nameof(Host.DateOfBirth),
                values: "Date is required");
            invalidHostException.AddData(
                key: nameof(Host.Email),
                values: "Text is required");
            invalidHostException.AddData(
                key: nameof(Host.PhoneNumber),
                values: "Text is required");
            //invalidHostException.AddData(
            //key: nameof(Host.HostGender),
            //  values: "Value is invalid");
            var expectedValidationException =
                new HostValidationException(invalidHostException);

            //when
            ValueTask<Host> taskAddHost =
                this.hostService.AddHostAsync(invalidHost);

            HostValidationException actualHostValidationException =
                await Assert.ThrowsAsync<HostValidationException>(taskAddHost.AsTask);
            //then
            actualHostValidationException.Should()
                .BeEquivalentTo(expectedValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertHostAsync(It.IsAny<Host>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfGenderIsInvalidAndLogItAsync()
        {
            //given
            Host randomHost = CreateRandomHost();
            Host invalidHost = randomHost;
            invalidHost.HostGender = GetInvalidEnum<GenderType>();
            var invalidHostException = new InvalidHostException();
            invalidHostException.AddData(
                key: nameof(Host.HostGender),
                values: "Value is invalid");
            var expectedHostValidationException = new HostValidationException(invalidHostException);

            //when
            ValueTask<Host> addHostTask =
                         this.hostService.AddHostAsync(invalidHost);

            //then
            await Assert.ThrowsAsync<HostValidationException>(() =>
                    addHostTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedHostValidationException))),
                Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertHostAsync(It.IsAny<Host>()),
                Times.Never());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
