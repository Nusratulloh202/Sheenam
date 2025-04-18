﻿//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;
using System.Linq;
using System.Threading.Tasks;
using Sheenam.Api.Brokers.Logings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Services.Foundations.Guests
{
    public partial class GuestService : IGuestService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public GuestService(IStorageBroker storageBroker,
                            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Guest> AddGuestAsync(Guest guest) =>
            TryCatch(async () =>
        {
            ValidateGuestOnAdd(guest);
            return await this.storageBroker.InsertGuestAsync(guest);
        });
        //Read All (Retrieve All)
        public IQueryable<Guest> RetrieveAllGuests() =>
            TryCatch(() => this.storageBroker.SelectAllGuests());
        //Read Id (Retrieve Id)
        public ValueTask<Guest> RetrieveGuestByIdAsync(Guid guestId) =>
            TryCatch(async () =>
            {
                ValidateGuestId(guestId);
                Guest maybeGuest = await this.storageBroker.SelectGuestByIdAsync(guestId);
                ValidateStorageGuest(maybeGuest, guestId);
                return maybeGuest;
            });
        //Update (Modify)
        public ValueTask<Guest> ModifyGuestAsync(Guest guest) =>
            TryCatch(async () =>
            {
                ValidateGuestOnModify(guest);

                Guest maybeGuest = await this.storageBroker.SelectGuestByIdAsync(guest.Id);

                ValidateAgainstStorageGuestOnModify(guest, maybeGuest);

                return await this.storageBroker.UpdateGuestAsync(guest);
            });
        //Delete (Remove)
        public ValueTask<Guest> RemoveGuestByIdAsync(Guid guestId) =>
            TryCatch(async () =>
            {
                ValidateGuestId(guestId);
                Guest maybeGuest =
                    await this.storageBroker.SelectGuestByIdAsync(guestId);
                ValidateStorageGuest(maybeGuest, guestId);


                return await this.storageBroker.DeleteGuestAsync(maybeGuest);
            });
    }
}
