//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using FluentAssertions;
using Moq;
using Sheenam.Api.Models.Foundations.Home;
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
            var nullHomeException = new NullHomeException();

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
                Id = Guid.Empty,
                HostId = Guid.Empty,
                Address = null,
                AdditionalInfo = null,
                IsVacant = false,
                NumberOfBedrooms = 0,
                NumberOfBathrooms = 0,
                AreaInSquareMeters = 0,
                IsPetAllowed = false,
                IsShared = false,
                Type = default,
                Price = 0,
                CreatedDate = default,
                UpdatedDate = default
            };
            var invalidHomeException = new InvalidHomeException();

            invalidHomeException.AddData(
                   key: nameof(Home.Id),
                   values: "Id is required");

            invalidHomeException.AddData(
                key: nameof(Home.HostId),
                values: "HostId is required");

            invalidHomeException.AddData(
                key: nameof(Home.Address),
                values: "Text is required");

            invalidHomeException.AddData(
                key: nameof(Home.NumberOfBedrooms),
                values: "At least 1 bedroom is required");

            invalidHomeException.AddData(
                key: nameof(Home.NumberOfBathrooms),
                values: "At least 1 bathroom is required");

            invalidHomeException.AddData(
                key: nameof(Home.AreaInSquareMeters),
                values: "Area must be greater than 0");

            invalidHomeException.AddData(
                key: nameof(Home.Price),
                values: "Price must be greater than 0");

            invalidHomeException.AddData(
                key: nameof(Home.CreatedDate),
                values: "CreatedDate is required");

            invalidHomeException.AddData(
                key: nameof(Home.UpdatedDate),
                values: "UpdatedDate is required");

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
    }
}
