//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;
using Sheenam.Api.Models.Foundations.Home;
using Sheenam.Api.Models.Foundations.Home.Enums;
using Sheenam.Api.Models.Foundations.Houses.Exceptions.SmallExceptions;

namespace Sheenam.Api.Services.Foundations.Houses
{
    public partial class HomeService
    {
        private static void ValidateHomeNotNull(Home home)
        {
            if (home is null)
            {
                throw new NullHomeException();
            }
        }
        private static void ValidateHome(Home home)
        {
            ValidateHomeNotNull(home);
            Validate(
                (Rule: IsInvalid(home.Id), Parameter: nameof(home.Id)),
                (Rule: IsInvalid(home.HostId), Parameter: nameof(home.HostId)),
                (Rule: IsInvalid(home.Address), Parameter: nameof(home.Address)),
                (Rule: IsInvalid(home.AdditionalInfo), Parameter: nameof(home.AdditionalInfo)),
                (Rule: IsInvalid(home.IsVacant), Parameter: nameof(home.IsVacant)),
                (Rule: IsInvalid(home.NumberOfBedrooms), Parameter: nameof(home.NumberOfBedrooms)),
                (Rule: IsInvalid(home.NumberOfBathrooms), Parameter: nameof(home.NumberOfBathrooms)),
                (Rule: IsInvalid(home.AreaInSquareMeters), Parameter: nameof(home.AreaInSquareMeters)),
                (Rule: IsInvalid(home.IsPetAllowed), Parameter: nameof(home.IsPetAllowed)),
                (Rule: IsInvalid(home.IsShared), Parameter: nameof(home.IsShared)),
                (Rule: IsInvalid(home.Type), Parameter: nameof(home.Type)),
                (Rule: IsInvalid(home.Price), Parameter: nameof(home.Price)),
                (Rule: IsInvalid(home.CreatedDate), Parameter: nameof(home.CreatedDate)),
                (Rule: IsInvalid(home.UpdatedDate), Parameter: nameof(home.UpdatedDate))
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
        private static dynamic IsInvalid(decimal decimalNumber) => new
        {
            Condition = decimalNumber <= 0,
            Message = "Number is required"
        };
        private static dynamic IsInvalid(int intNumber) => new
        {
            Condition = intNumber <= 0,
            Message = "Number is required"
        };
        private static dynamic IsInvalid(double doubleNumber) => new
        {
            Condition = doubleNumber <= 0,
            Message = "Number is required"
        };
        private static dynamic IsInvalid(bool? boolean) => new
        {
            Condition = boolean == default,
            Message = "Boolean is required"
        };
        private static dynamic IsInvalid(HomeType homeType) => new
        {
            Condition = Enum.IsDefined(typeof(HomeType), homeType) is false,
            Message = "Value is invalid"
        };

        private static void Validate(params (dynamic Rule, string Paramet)[] validations)
        {
            var invalidHomeException = new InvalidHomeException();
            foreach ((dynamic rule, string parametr) in validations)
            {
                if (rule.Condition)
                {
                    invalidHomeException.UpsertDataList(
                        key: parametr,
                        value: rule.Message);
                }
            }
            invalidHomeException.ThrowIfContainsErrors();
        }
    }
}
