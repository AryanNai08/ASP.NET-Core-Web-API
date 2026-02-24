
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //With the [EnableCors] attribute.
    [EnableCors(PolicyName = "AllowOnlyMicrosoft")]
 
    public class MicrosoftController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Get()
        {
            return Ok("THis is microsoft");
        }
    }
}
