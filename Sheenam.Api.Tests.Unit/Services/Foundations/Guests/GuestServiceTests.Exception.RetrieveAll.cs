//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Guests;

public partial class GuestServiceTests
{
    [Fact]
    public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
    {
        //given
        SqlException sqlException = GetSqlException();
        var failedGuestStorageException =
            new FailedGuestStorageException(sqlException);

        var expectedGuestDependencyException =
                  new GuestDependencyException(failedGuestStorageException);
        this.storageBrokerMock.Setup(broker =>
        broker.SelectAllGuests()).Throws(sqlException);

        //when
        Action retrieveAllGuestsAction = () =>
            this.guestService.RetrieveAllGuests();
        GuestDependencyException actualGuestDependensyException =
            Assert.Throws<GuestDependencyException>(retrieveAllGuestsAction);

        //then
        actualGuestDependensyException.Should()
            .BeEquivalentTo(expectedGuestDependencyException);

        this.storageBrokerMock.Verify(broker =>
            broker.SelectAllGuests(),
            Times.Once());

        this.loggingBrokerMock.Verify(broker =>
            broker.LogCritical(It.Is(SameExceptionAs(expectedGuestDependencyException))),
            Times.Once);

        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();


    }

    [Fact]
    public void ShouldThrowServiceExceptionOnRetriveAllIfServiceErrorOccursAndLogIt()
    {
        //given 
        string exceptionMessage = GetRandomMessage();
        var serverException = new Exception(message: exceptionMessage);

        var failedGuestServiceException =
            new FailedGuestServiceException(serverException);

        var expectedGuestSeviceAllException =
            new GuestServiceAllException(failedGuestServiceException);

        this.storageBrokerMock.Setup(broker =>
        broker.SelectAllGuests()).Throws(serverException);

        //when
        Action retriveAllGuestAction = () =>
            this.guestService.RetrieveAllGuests();

        GuestServiceAllException actualServiceException =
            Assert.Throws<GuestServiceAllException>(retriveAllGuestAction);

        //then
        actualServiceException.Should().BeEquivalentTo(expectedGuestSeviceAllException);

        this.storageBrokerMock.Verify(broker =>
        broker.SelectAllGuests(), Times.Once());

        this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedGuestSeviceAllException))),
            Times.Once);

        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}
