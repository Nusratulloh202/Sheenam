//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using FluentAssertions;
using Moq;
using Sheenam.Api.Models.Foundations.Home;
using Sheenam.Api.Models.Foundations.Home.Enums;
using Sheenam.Api.Models.Foundations.Houses.Exceptions.BigExceptions;
using Sheenam.Api.Models.Foundations.Houses.Exceptions.SmallExceptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Houses
{
    public partial class HomeServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfHomeIsNullAndLogItAsync()
        {
            // given
            Home nullHome = null;
            var nullHomeException = new  NullHomeException();

            var expectedHomeValidationException =
                new HomeValidationException(nullHomeException);


            // when
            ValueTask<Home> addHomeTask =
                this.homeService.AddHomeAsync(nullHome);
            // then
           HomeValidationException actual= await Assert.ThrowsAsync<HomeValidationException>(() =>
                addHomeTask.AsTask());
            actual.Should().BeEquivalentTo(expectedHomeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHomeValidationException))),
                        Times.Once);
            this.storageBrokerMock.Verify(broker =>
                broker.InsertHomeAsync(It.IsAny<Home>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData (null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfHomeIsInvalidAndLogItAsync(string invalidString)
        {
            //given
            var invalidHome = new Home
            {
                Address = invalidString
               
            };
            var invalidHomeException = new InvalidHomeException();

            invalidHomeException.AddData(
                   key: nameof(Home.Id),
                   values: "Id is required");

            invalidHomeException.AddData(
                key: nameof(Home.HostId),
                values: "Id is required");

            invalidHomeException.AddData(
                key: nameof(Home.Address),
                values: "Text is required");
            invalidHomeException.AddData(
                key: nameof(Home.AdditionalInfo),
                values: "Text is required");
            invalidHomeException.AddData(
                key: nameof(Home.IsShared),
                values: "Boolean is required");
            invalidHomeException.AddData(
                key: nameof(Home.IsVacant),
                values: "Boolean is required");
            invalidHomeException.AddData(
                key: nameof(Home.IsPetAllowed),
                values: "Boolean is required");
            invalidHomeException.AddData(
                key: nameof(Home.NumberOfBedrooms),
                values: "Number is required");

            invalidHomeException.AddData(
                key: nameof(Home.NumberOfBathrooms),
                values: "Number is required");

            invalidHomeException.AddData(
                key: nameof(Home.AreaInSquareMeters),
                values: "Number is required");

            invalidHomeException.AddData(
                key: nameof(Home.Price),
                values: "Number is required");

            invalidHomeException.AddData(
                key: nameof(Home.CreatedDate),
                values: "Date is required");

            invalidHomeException.AddData(
                key: nameof(Home.UpdatedDate),
                values: "Date is required");

            var expectedHomeValidationException =
                new HomeValidationException(invalidHomeException);

            // when

            ValueTask<Home> addHomeTask =
                this.homeService.AddHomeAsync(invalidHome);

            HomeValidationException actualHomeValidationException =
                await Assert.ThrowsAsync<HomeValidationException>(() =>
                    addHomeTask.AsTask());

            //then
            actualHomeValidationException.Should().BeEquivalentTo(
                expectedHomeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedHomeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertHomeAsync(It.IsAny<Home>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfHomeTypedAndLogItAsync()
        {
            //given
            Home randomHome = CreateRandomHome();
            Home invalidHome = randomHome;
            invalidHome.Type = GetInvalidEnum<HomeType>();
            var invalidHomeException = new InvalidHomeException();
            invalidHomeException.AddData(
                key: nameof(Home.Type),
                values: "Value is invalid");
            var expectedHomeValidationException = new HomeValidationException(invalidHomeException);

            //when
            ValueTask<Home> addHomeTask =
                         this.homeService.AddHomeAsync(invalidHome);

            //then
            HomeValidationException actualHomeValidationException =
                await Assert.ThrowsAsync<HomeValidationException>(() =>
                    addHomeTask.AsTask());
           actualHomeValidationException.Should().BeEquivalentTo(
                expectedHomeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedHomeValidationException))),
                Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertHomeAsync(It.IsAny<Home>()),
                Times.Never());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
