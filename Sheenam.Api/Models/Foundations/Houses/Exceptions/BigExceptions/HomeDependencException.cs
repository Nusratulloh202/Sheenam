//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Houses.Exceptions.BigExceptions
{
    public class HomeDependencException : Xeption
    {
        public HomeDependencException(Xeption innerException)

            : base(message: "Home dependecy error occured, contact support. ", innerException)
        { }
    }
}
