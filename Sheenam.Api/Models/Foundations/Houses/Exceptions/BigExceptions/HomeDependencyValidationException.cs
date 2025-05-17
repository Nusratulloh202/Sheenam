//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Houses.Exceptions.BigExceptions
{
    public class HomeDependencyValidationException:Xeption
    {
        public HomeDependencyValidationException(Xeption innerException)
            : base(message:"Home dependency validation error occurrs. ", innerException)
        {}
    }
}
