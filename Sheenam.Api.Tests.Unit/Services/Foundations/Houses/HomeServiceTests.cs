//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Brokers.Logings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Houses;
using Sheenam.Api.Models.Foundations.Houses.Enums;
using Sheenam.Api.Services.Foundations.Houses;
using Tynamix.ObjectFiller;
using Xeptions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Houses
{
    public partial class HomeServiceTests
    {
        private readonly IHomeService homeService;
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;

        public HomeServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.homeService = new HomeService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
                        actualException => actualException.SameExceptionAs(expectedException);
        private static Filler<Home> CreateHomeFiller(DateTimeOffset date)
        {
            var filler = new Filler<Home>();
            filler.Setup().OnType<DateTimeOffset>().Use(date);
            filler.Setup().OnProperty(home => home.Address).Use(GetRandomString());
            filler.Setup().OnProperty(home => home.AdditionalInfo).Use(GetRandomString());
            filler.Setup().OnProperty(home => home.Price).Use(() => new decimal(new Random().Next(1, 10000)));
            filler.Setup().OnProperty(home => home.NumberOfBedrooms).Use(() => new Random().Next(1, 10));
            filler.Setup().OnProperty(home => home.NumberOfBathrooms).Use(() => new Random().Next(1, 10));
            filler.Setup().OnProperty(home => home.AreaInSquareMeters).Use(() => new Random().NextDouble() * 100 + 20); // 20 dan katta
            filler.Setup().OnProperty(home => home.IsVacant).Use(true);
            filler.Setup().OnProperty(home => home.IsPetAllowed).Use(true);
            filler.Setup().OnProperty(home => home.IsShared).Use(true);
            filler.Setup().OnProperty(home => home.Type).Use(HomeType.Duplex);
            return filler;
        }
        private static DateTimeOffset GetRandomDateTimeOffSet() =>
            new DateTimeRange(earliestDate: new DateTime(2000, 1, 1)).GetValue();
        private static Home CreateRandomHome() =>
            CreateHomeFiller(date: GetRandomDateTimeOffSet()).Create();

        private IQueryable<Home> CreateRandomHouses()
        {
            return CreateHomeFiller(date: GetRandomDateTimeOffSet()).Create(count: GetRandomNumber()).AsQueryable();
        }
        private static int GetRandomNumber() =>
            new IntRange(min: 1, max: 10).GetValue();

        private static SqlException GetSqlException() =>
           (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static T GetInvalidEnum<T>()
        {
            int randomNumber = GetRandomNumber();
            while (Enum.IsDefined(typeof(T), randomNumber) is true)
            {
                randomNumber = GetRandomNumber();
            }
            return (T)(object)randomNumber;
        }
    }
}
