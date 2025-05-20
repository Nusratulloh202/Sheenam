//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;
using Sheenam.Api.Models.Foundations.Home.Enums;
using Sheenam.Api.Models.Foundations.Hosts;

namespace Sheenam.Api.Models.Foundations.Home
{
    public class Home
    {
        public Guid Id { get; set; }
        public Guid HostId { get; set; }
        public Host Host { get; set; } // Navigation property
        public string Address { get; set; }
        public string AdditionalInfo { get; set; }
        public bool IsVacant { get; set; }
        public int NumberOfBedrooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public double AreaInSquareMeters { get; set; }
        public bool IsPetAllowed { get; set; }
        public bool IsShared { get; set; }
        public HomeType Type { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
