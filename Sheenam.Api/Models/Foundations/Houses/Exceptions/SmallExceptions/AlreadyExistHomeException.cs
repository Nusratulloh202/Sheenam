//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Houses.Exceptions.SmallExceptions
{
    public class AlreadyExistHomeException:Xeption
    {
        public AlreadyExistHomeException(Exception exception)
            :base(message: "Host with the same id already exists.", innerException: exception)
        {}
    }
}
