﻿//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System.Linq.Expressions;
using Moq;
using Sheenam.Api.Brokers.Logings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Services.Foundations.Guests;
using Tynamix.ObjectFiller;
using Xeptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IGuestService guestService;

        public GuestServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.guestService = new GuestService
                (storageBroker: this.storageBrokerMock.Object,
                 loggingBroker: this.loggingBrokerMock.Object);
        }

        private Expression<Func<Xeption,bool>> SameExceptionAs(Xeption expectedException)
        {
            return actualException =>
            actualException.Message == expectedException.Message &&
            actualException.InnerException.Message == expectedException.InnerException.Message &&
            (actualException.InnerException as Xeption).DataEquals(expectedException.InnerException.Data);
        }
        private static Filler<Guest>CreateGuestFiller(DateTimeOffset date)
        {
            var filler = new Filler<Guest>();
            filler.Setup()
                .OnType<DateTimeOffset>().Use(date);
            return filler;
        }
        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Guest CreateRandomGuest()=>
            CreateGuestFiller(date:GetRandomDateTimeOffset()).Create();
    } 
}
