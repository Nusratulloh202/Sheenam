//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker : EFxceptionsContext, IStorageBroker
    {
        public readonly IConfiguration configuration;
        public StorageBroker(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = this.configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
        public async ValueTask<T> InsertAsync<T>(T @object) where T : class
        {
            var broker = new StorageBroker(configuration);
            await broker.Set<T>().AddAsync(@object);
            await broker.SaveChangesAsync();

            return @object;
        }
        public IQueryable<T> SelectAll<T>() where T : class
        {
            var broker = new StorageBroker(configuration);
            return broker.Set<T>();
        }
        public async ValueTask<T> SelectAsync<T>(Guid id) where T : class
        {
            var broker = new StorageBroker(configuration);
            return await broker.Set<T>().FindAsync(id);
        }
        public async ValueTask<T> UpdateAsync<T>(T @object)
        {
            var broker = new StorageBroker(configuration);
            broker.Entry(@object).State = EntityState.Modified;
            await broker.SaveChangesAsync();

            return @object;
        }
        public async ValueTask<T> DeleteAsync<T>(T @object)
        {
            var broker = new StorageBroker(configuration);
            broker.Entry(@object).State = EntityState.Deleted;
            await broker.SaveChangesAsync();
            return @object;
        }


        public override void Dispose() { }
    }
}
