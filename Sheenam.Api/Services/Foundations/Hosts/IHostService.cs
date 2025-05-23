﻿//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;
using System.Linq;
using System.Threading.Tasks;
using Sheenam.Api.Models.Foundations.Hosts;

namespace Sheenam.Api.Services.Foundations.Hosts
{
    public interface IHostService
    {
        ValueTask<Host> AddHostAsync(Host host);
        IQueryable<Host> RetriveAllHosts();
        ValueTask<Host> RetrieveByIdHostAsync(Guid hostId);
        ValueTask<Host> ModifyHostAsync(Host host);
        ValueTask<Host> RemoveHostAsync(Guid hostId);
    }
}
