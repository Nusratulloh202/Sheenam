//================================================
//Copyright(c) Coalition of Good-Hearted Engineers
//Free To Use To Find Comfort and Peace
//================================================

using System.Threading.Tasks;
using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Broker.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Guest> InsertGuestAsync<T>(Guest guest);
}
