//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Hosts.Exceptions.SmallExceptions
{
    public class NotFoundHostException:Xeption
    {
        public NotFoundHostException(Guid hostId)
            :base(message: $"Couldn't find host with id {hostId}.")
        {}
    }
}
