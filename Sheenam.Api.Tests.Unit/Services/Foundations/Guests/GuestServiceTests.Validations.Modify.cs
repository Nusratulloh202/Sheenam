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
    }
}
