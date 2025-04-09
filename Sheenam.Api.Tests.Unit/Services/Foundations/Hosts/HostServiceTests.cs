//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System.Linq.Expressions;
using Moq;
using Sheenam.Api.Brokers.Logings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Hosts;
using Sheenam.Api.Services.Foundations.Hosts;
using Tynamix.ObjectFiller;
using Xeptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Hosts
{
    public partial class HostServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly HostService hostService;

        public HostServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.hostService = new HostService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }
        private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);
        private static Filler<Host> CreateHostFiller(DateTimeOffset date)
        {
            var filler = new Filler<Host>();
            filler.Setup()
                .OnType<DateTimeOffset>().Use(date);
            return filler;
        }
        private static DateTimeOffset GetRandomDateTimeOffSet() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Host CreateRandomHost() =>
            CreateHostFiller(date: GetRandomDateTimeOffSet()).Create();     
    }
}
