//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using Sheenam.Api.Models.Foundations.Home;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions.SmallExceptions;
using Sheenam.Api.Models.Foundations.Houses.Exceptions.SmallExceptions;

namespace Sheenam.Api.Services.Foundations.Houses
{
    public partial class HomeService
    {
        private static void ValidateHomeNotNull(Home home)
        {
            if (home is null)
            {
                throw new NullHomeException();
            }
        }
    }
}
