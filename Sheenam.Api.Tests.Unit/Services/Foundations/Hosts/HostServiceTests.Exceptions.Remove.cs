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
        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            //given
            Guid someHostId = Guid.NewGuid();

            var dbUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedHostException =
                new LockedHostException(dbUpdateConcurrencyException);

            var expectedHostDependencyValidationException =
                new HostDependencyValidationException(lockedHostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectByIdHostAsync(It.IsAny<Guid>()))
                .ThrowsAsync(dbUpdateConcurrencyException);
            //when
            ValueTask<Host> removeHostById =
                this.hostService.RemoveHostAsync(someHostId);

            var actualHostDependencyValidationException =
                await Assert.ThrowsAsync<HostDependencyValidationException>(
                    removeHostById.AsTask);
            //then
            actualHostDependencyValidationException.Should()
                .BeEquivalentTo(expectedHostDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectByIdHostAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteHostAsync(It.IsAny<Host>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            //given
            Guid someHostId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedHostStorageException =
                new FailedHostStorageException(sqlException);

            var expectedHostDependencyException =
                new HostDependencyException(failedHostStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectByIdHostAsync(It.IsAny<Guid>()))
                .ThrowsAsync(sqlException);
            //when
            ValueTask<Host> removeHostById =
                this.hostService.RemoveHostAsync(someHostId);

            var actualHostDependencyException =
                await Assert.ThrowsAsync<HostDependencyException>(
                    removeHostById.AsTask);
            //then
            actualHostDependencyException.Should()
                .BeEquivalentTo(expectedHostDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectByIdHostAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedHostDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteHostAsync(It.IsAny<Host>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceAllExceptionsOnRemoveIfServiceErrorOccursAndLogItAsync()
        {
            //given
            Guid someHostId = Guid.NewGuid();

            var serviceException = new Exception();

            var failedHostServiceException =
                new FailedHostServiceException(serviceException);
            var expectedHostServiceException =
                new HostServiceAllException(failedHostServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectByIdHostAsync(It.IsAny<Guid>()))
                .ThrowsAsync(serviceException);
            //when
            ValueTask<Host> removeHostById =
                this.hostService.RemoveHostAsync(someHostId);

            var actualHostServiceException =
                await Assert.ThrowsAsync<HostServiceAllException>(
                    removeHostById.AsTask);
            //then
            actualHostServiceException.Should()
                .BeEquivalentTo(expectedHostServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostServiceException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectByIdHostAsync(It.IsAny<Guid>()), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteHostAsync(It.IsAny<Host>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
