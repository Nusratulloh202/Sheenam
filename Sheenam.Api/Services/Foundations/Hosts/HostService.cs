//==================================================
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
            TryCatch(() => this.storageBroker.SelectAllHosts());
        public ValueTask<Host> RetrieveByIdHostAsync(Guid hostId) =>
            TryCatch(async () =>
            {
                ValidateHostId(hostId);
                Host maybeHost = await this.storageBroker.SelectByIdHostAsync(hostId);
                ValidateStorageHost(maybeHost, hostId);

                return maybeHost;
            });
        public ValueTask<Host> ModifyHostAsync(Host host) =>
            TryCatch(async () =>
            {
                ValidateHostOnModify(host);
                Host maybeHost =
                    await this.storageBroker.SelectByIdHostAsync(host.Id);
                ValidateAgainstStorageHostOnModify(host, maybeHost);

                return await this.storageBroker.UpdateHostAsync(host);
            });
        public ValueTask<Host> RemoveHostAsync(Guid hostId) =>
            TryCatch(async () =>
            {
                ValidateHostId(hostId);
                Host maybeHost = await this.storageBroker.SelectByIdHostAsync(hostId);
                ValidateStorageHost(maybeHost, hostId);
                return await this.storageBroker.DeleteHostAsync(hostId);
            });
    }
}
