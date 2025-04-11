//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Hosts.Exceptions.BigExceptions
{
    public class HostServiceAllException : Xeption
    {
        public HostServiceAllException(Xeption innerException)
            : base(message: "Host Service error occurred, contact support")
        { }
    }
}
