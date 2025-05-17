//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using System.Runtime.Serialization;
using Moq;
using Sheenam.Api.Brokers.Logings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Home;
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

        private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException)=>
                        actualException => actualException.SameExceptionAs(expectedException);
        private static Filler<Home> CreateHomeFiller(DateTimeOffset date)
        {
            var filler = new Filler<Home>();
            filler.Setup().OnType<DateTimeOffset>().Use(date);
            filler.Setup().OnType<string>().Use(string.Empty);
            filler.Setup().OnType<decimal>().Use(0.0m);
            return filler;
        }
        private static DateTimeOffset GetRandomDateTimeOffSet() =>
            new DateTimeRange(earliestDate: new DateTime(2000, 1, 1)).GetValue();
        private static Home CreateRandomHome() =>
            CreateHomeFiller(date: GetRandomDateTimeOffSet()).Create();

        private IQueryable<Home> CreateRandomHouses()
        {
            return  CreateHomeFiller(date: GetRandomDateTimeOffSet()).Create(count:GetRandomNumber()).AsQueryable();
        }
        private static int GetRandomNumber()=>
            new IntRange(min:1, max:10).GetValue();

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
