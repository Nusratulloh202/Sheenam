//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Sheenam.Api.Models.Foundations.Hosts;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions.BigExceptions;
using Sheenam.Api.Models.Foundations.Hosts.Exceptions.SmallExceptions;
using Sheenam.Api.Services.Foundations.Hosts;

namespace Sheenam.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HostsController : RESTFulController
    {
        private readonly IHostService hostService;
        public HostsController(IHostService hostService)
        {
            this.hostService = hostService;
        }

        [HttpPost]
        public async ValueTask<ActionResult<Host>> PostHostAsync(Host host)
        {
            try
            {
                Host postedHost = await this.hostService.AddHostAsync(host);
                return Created(postedHost);
            }
            catch (HostValidationException hostValidationException)
            {
                return BadRequest(hostValidationException.InnerException);
            }
            catch (HostDependencyValidationException hostDependencyValidationException)
                when (hostDependencyValidationException.InnerException is AlreadyExistHostException)
            {
                return Conflict(hostDependencyValidationException.InnerException);
            }
            catch (HostDependencyValidationException hostDependencyValidationException)
            {
                return BadRequest(hostDependencyValidationException.InnerException);
            }
            catch (HostDependencyException hostDependencyException)
            {
                return InternalServerError(hostDependencyException.InnerException);
            }
            catch (HostServiceAllException hostServiceAllException)
            {
                return InternalServerError(hostServiceAllException.InnerException);
            }
        }
        [HttpGet("All")]
        public ActionResult<IQueryable<Host>> GetAllHosts()
        {
            try
            {
                IQueryable<Host> hosts = this.hostService.RetriveAllHosts();
                return Ok(hosts);
            }


            catch (HostDependencyException hostDependencyException)
            {
                return InternalServerError(hostDependencyException.InnerException);
            }
            catch (HostServiceAllException hostServiceAllException)
            {
                return InternalServerError(hostServiceAllException.InnerException);
            }
        }
    }
}
