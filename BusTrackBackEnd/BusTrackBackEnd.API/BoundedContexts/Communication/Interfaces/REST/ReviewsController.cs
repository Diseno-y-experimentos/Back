using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace BusTrackBackEnd.API.BoundedContexts.Communication.Interfaces.REST
{
    [ApiController]
    [Route("api/v1/reviews")]
    public class ReviewsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetReviews()
        {
            return Ok(new List<object>());
        }
    }
}
