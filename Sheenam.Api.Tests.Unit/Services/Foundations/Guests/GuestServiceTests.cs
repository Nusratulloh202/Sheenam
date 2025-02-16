//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using FluentAssertions;
using Moq;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Enums;
using Sheenam.Api.Services.Foundations.Guests;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Guests
{
    public class GuestServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IGuestService guestService;

        public GuestServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.guestService = new GuestService
                (storageBroker: this.storageBrokerMock.Object);
        }

        [Fact]
        public async Task ShouldBeTrue()
        {
            //Arrange
            Guest randomGuest=new Guest
            {
                Id = Guid.NewGuid(),
                FirstName = "RandomFirstName",
                LastName = "RandomLastName",
                DateOffBirth = DateTimeOffset.UtcNow,
                Email = "RandomEmail",
                PhoneNumber = "RandomPhoneNumber",
                Address = "RandomAddress",
                Gender = GenderType.Male
            };
            this.storageBrokerMock.Setup(broker =>
                broker.InsertGuestAsync(randomGuest))
                .ReturnsAsync(randomGuest);
            //Act
            Guest actualGuest = await this.guestService.AddGuestAsync(randomGuest);
            //Assert
            actualGuest.Should().BeEquivalentTo(randomGuest);
        }
    }
}
