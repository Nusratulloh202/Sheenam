//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using Xeptions;
namespace Sheenam.Api.Models.Foundations.Hosts.Exceptions.SmallExceptions
{
    public class NullHostException : Xeption
    {
        public NullHostException()
            : base(message: "Null host. Please try again.")
        { }
    }
}
