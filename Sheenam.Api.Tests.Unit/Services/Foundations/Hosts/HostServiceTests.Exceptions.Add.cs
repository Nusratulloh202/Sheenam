//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using FluentAssertions;
using Microsoft.Data.SqlClient;
using EFxceptions.Models.Exceptions;
using Moq;
using Sheenam.Api.Models.Foundations.Hosts;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions.BigExceptions;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions.SmallExceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Hosts
{
    public  partial class HostServiceTests
    {
        private object DublicateKeyException;

        // SQL Exception uchun test
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Host someHost = CreateRandomHost();
            SqlException sqlException = GetSqlException();
            var failedHostStorageException = new FailedHostStorageException(sqlException);
            var expectedHostDependencyException =
                new HostDependencyException(failedHostStorageException);
            this.storageBrokerMock.Setup(broker =>
            broker.InsertHostAsync(someHost))
                .ThrowsAsync(sqlException);
            // when
            ValueTask<Host> AddHostTask = this.hostService.AddHostAsync(someHost);
            var actualHostDependencyException =
                await Assert.ThrowsAsync<HostDependencyException>(() =>
                AddHostTask.AsTask());

            // then
            actualHostDependencyException.Should()
                .BeEquivalentTo(expectedHostDependencyException);
            this.storageBrokerMock.Verify(broker =>
                broker.InsertHostAsync(someHost),
                Times.Once());
            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedHostDependencyException))),
                Times.Once());
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }

        // Dublicat key Exception uchun test

        //[Fact]
        //public async  Task ShouldThrowDependencyValidationOnAddIfDublicateKeyErrorOccursAndLogItAsync()
        //{
        //    //given
        //    Host someHost= CreateRandomHost();
        //    string message= GetRandomString();
        //    var dublicateKeyException = new DuplicateKeyException(message);

        //    var alreadyExistHostException = 
        //        new AlreadyExistHostException(dublicateKeyException);

        //    var expectedHostDependencyValidationException =
        //        new HostDependencyValidationException(alreadyExistHostException);
        //    this.storageBrokerMock.Setup(broker=>
        //        broker.InsertHostAsync(someHost))
        //        .ThrowsAsync(dublicateKeyException);
        //    //when
        //    ValueTask<Host> addHostTask =
        //        this.hostService.AddHostAsync(someHost);
        //    var actualHostDependencyyValidationException =
        //        await Assert.ThrowsAsync<HostDependencyValidationException>(() =>
        //        addHostTask.AsTask());
        //    //then
        //    actualHostDependencyyValidationException.Should().BeEquivalentTo(expectedHostDependencyValidationException);

        //    this.storageBrokerMock.Verify(broker =>
        //        broker.InsertHostAsync(someHost),
        //        Times.Once());

        //    this.loggingBrokerMock.Verify(broker =>
        //        broker.LogError(It.Is(SameExceptionAs(expectedHostDependencyValidationException))),
        //        Times.Once());

        //    this.storageBrokerMock.VerifyNoOtherCalls();
        //    this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
