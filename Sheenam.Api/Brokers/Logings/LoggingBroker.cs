//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;
using Microsoft.Extensions.Logging;

namespace Sheenam.Api.Brokers.Logings
{
    public class LoggingBroker: ILoggingBroker
    {
        private readonly ILogger logger;

        public LoggingBroker(ILogger logger) =>
            this.logger = logger;

        public void LogError(Exception exception)=>
            this.logger.LogError(exception, exception.Message);
        public void  LogCritical(Exception exception)=>
            this.logger.LogCritical(exception, exception.Message);

    }
}
