//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;
using System.Data;
using Microsoft.IdentityModel.Tokens;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;

namespace Sheenam.Api.Services.Foundations.Guests
{
    public partial class GuestService
    {
        private void ValidateGuestNotNull(Guest guest)
        {
            if (guest is null)
            {
                throw new NullGuestExceptions();
            }
        }
        private void ValidateGuestOnAdd(Guest guest)
        {
            ValidateGuestNotNull(guest);

            Validate(
                    (Rule: IsInvalid(guest.Id), Parametr: nameof(guest.Id)),
                    (Rule: IsInvalid(guest.FirstName), Parametr: nameof(guest.FirstName)),
                    (Rule: IsInvalid(guest.LastName), Parametr: nameof(guest.LastName)),
                    (Rule: IsInvalid(guest.Email), Parametr: nameof(guest.Email)),
                    (Rule: IsInvalid(guest.PhoneNumber), Parametr: nameof(guest.PhoneNumber)),
                    (Rule: IsInvalid(guest.Address), Parametr: nameof(guest.Address)),
                    (Rule: IsInvalid(guest.DateOffBirth), Parametr: nameof(guest.DateOffBirth))

                    );
        }
        
        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };
        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };
        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static  void Validate(params(dynamic Rule, string Parametr)[] validations)
        {
            var invalidGuestException = new InvalidGuestException();

            foreach((dynamic rule, string parametr) in validations)
            {
                if (rule.Condition)
                {
                    invalidGuestException.UpsertDataList(
                        key: parametr,
                        value: rule.Message);
                }
            }
            invalidGuestException.ThrowIfContainsErrors();
        }
    }
}
