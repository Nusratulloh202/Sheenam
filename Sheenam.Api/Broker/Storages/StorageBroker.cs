//================================================
//Copyright(c) Coalition of Good-Hearted Engineers
//Free To Use To Find Comfort and Peace
//================================================

using System.Threading.Tasks;
using EFxceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheenam.Api.Broker.Storages;
using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Broker.Storages
{
    public partial class StorageBroker :EFxceptionsContext,IStorageBroker
    {
        private readonly IConfiguration configuration;

        public StorageBroker(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.Database.Migrate();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = this.configuration
                .GetConnectionString(name:"DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
        public override void Dispose(){}

        ValueTask<Guest> IStorageBroker.InsertGuestAsync<T>(Guest guest)
        {
            throw new System.NotImplementedException();
        }
    }
}






