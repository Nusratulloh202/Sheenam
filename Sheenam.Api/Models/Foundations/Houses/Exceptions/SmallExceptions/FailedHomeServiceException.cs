﻿//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Houses.Exceptions.SmallExceptions
{
    public class FailedHomeServiceException : Xeption
    {
        public FailedHomeServiceException(Exception innerException)
            : base(
                message: "Failed home service error occurred, contact support.",
                innerException)
        { }
    }
}
