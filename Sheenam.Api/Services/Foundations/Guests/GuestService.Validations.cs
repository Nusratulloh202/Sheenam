﻿//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Enums;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;

namespace Sheenam.Api.Services.Foundations.Guests
{
    public partial class GuestService
    {
        private static void ValidateGuestNotNull(Guest guest)
        {
            if (guest is null)
            {
                throw new NullGuestException();
            }
        }
        private static void ValidateStorageGuestExist(Guest maybeGuest, Guid guestId)
        {
            if (maybeGuest is null)
            {
                throw new NotFoundGuestException(guestId);
            }
        }
        private void ValidateGuestOnAdd(Guest guest)
        {
            ValidateGuestNotNull(guest);

            Validate(
                    (Rule: IsInvalid(guest.Id), Parameter: nameof(guest.Id)),
                    (Rule: IsInvalid(guest.FirstName), Parameter: nameof(guest.FirstName)),
                    (Rule: IsInvalid(guest.LastName), Parameter: nameof(guest.LastName)),
                    (Rule: IsInvalid(guest.Email), Parameter: nameof(guest.Email)),
                    (Rule: IsInvalid(guest.PhoneNumber), Parameter: nameof(guest.PhoneNumber)),
                    (Rule: IsInvalid(guest.Address), Parameter: nameof(guest.Address)),
                    (Rule: IsInvalid(guest.DateOffBirth), Parameter: nameof(guest.DateOffBirth)),
                    (Rule: IsInvalid(guest.Gender), Parameter: nameof(guest.Gender)));
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
        private static dynamic IsInvalid(GenderType gender) => new
        {
            Condition = Enum.IsDefined(gender) is false,
            Message = "Value is invalid"
        };
        private static void ValidateGuestId(Guid guestId) =>
            Validate((Rule: IsInvalid(guestId), Parameter: nameof(Guest.Id)));
        private static void ValidateStorageGuest(Guest maybeGuest, Guid guestId)
        {
            if (maybeGuest is null)
            {
                throw new NotFoundGuestException(guestId);
            }
        }
        private static void ValidateGuestOnModify(Guest guest)
        {
            ValidateGuestNotNull(guest);

            Validate(
                    (Rule: IsInvalid(guest.Id), Parameter: nameof(guest.Id)),
                    (Rule: IsInvalid(guest.FirstName), Parameter: nameof(guest.FirstName)),
                    (Rule: IsInvalid(guest.LastName), Parameter: nameof(guest.LastName)),
                    (Rule: IsInvalid(guest.Email), Parameter: nameof(guest.Email)),
                    (Rule: IsInvalid(guest.PhoneNumber), Parameter: nameof(guest.PhoneNumber)),
                    (Rule: IsInvalid(guest.Address), Parameter: nameof(guest.Address)),
                    (Rule: IsInvalid(guest.DateOffBirth), Parameter: nameof(guest.DateOffBirth)),
                    (Rule: IsInvalid(guest.Gender), Parameter: nameof(guest.Gender)));
        }

        private static void ValidateAgainstStorageGuestOnModify(Guest guest, Guest storageGuest)
        {

            ValidateStorageGuestExist(storageGuest, guest.Id);

            Validate(
                (Rule: IsInvalid(guest.Id), Parameter: nameof(Guest.Id)),
                (Rule: IsInvalid(guest.FirstName), Parameter: nameof(Guest.FirstName)),
                (Rule: IsInvalid(guest.LastName), Parameter: nameof(Guest.LastName)),
                (Rule: IsInvalid(guest.DateOffBirth), Parameter: nameof(Guest.DateOffBirth)),
                (Rule: IsInvalid(guest.Email), Parameter: nameof(Guest.Email)),
                (Rule: IsInvalid(guest.PhoneNumber), Parameter: nameof(Guest.PhoneNumber)),
                (Rule: IsInvalid(guest.Address), Parameter: nameof(Guest.Address)));
        }

        private static void Validate(params (dynamic Rule, string Parametr)[] validations)
        {
            var invalidGuestException = new InvalidGuestException();

            foreach ((dynamic rule, string parametr) in validations)
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
