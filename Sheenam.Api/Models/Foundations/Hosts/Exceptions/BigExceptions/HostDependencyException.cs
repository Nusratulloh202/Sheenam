﻿//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Hosts.Exceptions.BigExceptions
{
    public class HostDependencyException : Xeption
    {
        public HostDependencyException(Xeption innerException)
            : base(message: "Host dependecy error occured, contact support. ", innerException)
        { }
    }
}
