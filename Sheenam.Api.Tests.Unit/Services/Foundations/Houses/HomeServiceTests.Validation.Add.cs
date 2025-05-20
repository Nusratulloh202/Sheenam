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
            var nullHomeException = new NullHomeException();

            var expectedHomeValidationException =
                new HomeValidationException(nullHomeException);


            // when
            ValueTask<Home> addHomeTask =
                this.homeService.AddHomeAsync(nullHome);
            // then
            HomeValidationException actual = await Assert.ThrowsAsync<HomeValidationException>(() =>
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
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfHomeIsInvalidAndLogItAsync(string invalidString)
        {
            // given
            var invalidHome = new Home
            {
                Id = Guid.Empty,
                HostId = Guid.Empty,
                Address = invalidString,
                AdditionalInfo = invalidString,
                IsShared = default,
                IsVacant = default,
                IsPetAllowed = default,
                NumberOfBedrooms = default,
                NumberOfBathrooms = default,
                AreaInSquareMeters = default,
                Price = default,
                CreatedDate = default,
                UpdatedDate = default
            };

            var invalidHomeException = new InvalidHomeException();

            invalidHomeException.AddData(nameof(Home.Id), "Id is required");
            invalidHomeException.AddData(nameof(Home.HostId), "Id is required");
            invalidHomeException.AddData(nameof(Home.Address), "Text is required");
            invalidHomeException.AddData(nameof(Home.AdditionalInfo), "Text is required");
            invalidHomeException.AddData(nameof(Home.IsShared), "Boolean is required");
            invalidHomeException.AddData(nameof(Home.IsVacant), "Boolean is required");
            invalidHomeException.AddData(nameof(Home.IsPetAllowed), "Boolean is required");
            invalidHomeException.AddData(nameof(Home.NumberOfBedrooms), "Number is required");
            invalidHomeException.AddData(nameof(Home.NumberOfBathrooms), "Number is required");
            invalidHomeException.AddData(nameof(Home.AreaInSquareMeters), "Number is required");
            invalidHomeException.AddData(nameof(Home.Price), "Number is required");
            invalidHomeException.AddData(nameof(Home.CreatedDate), "Date is required");
            invalidHomeException.AddData(nameof(Home.UpdatedDate), "Date is required");

            var expectedHomeValidationException =
                new HomeValidationException(invalidHomeException);

            // when
            ValueTask<Home> addHomeTask = this.homeService.AddHomeAsync(invalidHome);

            HomeValidationException actualHomeValidationException =
                await Assert.ThrowsAsync<HomeValidationException>(() => addHomeTask.AsTask());

            // then
            actualHomeValidationException.Should().BeEquivalentTo(expectedHomeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedHomeValidationException))),
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
