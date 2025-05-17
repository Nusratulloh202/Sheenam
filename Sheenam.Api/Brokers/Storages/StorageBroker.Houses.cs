//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using Microsoft.EntityFrameworkCore;
using Sheenam.Api.Models.Foundations.Home;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
            public DbSet<Home> Home { get; set; }
    }
}
