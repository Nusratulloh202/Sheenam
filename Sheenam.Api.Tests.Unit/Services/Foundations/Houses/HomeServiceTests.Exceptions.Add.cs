//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Models.Foundations.Home;
using Sheenam.Api.Models.Foundations.Houses.Exceptions.BigExceptions;
using Sheenam.Api.Models.Foundations.Houses.Exceptions.SmallExceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Houses
{
    public partial class HomeServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Home someHome = CreateRandomHome();
            SqlException sqlException = GetSqlException();

            var failedHomeStorageException =
                new FailedHomeStorageException(sqlException);

            var expectedHomeException = 
                new HomeDependencException(failedHomeStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertHomeAsync(someHome))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Home> addHomeTask = this.homeService.AddHomeAsync(someHome);

            HomeDependencException actualHomeDependencyException = 
                await Assert.ThrowsAsync<HomeDependencException>(()=>addHomeTask.AsTask());

            //then
            actualHomeDependencyException.Should().BeEquivalentTo(expectedHomeException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertHomeAsync(someHome),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedHomeException))),
                    Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }
            //Dublicat Key Exception
        [Fact]
        public async Task ShouldThrowDependencyValidationOnAddIfDublicateKeyErrorOccursAndLogItAsync()
        {
            //given
            Home someHome = CreateRandomHome();
            string message = GetRandomString();
            var duplicateKeyException = new DuplicateKeyException(message);

            var alreadyExistHomeException =
                new AlreadyExistHomeException(duplicateKeyException);

            var expectedHomeDependencyValidationException = 
                new HomeDependencyValidationException(alreadyExistHomeException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertHomeAsync(someHome))
                .ThrowsAsync(duplicateKeyException);

            //when
            ValueTask<Home> homeTask =
                this.homeService.AddHomeAsync(someHome);

            HomeDependencyValidationException actualHomeDependencyException =
                await Assert.ThrowsAsync<HomeDependencyValidationException>(() =>
                    homeTask.AsTask());

            //then
            actualHomeDependencyException.Should()
                .BeEquivalentTo(expectedHomeDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertHomeAsync(someHome),
                Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedHomeDependencyValidationException))),
                Times.Once());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

    }
}
