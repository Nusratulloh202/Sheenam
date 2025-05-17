//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace Sheenam.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class HousesController : RESTFulController
    {
        [HttpGet]
        public ActionResult<string> Get() =>
            Ok("Hello, Sheenam");
    }
}
