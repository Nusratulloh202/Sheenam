//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Sheenam.Api.Models.Foundations.Hosts;
using Sheenam.Api.Models.Foundations.Houses.Enums;

namespace Sheenam.Api.Models.Foundations.Houses
{
    public class Home
    {
        public Guid Id { get; set; }
        public Guid HostId { get; set; }
        [JsonIgnore]
        public Host Host { get; set; } // Navigation property
        public string Address { get; set; }
        public string AdditionalInfo { get; set; }
        [Required]
        public bool? IsVacant { get; set; }
        public int NumberOfBedrooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public double AreaInSquareMeters { get; set; }
        [Required]
        public bool? IsPetAllowed { get; set; }
        [Required]
        public bool? IsShared { get; set; }
        public HomeType Type { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
