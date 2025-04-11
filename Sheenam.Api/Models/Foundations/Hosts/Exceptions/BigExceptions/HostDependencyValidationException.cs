//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Hosts.Exceptions.BigExceptions
{
    public class HostDependencyValidationException : Xeption
    {
        public HostDependencyValidationException(Xeption innerException)
            : base(message: "Host dependency validation error occured.", innerException)
        { }
    }
}
