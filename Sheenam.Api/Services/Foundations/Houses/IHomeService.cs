//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System.Threading.Tasks;
using Sheenam.Api.Models.Foundations.Houses;

namespace Sheenam.Api.Services.Foundations.Houses
{
    public interface IHomeService
    {
        ValueTask<Home> AddHomeAsync(Home home);
        //IQueryable<Home> RetrieveAllHomes();
        //ValueTask<Home> RetrieveByIdHomeAsync(Guid homeId);
        //ValueTask<Home> ModifyHomeAsync(Home home);
        //ValueTask<Home> RemoveHomeAsync(Guid homeId);

    }
}
