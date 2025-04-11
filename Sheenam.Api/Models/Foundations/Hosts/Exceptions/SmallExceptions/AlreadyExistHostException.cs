//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Hosts.Exceptions.SmallExceptions
{
    public class AlreadyExistHostException : Xeption
    {
        public AlreadyExistHostException(Exception exception)
            : base(message: "Host with the same id already exists.", exception)
        { }
    }
}
