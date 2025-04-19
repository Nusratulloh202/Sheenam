//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Models.Foundations.Hosts;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions.BigExceptions;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions.SmallExceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Hosts
{
    public partial class HostServiceTests
    {
        //Sql Error uchun test
        //given
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Guid someGuidHost = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedHostStorageException =
                new FailedHostStorageException(sqlException);

            var expectedHostDependencyException =
                new HostDependencyException(failedHostStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectByIdHostAsync(It.IsAny<Guid>()))
                .ThrowsAsync(sqlException);

            //when
            ValueTask<Host> RetrieveByIdTask =
                this.hostService.RetrieveByIdHostAsync(someGuidHost);

            HostDependencyException actualHostDependencyException =
                await Assert.ThrowsAsync<HostDependencyException>(RetrieveByIdTask.AsTask);

            //then
            actualHostDependencyException.Should()
                .BeEquivalentTo(expectedHostDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectByIdHostAsync(someGuidHost), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedHostDependencyException))),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }
        //ServiceException uchun test
        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdAsyncIfServiceErrorOccursAndLogItAsync()
        {
            //given
            Guid someIdHost = Guid.NewGuid();
            Exception serverException = new Exception();

            var failedHostServiceException =
                new FailedHostServiceException(serverException);

            var expectedHostServiceAllException =
                new HostServiceAllException(failedHostServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectByIdHostAsync(It.IsAny<Guid>()))
                .ThrowsAsync(serverException);

            //when
            ValueTask<Host> RetrieveByIdTask =
                this.hostService.RetrieveByIdHostAsync(someIdHost);

            HostServiceAllException actualHostServiceAllException =
                await Assert.ThrowsAsync<HostServiceAllException>(
                    RetrieveByIdTask.AsTask);

            //then
            actualHostServiceAllException.Should()
                .BeEquivalentTo(expectedHostServiceAllException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectByIdHostAsync(someIdHost),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHostServiceAllException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
