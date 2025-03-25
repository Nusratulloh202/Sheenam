//==================================================
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
        //Sql dan keladigan barcha errorlar uchun 
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
        //database da update qilgunimizcha yuzaga keladigan Exceptionlar uchun
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            //given
            Guest randomGuest = CreateRandomGuest();
            Guest someGuest = randomGuest;
            Guid guestId = someGuest.Id;
            var databaseUpdateException = 
                new DbUpdateException();

            var failedGuestStorageException = 
                new FailedGuestStorageException(databaseUpdateException);

            var expectedGuestDependencyException =
                new GuestDependencyException(failedGuestStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(someGuest.Id)).ThrowsAsync(databaseUpdateException);

            //when
            ValueTask<Guest> modifyGuestTask =
                this.guestService.ModifyGuestAsync(someGuest);

            //then
            await Assert.ThrowsAsync<GuestDependencyException>
                (modifyGuestTask.AsTask);

            this.loggingBrokerMock.Verify(broker=>
                broker.LogError(It.Is(SameExceptionAs
                (expectedGuestDependencyException))),
                Times.Once());

            this.storageBrokerMock.Verify(broker=>
                broker.SelectGuestByIdAsync (someGuest.Id),
                Times.Once());

            this.storageBrokerMock.Verify(broker=>
                broker.UpdateGuestAsync(someGuest),
                Times.Never());  
            
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

       //qora ruyxatdagi guestni update qilmaslik uchun test
        [Fact]

        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            Guest randomGuest = CreateRandomGuest();
            Guest someGuest = randomGuest;
            Guid guestId = someGuest.Id;

            DbUpdateConcurrencyException dbUpdateConcurrencyException = 
                new DbUpdateConcurrencyException ();
           
            var lockedGuestException = 
                new LockedGuestException(dbUpdateConcurrencyException);

            var expectedGuestDependencyValidationException =
                new GuestDependencyValidationException(lockedGuestException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(guestId)).Throws(dbUpdateConcurrencyException);

            //when
            ValueTask<Guest> modifyGuestTask=
                this.guestService.ModifyGuestAsync(someGuest);

            GuestDependencyValidationException actualGuestDependencyValidationException = await
                Assert.ThrowsAsync<GuestDependencyValidationException>(modifyGuestTask.AsTask);

            //then
            actualGuestDependencyValidationException.Should()
                .BeEquivalentTo(expectedGuestDependencyValidationException);
            
            this.storageBrokerMock.Verify(broker=>
                broker.SelectGuestByIdAsync(guestId), Times.Once());

            this.loggingBrokerMock.Verify(broker=>
                broker.LogError(It.Is(SameExceptionAs
                (expectedGuestDependencyValidationException))), 
                Times.Once());
            

            this.storageBrokerMock.Verify(broker=>
                broker.UpdateGuestAsync(someGuest), 
                Times.Never());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
        //Servicedan keladigan boshqa barcha errorlar uchun test
        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            //given
            Guest randomGuest = CreateRandomGuest();
            Guest someGuest = randomGuest;
            Guid guestId = someGuest.Id;
            var serviceException = new Exception();

            var failedGuestServiceException = 
                new FailedGuestServiceException(serviceException);

            var expectedGuestServiceAllException=
                new GuestServiceAllException(failedGuestServiceException);

            this.storageBrokerMock.Setup(broker=>
                broker.SelectGuestByIdAsync(guestId))
                .Throws(serviceException);
            
            //when
            ValueTask<Guest> modifyGuestTask=
                this.guestService.ModifyGuestAsync(someGuest);

            GuestServiceAllException actualGuestServiceAllException = 
                await Assert.ThrowsAsync<GuestServiceAllException>(modifyGuestTask.AsTask);

            //then
            actualGuestServiceAllException.Should()
                .BeEquivalentTo(expectedGuestServiceAllException);

            this.loggingBrokerMock.Verify(broker=>
                broker.LogError(It.Is(SameExceptionAs(expectedGuestServiceAllException))),
                Times.Once);

            this.storageBrokerMock.Verify(broker=>
                broker.SelectGuestByIdAsync(guestId),
                Times.Once);
            
            this.storageBrokerMock.Verify(broker=>
                broker.UpdateGuestAsync(someGuest), 
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
