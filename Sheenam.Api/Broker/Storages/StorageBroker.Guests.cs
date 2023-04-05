//================================================
//Copyright(c) Coalition of Good-Hearted Engineers
//Free To Use To Find Comfort and Peace
//================================================
using Microsoft.EntityFrameworkCore;
using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Broker.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Guest> Guests { get; set; }
    }
}
