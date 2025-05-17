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

        // Foreign key: Home belongs to a Host
        public Guid HostId { get; set; }
        public Host Host { get; set; } // Navigation property

        // Location info
        public string Address { get; set; }
        public string AdditionalInfo { get; set; }

        // Availability and features
        public bool IsVacant { get; set; }
        public int NumberOfBedrooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public double AreaInSquareMeters { get; set; }
        public bool IsPetAllowed { get; set; }
        public bool IsShared { get; set; }

        // Enum type for category
        public HomeType Type { get; set; }

        // Price per night or per month, depending on context
        public decimal Price { get; set; }

        // Audit info (optional but recommended for traceability)
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
