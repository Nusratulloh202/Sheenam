﻿//==================================================
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
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdSqlErrorOccurseAndLogItAsync()
        {
            //given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedGuestStorageException =
                new FailedGuestStorageException(sqlException);

            var expectedGuestDependencyException =
                new GuestDependencyException(failedGuestStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            //when
            ValueTask<Guest> retrieveGuestById =
                this.guestService.RetrieveGuestByIdAsync(someId);

            GuestDependencyException actualGuestDependencyException =
                await Assert.ThrowsAsync<GuestDependencyException>
                (retrieveGuestById.AsTask);

            //then
            actualGuestDependencyException.Should().BeEquivalentTo(expectedGuestDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(someId), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGuestDependencyException))),
                    Times.Once);
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowsServiceDependencyExceptionOnRetrieveByIdAsyncIfServiceErrorOccursAndLogItAsync()
        {
            //given
            Guid someId = Guid.NewGuid();
            var serverException = new Exception();

            var failedGuestServiceException =
                new FailedGuestServiceException(serverException);

            var expectedGuestServiceAllException =
                new GuestServiceAllException(failedGuestServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(It.IsAny<Guid>()))
                .ThrowsAsync(serverException);

            //when
            ValueTask<Guest> retrieveGuestById =
                this.guestService.RetrieveGuestByIdAsync(someId);

            GuestServiceAllException actualGuestServiceAllException =
                await Assert.ThrowsAsync<GuestServiceAllException>(
                    retrieveGuestById.AsTask);

            //then
            actualGuestServiceAllException.Should().BeEquivalentTo(expectedGuestServiceAllException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(someId), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestServiceAllException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
