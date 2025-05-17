//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using Sheenam.Api.Brokers.Logings;
using Sheenam.Api.Brokers.Storages;

namespace Sheenam.Api.Services.Foundations.Houses
{
    public class HomeService:IHomeService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
    }
}
