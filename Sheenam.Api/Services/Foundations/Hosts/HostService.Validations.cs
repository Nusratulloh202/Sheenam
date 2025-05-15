//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;
using Sheenam.Api.Models.Foundations.Enums;
using Sheenam.Api.Models.Foundations.Hosts;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions.SmallExceptions;

namespace Sheenam.Api.Services.Foundations.Hosts
{
    public partial class HostService
    {
        private static void ValidateHostNotNull(Host host)
        {
            if (host is null)
            {
                throw new NullHostException();
            }
        }
        private void ValidateHost(Host host)
        {
            ValidateHostNotNull(host);

            Validate(
                (Rule: IsInvalid(host.Id), Parameter: nameof(host.Id)),
                (Rule: IsInvalid(host.FirstName), Parameter: nameof(host.FirstName)),
                (Rule: IsInvalid(host.LastName), Parameter: nameof(host.LastName)),
                (Rule: IsInvalid(host.Email), Parameter: nameof(host.Email)),
                (Rule: IsInvalid(host.PhoneNumber), Parameter: nameof(host.PhoneNumber)),
                (Rule: IsInvalid(host.DateOfBirth), Parameter: nameof(host.DateOfBirth)),
                (Rule: IsInvalid(host.HostGender), Parameter: nameof(host.HostGender)));
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
        private static void ValidateHostId(Guid hostId) =>
           Validate((Rule: IsInvalid(hostId), Parameter: nameof(Host.Id)));

        private static void ValidateStorageHost(Host maybeHost, Guid hostId)
        {
            if (maybeHost is null)
            {
                throw new NotFoundHostException(hostId);
            }
        }



        private static void ValidateHostOnModify(Host host)
        {
            ValidateHostNotNull(host);
            Validate(
                (Rule: IsInvalid(host.Id), Parameter: nameof(host.Id)),
                (Rule: IsInvalid(host.FirstName), Parameter: nameof(host.FirstName)),
                (Rule: IsInvalid(host.LastName), Parameter: nameof(host.LastName)),
                (Rule: IsInvalid(host.Email), Parameter: nameof(host.Email)),
                (Rule: IsInvalid(host.PhoneNumber), Parameter: nameof(host.PhoneNumber)),
                (Rule: IsInvalid(host.DateOfBirth), Parameter: nameof(host.DateOfBirth)));
        }
        private static void ValidateAgainstStorageHostOnModify(Host host, Host storageHost)
        {
            ValidateStorageHost(storageHost, host.Id);
            Validate(
                (Rule: IsInvalid(host.FirstName), Parameter: nameof(host.FirstName)),
                (Rule: IsInvalid(host.LastName), Parameter: nameof(host.LastName)),
                (Rule: IsInvalid(host.Email), Parameter: nameof(host.Email)),
                (Rule: IsInvalid(host.PhoneNumber), Parameter: nameof(host.PhoneNumber)),
                (Rule: IsInvalid(host.DateOfBirth), Parameter: nameof(host.DateOfBirth)));
        }

        private static void Validate(params (dynamic Rule, string Paramet)[] validations)
        {
            var invalidHostExseption = new InvalidHostException();
            foreach ((dynamic rule, string parametr) in validations)
            {
                if (rule.Condition)
                {
                    invalidHostExseption.UpsertDataList(
                        key: parametr,
                        value: rule.Message);
                }
            }
            invalidHostExseption.ThrowIfContainsErrors();
        }
    }
}
