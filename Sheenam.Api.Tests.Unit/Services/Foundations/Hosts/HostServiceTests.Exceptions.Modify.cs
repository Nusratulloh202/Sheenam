//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Sheenam.Api.Models.Foundations.Hosts;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions.BigExceptions;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions.SmallExceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Hosts
{
    public partial class HostServiceTests
    {

        //Sql dan keladigan barcha errorlar uchun
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionsOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Host randomHost = CreateRandomHost();
            Host someHost = randomHost;
            Guid hostId = someHost.Id;
            SqlException sqlException = GetSqlException();

            var failedHostStorageException =
                new FailedHostStorageException(sqlException);

            var expectedHostDependencyException =
                new HostDependencyException(failedHostStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectByIdHostAsync(hostId))
                .ThrowsAsync(sqlException);
            //when
            ValueTask<Host> modifyHostTask =
                this.hostService.ModifyHostAsync(someHost);

            HostDependencyException actualHostDependencyException =
                await Assert.ThrowsAsync<HostDependencyException>
                (modifyHostTask.AsTask);

            //then
            actualHostDependencyException.Should()
                .BeEquivalentTo(expectedHostDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedHostDependencyException))),
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectByIdHostAsync(hostId),
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateHostAsync(It.IsAny<Host>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
        //database da update qilgunimizcha yuzaga keladigan Exceptionlar uchun
        [Fact]
        public async Task ShouldThrowDependencyValidationOnModifyIfDbUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            //given
            Host randomHost = CreateRandomHost();
            Host someHost = randomHost;
            Guid hostId = someHost.Id;
            var dbUpdateException =
                new DbUpdateException();

            FailedHostStorageException failedHostStorageException =
                new FailedHostStorageException(dbUpdateException);

            HostDependencyException expectedHostDependencyException =
                new HostDependencyException(failedHostStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectByIdHostAsync(hostId))
                .ThrowsAsync(dbUpdateException);

            //when
            ValueTask<Host> modifyHostTask =
                this.hostService.ModifyHostAsync(someHost);

            HostDependencyException actualHostDependencyException =
                await Assert.ThrowsAsync<HostDependencyException>
                (modifyHostTask.AsTask);
            //then
            actualHostDependencyException.Should()
                .BeEquivalentTo(expectedHostDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedHostDependencyException))),
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectByIdHostAsync(hostId),
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateHostAsync(It.IsAny<Host>()),
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
        //qora ro'yxatdagilarni yangilamaslik
        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            //given
            Host randomHost = CreateRandomHost();
            Host someHost = randomHost;
            Guid hostId = someHost.Id;
            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();
            var lockedHostException =
                new LockedHostException(databaseUpdateConcurrencyException);
            var expectedHostDependencyValidationException =
                new HostDependencyValidationException(lockedHostException);
            
            this.storageBrokerMock.Setup(broker =>
                broker.SelectByIdHostAsync(hostId))
                .ThrowsAsync(databaseUpdateConcurrencyException);

            //when
            ValueTask<Host> modifyHostAsync = this.hostService.ModifyHostAsync(someHost);

            HostDependencyValidationException actualHostDependencyValidationException =
                await Assert.ThrowsAsync<HostDependencyValidationException>
                (modifyHostAsync.AsTask);

            //then
            actualHostDependencyValidationException.Should()
                .BeEquivalentTo(expectedHostDependencyValidationException);

            this.storageBrokerMock.Verify(broker=>
                broker.SelectByIdHostAsync(hostId),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateHostAsync(It.IsAny<Host>()),
                Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }
    }
}
