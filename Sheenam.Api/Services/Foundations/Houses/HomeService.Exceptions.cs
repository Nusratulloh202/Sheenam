//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;
using System.Linq;
using System.Threading.Tasks;
using Sheenam.Api.Models.Foundations.Home;

namespace Sheenam.Api.Services.Foundations.Houses
{
    public partial class HomeService
    {
        private delegate ValueTask<Home> ReturningHomeFunction();
        private delegate IQueryable<Home> ReturningHousesFunction();
        private async ValueTask<Home> TryCatch(ReturningHomeFunction returningHomeFunction)
        {
            try
            {
                return await returningHomeFunction();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
