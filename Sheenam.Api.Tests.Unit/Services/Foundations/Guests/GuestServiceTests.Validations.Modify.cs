﻿//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using FluentAssertions;
using Moq;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfGuestIsNullAndLogItAsync()
        {
            //given
            Guest nullGuest = null;

            var nullGuestExceptions = 
                new NullGuestExceptions();

            var expectedGuestValidationException = 
                new GuestValidationException(nullGuestExceptions);

            //when
            ValueTask<Guest> modifyGuestTask = 
                this.guestService.ModifyGuestAsync(nullGuest);

            GuestValidationException actualGuestValidationExcception =
                await Assert.ThrowsAsync<GuestValidationException>(
                    modifyGuestTask.AsTask);

            //then
            actualGuestValidationExcception.Should().BeEquivalentTo(expectedGuestValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(
                    SameExceptionAs(expectedGuestValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGuestAsync(It.IsAny<Guest>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionModfiyIfGuestIsInvalidAndLogItAsync(string invalidString)
        {
            Guest invalidGuest = new Guest
            {
                FirstName = invalidString
            };
            var invalidGuestException = new InvalidGuestException();

            invalidGuestException.AddData(
                key: nameof(Guest.Id),
                values: "Id is requered");
            invalidGuestException.AddData(
               key: nameof(Guest.FirstName),
               values: "Text is required");

            invalidGuestException.AddData(
                key: nameof(Guest.LastName),
                values: "Text is required");

            invalidGuestException.AddData(
                key: nameof(Guest.DateOffBirth),
                values: "Date is required");

            invalidGuestException.AddData(
                key: nameof(Guest.PhoneNumber),
                values: "Text is required");

            invalidGuestException.AddData(
                key: nameof(Guest.Email),
                values: "Text is required");

            invalidGuestException.AddData(
                key: nameof(Guest.Address),
                values: "Text is required");

            var expectedGuestValidationException = 
                new GuestValidationException(invalidGuestException);

            //when
            ValueTask<Guest> modifyGuestTask = 
                this.guestService.ModifyGuestAsync(invalidGuest);

            GuestValidationException actualGuestValidationException =
                await Assert.ThrowsAsync<GuestValidationException>(modifyGuestTask.AsTask);

            //then
            actualGuestValidationException.Should()
                .BeEquivalentTo(expectedGuestValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuestValidationException))), 
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGuestAsync(It.IsAny<Guest>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

    }
}
