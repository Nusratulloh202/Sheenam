//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Hosts;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Host> Hosts { get; set; }
    


    }
}
