//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Houses.Exceptions.SmallExceptions
{
    public class NullHomeException:Xeption
    {
        public NullHomeException()
            :base("Home is null. Try Again.")
        {}
    }
}
