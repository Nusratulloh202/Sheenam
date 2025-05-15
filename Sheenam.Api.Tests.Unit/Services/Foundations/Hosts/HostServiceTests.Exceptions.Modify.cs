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
    }
}
