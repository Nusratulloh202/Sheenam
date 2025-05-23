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
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvaledAndLogItAsync()
        {
            // given
            Guid invalidId = Guid.Empty;
            var invalidGuestIdException = new InvalidGuestException();
            invalidGuestIdException.AddData(
                key: nameof(Guest.Id),
                values: "Id is required");
            var expectedGuestValidationException = new
                GuestValidationException(invalidGuestIdException);
            //when
            ValueTask<Guest> removeGuestTask =
                this.guestService.RemoveGuestByIdAsync(invalidId);

            GuestValidationException actualGuestValidationException =
                await Assert.ThrowsAsync<GuestValidationException>(removeGuestTask.AsTask);
            //then
            actualGuestValidationException.Should().BeEquivalentTo(expectedGuestValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuestValidationException))),
                Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(It.IsAny<Guid>()),
                Times.Never());
            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGuestAsync(It.IsAny<Guest>()),
                Times.Never());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRemoveGuestByIdIsNotFoundAndLogItAsync()
        {
            // given
            Guid inputGuid = Guid.NewGuid();
            Guest noGuest = null;

            var notFoundGuestException =
                new NotFoundGuestException(inputGuid);

            var expectedGuestValidationException =
                new GuestValidationException(notFoundGuestException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(inputGuid))
                    .ReturnsAsync(noGuest);

            // when
            ValueTask<Guest> removeGuestTask =
                this.guestService.RemoveGuestByIdAsync(inputGuid);

            var actualGuestValidationException =
                await Assert.ThrowsAsync<GuestValidationException>(
                    removeGuestTask.AsTask);

            // then
            actualGuestValidationException.Should().BeEquivalentTo(
                expectedGuestValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(inputGuid),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGuestAsync(It.IsAny<Guest>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();


        }
    }
}
