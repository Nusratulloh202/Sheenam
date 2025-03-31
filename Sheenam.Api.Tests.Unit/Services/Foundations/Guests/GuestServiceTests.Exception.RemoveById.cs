﻿//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            //given
            Guid someGuestId = Guid.NewGuid();
            var dbUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedGuestException = 
                new LockedGuestException(dbUpdateConcurrencyException);

            var expectedGuestDependencyValidationException = 
                new GuestDependencyValidationException(lockedGuestException);
            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(It.IsAny<Guid>()))
                .ThrowsAsync(dbUpdateConcurrencyException);

            //when
            ValueTask<Guest> removeGuestById =
                this.guestService.RemoveGuestByIdAsync(someGuestId);

            var actualGuestDependencyValidationException =
                await Assert.ThrowsAsync<GuestDependencyValidationException>(removeGuestById.AsTask);

            //then
            actualGuestDependencyValidationException.Should()
                .BeEquivalentTo(expectedGuestDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGuestAsync(It.IsAny<Guest>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            //given
            Guid someGuestId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedGuestStorageException =
                new FailedGuestStorageException(sqlException);

            var excpectedGuestDependencyException =
                new GuestDependencyException(failedGuestStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(It.IsAny<Guid>()))
                .ThrowsAsync(sqlException);

            //when
            ValueTask<Guest> removeGuestTask =
                this.guestService.RemoveGuestByIdAsync(someGuestId);

            GuestDependencyException actualGuestDependencyException =
                await Assert.ThrowsAsync<GuestDependencyException>(removeGuestTask.AsTask);

            //then
            actualGuestDependencyException.Should()
                .BeEquivalentTo(excpectedGuestDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    excpectedGuestDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

    }
}
