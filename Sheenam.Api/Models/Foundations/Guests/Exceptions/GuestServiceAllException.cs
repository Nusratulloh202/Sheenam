//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Guests.Exceptions
{
    public class GuestServiceAllException : Xeption
    {
        public GuestServiceAllException(Xeption exception)
            : base(message: "Guest Service error occurred, contact support",
                exception)
        {

        }
    }
}
