//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Guest randomGuest = CreateRandomGuest();
            Guest someGuest = randomGuest;
            Guid guestId = someGuest.Id;
            SqlException sqlException = GetSqlException();

            var failedGuestStorageException =
                new FailedGuestStorageException(sqlException);

            var excpectedGuestDependencyException = 
                new GuestDependencyException(failedGuestStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(guestId)).Throws(sqlException);

            //when
            ValueTask<Guest> modifyGuestTask = 
                this.guestService.ModifyGuestAsync(someGuest);

            GuestDependencyException actualGuestDependencyException =
                await Assert.ThrowsAsync<GuestDependencyException>(modifyGuestTask.AsTask);

            //then
            actualGuestDependencyException.Should()
                .BeEquivalentTo(excpectedGuestDependencyException);

            this.loggingBrokerMock.Verify(broker=>
                broker.LogCritical(It.Is(SameExceptionAs(excpectedGuestDependencyException))),
                Times.Once());

            this.storageBrokerMock.Verify(broker=>
                broker.SelectGuestByIdAsync(guestId),
                Times.Once());

            this.storageBrokerMock.Verify(broker=>
                broker.UpdateGuestAsync(someGuest),Times.Never());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
