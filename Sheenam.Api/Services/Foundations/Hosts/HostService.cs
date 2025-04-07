//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using Sheenam.Api.Brokers.Logings;
using Sheenam.Api.Brokers.Storages;

namespace Sheenam.Api.Services.Foundations.Hosts
{
    public partial class HostService:IHostService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public HostService(IStorageBroker storageBroker, 
                            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

    }
}
