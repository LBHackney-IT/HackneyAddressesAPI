using LBHAddressesAPI.Extensions.Controller;
using LBHAddressesAPI.Infrastructure.V1.UseCase.Execution;
using Microsoft.AspNetCore.Mvc;

namespace LBHAddressesAPI.Controllers.V1
{
    public class BaseController : Controller
    {
        public IActionResult HandleResponse<T>(T result) where T : class
        {
            return this.StandardResponse(result);
        }
    }
}
