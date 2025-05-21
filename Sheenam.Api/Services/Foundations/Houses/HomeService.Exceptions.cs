//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Sheenam.Api.Models.Foundations.Houses;
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
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistHomeException = 
                    new AlreadyExistHomeException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistHomeException);
            }
            catch(SqlException sqlException)
            {
                var failedHomeStorageException =
                    new FailedHomeStorageException(sqlException);

                throw CreateAndLogDependencyException(failedHomeStorageException);
            }
            catch(Exception serviceException)
            {
                FailedHomeServiceException failedHomeServiceException = 
                    new FailedHomeServiceException(serviceException);

                throw CreateAndLogServiceException(failedHomeServiceException);
            }

        }

        private HomeValidationException CreateAndLogValidationException(Xeption exception)
        {
            HomeValidationException homeValidationException =
                new HomeValidationException(exception);

            this.loggingBroker.LogError(homeValidationException);
            return homeValidationException;
        }
        private HomeDependencException CreateAndLogDependencyException(Xeption exception)
        {
            HomeDependencException homeDependencException =
                new HomeDependencException(exception);

            this.loggingBroker.LogCritical(homeDependencException);
            return homeDependencException;
        }
        private HomeDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            HomeDependencyValidationException homeDependencyValidationException =
                new HomeDependencyValidationException(exception);

            this.loggingBroker.LogError(homeDependencyValidationException);
            return homeDependencyValidationException;
        }
        private HomeServiceException CreateAndLogServiceException(Xeption exception)
        {
            HomeServiceException homeServiceException =
                new HomeServiceException(exception);

            this.loggingBroker.LogError(homeServiceException);
            return homeServiceException;
        }

    }
}
