//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Hosts.Exceptions.BigExceptions
{
    public class HostValidationException:Xeption
    {
        public HostValidationException(Exception innerException)
            :base(message: "Host validation error occurred, fix the errors and try again", innerException)
        {}
    }
}
