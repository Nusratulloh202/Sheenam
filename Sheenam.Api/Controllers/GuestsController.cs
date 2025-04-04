//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System;
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

        [HttpGet("ById")]
        public async Task<ActionResult<Guest>> GetGuestByIdAsync(Guid guestId)
        {
            try
            {
                Guest guest = await this.guestService.RetrieveGuestByIdAsync(guestId);
                return Ok(guest);
            }
            catch (GuestDependencyException dependencyException)
            {
                return InternalServerError(dependencyException.InnerException);
            }
            catch (GuestValidationException guestValidationException)
                when (guestValidationException.InnerException is InvalidGuestException)
            {
                return BadRequest(guestValidationException.InnerException);
            }
            catch (GuestValidationException guestValidationException)
                when (guestValidationException.InnerException is NotFoundGuestException)
            {
                return NotFound(guestValidationException.InnerException);
            }
            catch (GuestServiceAllException guestServicesAllException)
            {
                return InternalServerError(guestServicesAllException.InnerException);
            }
        }
        [HttpPut]
        public async Task<ActionResult<Guest>> PutGuestAsync(Guest guest)
        {
            try
            {
                Guest modifiedGuest = await this.guestService.ModifyGuestAsync(guest);
                return Ok(modifiedGuest);
            }
            catch (GuestValidationException guestValidationException)
                when (guestValidationException.InnerException is NotFoundGuestException)
            {
                return NotFound(guestValidationException.InnerException);
            }

            catch (GuestValidationException guestValidationException)
            {
                return BadRequest(guestValidationException.InnerException);
            }
            catch (GuestDependencyException guestDependencyException)
            {
                return InternalServerError(guestDependencyException.InnerException);
            }
            catch (GuestServiceAllException guestServiceException)
            {
                return InternalServerError(guestServiceException.InnerException);
            }
        }

        //[HttpDelete]
        //public async Task<ActionResult<Guest>> DeleteGuestByIdAsync(Guid guestId)
        //{
        //    try
        //    {
        //        Guest deletedGuest = await this.guestService.RemoveGuestByIdAsync(guestId);
        //        return Ok(deletedGuest);
        //    }
        //    catch (GuestValidationException guestValidationException)
        //        when (guestValidationException.InnerException is InvalidGuestException)
        //    {
        //        return BadRequest(guestValidationException.InnerException);
        //    }
        //    catch (GuestValidationException guestValidationException)
        //        when (guestValidationException.InnerException is NotFoundGuestException)
        //    {
        //        return NotFound(guestValidationException.InnerException);
        //    }
        //    catch (GuestDependencyValidationException dependencyValidationException)
        //        when (dependencyValidationException.InnerException is LockedGuestException)
        //    {
        //        return Locked(dependencyValidationException.InnerException);
        //    }
        //    catch (GuestDependencyValidationException guestDependencyValidationException)
        //    {
        //        return BadRequest(guestDependencyValidationException.InnerException);
        //    }
        //    catch (GuestDependencyException guestDependencyException)
        //    {
        //        return InternalServerError(guestDependencyException.InnerException);
        //    }
        //    catch (GuestServiceAllException guestServiceAllException)
        //    {
        //        return InternalServerError(guestServiceAllException.InnerException);
        //    }
        //}
    }

}
