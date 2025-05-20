//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Houses.Exceptions.SmallExceptions
{
    public class NotFoundHomeException : Xeption
    {
        public NotFoundHomeException(Guid hostId)
            : base(message: $"Couldn't find host with id {hostId}.")
        { }
    }
}
