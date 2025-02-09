//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;

namespace Sheenam.Api.Brokers.Logings
{
    public interface ILoggingBroker
    {
        public void LogError(Exception exception);
        public void LogCritical(Exception exception);
    }
}
