//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Guests.Exceptions
{
    public class AlreadyExistGuestException:Xeption
    {
        public AlreadyExistGuestException(Exception exception)
            : base(message:"Guest with the same id already exists.", exception)
        { }
    }
}
