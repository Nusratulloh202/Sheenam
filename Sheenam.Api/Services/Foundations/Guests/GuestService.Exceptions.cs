﻿//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;
using Xeptions;

namespace Sheenam.Api.Services.Foundations.Guests
{
    public partial class GuestService

    {
        private delegate ValueTask<Guest> ReturningGuestFunction();
        private delegate IQueryable<Guest> ReturningGuestsFunction();

        private async ValueTask<Guest> TryCatch(ReturningGuestFunction returningGuestFunction)
        {
            try
            {
                return await returningGuestFunction();
            }
            catch (NullGuestExceptions nullGuestException)
            {
                throw CreateAndLogValidationException(nullGuestException);
            }
            catch (InvalidGuestException invalidGuestException)
            {
                throw CreateAndLogValidationException(invalidGuestException);
            }
            catch (SqlException sqlException)
            {
                var failedGuestStorageException =
                    new FailedGuestStorageException(sqlException);
                throw CreateAndLogDependencyException(failedGuestStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistGuestException = new AlreadyExistGuestException(duplicateKeyException);
                throw CreateAndLogDependencyValidationException(alreadyExistGuestException);
            }
            catch (Exception exception)
            {
                var failedGuestException = new FailedGuestServiceException(exception);
                throw CreateAndLogServiceAllException(failedGuestException);
            }
        }

        private IQueryable<Guest> TryCatch(ReturningGuestsFunction returningGuestsFunction)
        {
            try
            {
                return returningGuestsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedGuestStorageException =
                    new FailedGuestStorageException(sqlException);
                throw CreateAndLogDependencyException(failedGuestStorageException);
            }
            catch (Exception exception)
            {
                var failedGuestException = new FailedGuestServiceException(exception);
                throw CreateAndLogServiceAllException(failedGuestException);
            }
        }



        private GuestValidationException CreateAndLogValidationException(Xeption exception)
        {
            var guestValidationException = new GuestValidationException(exception);
            this.loggingBroker.LogError(guestValidationException);
            return guestValidationException;
        }
        private GuestDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var guestDependencyException = new GuestDependencyException(exception);
            this.loggingBroker.LogCritical(guestDependencyException);

            return guestDependencyException;
        }

        private GuestDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var guestDependencyValidationException =
                new GuestDependencyValidationException(exception);

            this.loggingBroker.LogError(guestDependencyValidationException);

            return guestDependencyValidationException;
        }
        private GuestServiceAllException CreateAndLogServiceAllException(Xeption exception)
        {
            var guestServiceAllException =
                new GuestServiceAllException(exception);

            this.loggingBroker.LogError(guestServiceAllException);

            return guestServiceAllException;
        }
    }
}
