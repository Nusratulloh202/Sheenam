//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;
using Sheenam.Api.Services.Foundations.Guests;

namespace Sheenam.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuestsController : RESTFulController
    {
        private readonly IGuestService guestService;
        public GuestsController(IGuestService guestService) =>
           this.guestService = guestService;
        [HttpPost]
        public async ValueTask<ActionResult<Guest>> PostGuestAsync(Guest guest)
        {
            try
            {
                Guest postedGuest = await this.guestService.AddGuestAsync(guest);
                return Created(postedGuest);
            }
            catch (GuestValidationException guestValidationException)
            {
                return BadRequest(guestValidationException.InnerException);
            }
            catch (GuestDependencyValidationException guestDependencyValidationException)
                when (guestDependencyValidationException.InnerException is AlreadyExistGuestException)
            {
                return Conflict(guestDependencyValidationException.InnerException);
            }
            catch (GuestDependencyValidationException guestsValidationException)
            {
                return BadRequest(guestsValidationException.InnerException);
            }
            catch (GuestDependencyException guestsValidationException)
            {
                return InternalServerError(guestsValidationException.InnerException);
            }
            catch (GuestServiceAllException guestServiceAllException)
            {
                return InternalServerError(guestServiceAllException.InnerException);
            }
        }
        [HttpGet("All")]
        public ActionResult<IQueryable<Guest>> GetAllGuests()
        {
            try
            {
                IQueryable<Guest> guests = this.guestService.RetrieveAllGuests();
                return Ok(guests);
            }

            
            catch (GuestDependencyException guestDependencyException)
            {
                return InternalServerError(guestDependencyException.InnerException);
            }
            catch (GuestServiceAllException guestServiceAllException)
            {
                return InternalServerError(guestServiceAllException.InnerException);
            }
        }

    }

}
