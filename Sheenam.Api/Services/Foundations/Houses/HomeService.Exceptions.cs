//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System.Linq;
using System.Threading.Tasks;
using Sheenam.Api.Models.Foundations.Home;
using Sheenam.Api.Models.Foundations.Houses.Exceptions.BigExceptions;
using Sheenam.Api.Models.Foundations.Houses.Exceptions.SmallExceptions;
using Xeptions;

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
            catch (NullHomeException nullHomeException)
            {
                throw CreateAndLogValidationException(nullHomeException);
            }
            catch (InvalidHomeException invalidHomeException)
            {
                throw CreateAndLogValidationException(invalidHomeException);
            }
        }

        private HomeValidationException CreateAndLogValidationException(Xeption exception)
        {
            HomeValidationException homeValidationException =
                new HomeValidationException(exception);

            this.loggingBroker.LogError(homeValidationException);
            return homeValidationException;
        }
       
    }
}
