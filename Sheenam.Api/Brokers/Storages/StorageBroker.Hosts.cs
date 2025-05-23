﻿//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sheenam.Api.Models.Foundations.Hosts;
using Sheenam.Api.Models.Foundations.Houses;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Host> Hosts { get; set; }
        public DbSet<Home> Homes { get; set; }


        public async ValueTask<Host> InsertHostAsync(Host host)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<Host> hostEntityEntry =
                await broker.Hosts.AddAsync(host);

            await broker.SaveChangesAsync();
            return hostEntityEntry.Entity;

        }
        private IQueryable<T> SelectAllClassHost<T>() where T : class
        {
            var broker = new StorageBroker(this.configuration);
            return broker.Set<T>();
        }

        public IQueryable<Host> SelectAllHosts()
        {
            var hosts = SelectAllClassHost<Host>()
                .Include(host => host.Homes);  // Bu yerda Include qo'shildi

            return hosts;
        }
        public async ValueTask<Host> SelectByIdHostAsync(Guid id)
        {
            return await Hosts
                .Include(host => host.Homes)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async ValueTask<Host> UpdateHostAsync(Host host) =>
            await UpdateAsync(host);
        public async ValueTask<Host> DeleteHostAsync(Host host) =>
            await DeleteAsync(host);

    }
}
