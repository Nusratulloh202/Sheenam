//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Guest> Guests { get; set; }

        public async ValueTask<Guest> InsertGuestAsync(Guest guest)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<Guest> guestEntityEntry =
                await broker.Guests.AddAsync(guest);

            await broker.SaveChangesAsync();

            return guestEntityEntry.Entity;
        }

       
        public IQueryable<Guest> SelectAllGuests()
        {
            var guests = SelectAll<Guest>();
            return guests;
        }
        public ValueTask<Guest> SelectGuestByIdAsync(Guid guestId)
        {
            var guestIdInfo = Guests
                 .FirstOrDefault(x => x.Id == guestId);
            return ValueTask.FromResult(guestIdInfo);
        }
        public async ValueTask<Guest> UpdateGuestAsync(Guest guest)
        {
            var broker = new StorageBroker(this.configuration);
            broker.Entry(guest).State = EntityState.Modified;
            await broker.SaveChangesAsync();

            return guest;
        }
        public async ValueTask<Guest> DeleteGuestAsync(Guest guest)
        {

            var broker = new StorageBroker(configuration);
            broker.Entry(guest).State = EntityState.Deleted;
            await broker.SaveChangesAsync();

            return guest;
        }
    }
}
