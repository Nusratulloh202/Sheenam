//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================

using Moq;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfGuestIsNullAndLogItAsync()
        {
            // given
            Guest nullGuest = null;

            var nullGuestExceptions = new NullGuestExceptions();

            var expectedGuestValidationException =
                new GuestValidationException(nullGuestExceptions);

            // when
            ValueTask<Guest>addGuestTask=this.guestService.AddGuestAsync(nullGuest);

            // then
            await Assert.ThrowsAsync<GuestValidationException>(() =>
                addGuestTask.AsTask());
            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionsAs(expectedGuestValidationException))),
                Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuestAsync(It.IsAny<Guest>()),
                Times.Never());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
