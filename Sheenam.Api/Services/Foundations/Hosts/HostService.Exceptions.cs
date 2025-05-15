//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Sheenam.Api.Models.Foundations.Hosts;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions.BigExceptions;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions.SmallExceptions;
using Xeptions;

namespace Sheenam.Api.Services.Foundations.Hosts
{
    public partial class HostService
    {
        private delegate ValueTask<Host> ReturningHostFunction();
        private delegate IQueryable<Host> ReturningHostsFunction();

        private async ValueTask<Host> TryCatch(ReturningHostFunction returningHostFunction)
        {
            try
            {
                return await returningHostFunction();
            }
            catch (NullHostException nullHostException)
            {
                throw CreateAndLogValidationException(nullHostException);
            }
            catch (InvalidHostException invalidHostException)
            {
                throw CreateAndLogValidationException(invalidHostException);
            }
            catch (NotFoundHostException notFoundHostException)
            {
                throw CreateAndLogValidationException(notFoundHostException);
            }
            catch(DbUpdateException dbUpdateException)
            {
                var failedHostStorageException =
                    new FailedHostStorageException(dbUpdateException);
                throw CreateAndLogCriticalDependencyException(failedHostStorageException);
            }

            catch (SqlException sqlException)
            {
                var failedHostStorageException =
                    new FailedHostStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedHostStorageException);
            }
            catch (DuplicateKeyException dublicateKeyException)
            {
                var alreadyExistHostException =
                    new AlreadyExistHostException(dublicateKeyException);
                throw CreateAndLogDependencyValidationException(alreadyExistHostException);
            }
            catch (Exception exception)
            {
                var failedHostServiceException =
                    new FailedHostServiceException(exception);
                throw CreateAndLogServiceAllException(failedHostServiceException);
            }
        }
        private IQueryable<Host> TryCatch(ReturningHostsFunction returningHostsFunction)
        {
            try
            {
                return returningHostsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedHostStorageException =
                    new FailedHostStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedHostStorageException);
            }

            catch (Exception exception)
            {
                var failedHostServiceException =
                    new FailedHostServiceException(exception);
                throw CreateAndLogServiceAllException(failedHostServiceException);
            }
        }
        private HostValidationException CreateAndLogValidationException(Xeption exception)
        {
            var hostValidationException =
                new HostValidationException(exception);

            this.loggingBroker.LogError(hostValidationException);

            return hostValidationException;
        }
        private HostDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var hostDependencyException =
                new HostDependencyException(exception);
            this.loggingBroker.LogCritical(hostDependencyException);
            return hostDependencyException;
        }
        private HostDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var hostDependencyValidationException =
                new HostDependencyValidationException(exception);
            this.loggingBroker.LogError(hostDependencyValidationException);
            return hostDependencyValidationException;
        }
        private HostServiceAllException CreateAndLogServiceAllException(Xeption exception)
        {
            var hostServiceAllException =
                new HostServiceAllException(exception);

            this.loggingBroker.LogError(hostServiceAllException);

            return hostServiceAllException;
        }
    }
}
