using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using LBHAddressesAPI.UseCases.V1.Exceptions;

namespace LBHAddressesAPI.Controllers.V1
{
    [ApiVersion("1")]
    [Produces("application/json")]
    [Route("api/v1/healthcheck")]
    [ApiController]
    public class HealthCheckController : BaseController
    {
        [HttpGet]
        [Route("ping")]
        [ProducesResponseType(typeof(Dictionary<string, bool>), 200)]
        public IActionResult HealthCheck()
        {
            var result = new Dictionary<string, bool> { { "success", true } };

            return Ok(result);
        }

        [HttpGet]
        [Route("error")]
        public void ThrowError()
        {
            ThrowOpsErrorUsecase.Execute();
        }
    }
}
