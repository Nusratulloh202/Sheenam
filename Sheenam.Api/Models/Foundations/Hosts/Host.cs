//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;
using System.Collections.Generic;
using Sheenam.Api.Models.Foundations.Enums;

using Sheenam.Api.Models.Foundations.Houses;

namespace Sheenam.Api.Models.Foundations.Hosts
{
    public class Host
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public GenderType HostGender { get; set; }
        public ICollection<Home> Homes { get; set; } // optional

    }
}
