//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions.BigExceptions;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions.SmallExceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Hosts
{
    public partial class HostServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetriveAllWhenSqlExceptionOccursAndLogIt()
        {
            //given
            SqlException sqlExceptions = GetSqlException();
            var failedHostStorageException = 
                new FailedHostStorageException(sqlExceptions);

            HostDependencyException expectedHostDependecyException = 
                new HostDependencyException(failedHostStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllHosts()).Throws(sqlExceptions);
            //when
            Action retriveAllHostsAction = () =>
                this.hostService.RetriveAllHosts();
            HostDependencyException actualHostDependencyException = Assert.Throws<HostDependencyException>(retriveAllHostsAction);

            //then
            actualHostDependencyException.Should().BeEquivalentTo(expectedHostDependecyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllHosts(), 
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedHostDependecyException))), 
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }
    }
}
