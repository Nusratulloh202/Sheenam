﻿//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;
using System.Linq;
using System.Threading.Tasks;
using Sheenam.Api.Brokers.Logings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Hosts;

namespace Sheenam.Api.Services.Foundations.Hosts
{
    public partial class HostService : IHostService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public HostService(IStorageBroker storageBroker,
                            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }
        public async ValueTask<Host> AddHostAsync(Host host) =>
            await TryCatch(async () =>
            {
                ValidateHost(host);
                return await this.storageBroker.InsertHostAsync(host);
            });
        public IQueryable<Host> RetriveAllHosts() =>
            TryCatch(() =>  this.storageBroker.SelectAllHosts());
        public ValueTask<Host> RetrieveByIdHostAsync(Guid hostId) =>
            TryCatch(async () =>
            {
                Host maybeHost = await this.storageBroker.SelectByIdHostAsync(hostId);
                return maybeHost;
            });
    }
}
