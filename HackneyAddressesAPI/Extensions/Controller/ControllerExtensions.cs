using System;
using LBHAddressesAPI.Infrastructure.V1.API;
using LBHAddressesAPI.Infrastructure.V1.UseCase.Execution;
using Microsoft.AspNetCore.Mvc;

namespace LBHAddressesAPI.Extensions.Controller
{
    /// <summary>
    /// Extension class to help format a positive response into an standardised API Response
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        /// Formats postive responses from UseCases into APIResponses
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static IActionResult StandardResponse<T>(this Microsoft.AspNetCore.Mvc.Controller controller, T result) where T : class
        {
            var apiResponse = new APIResponse<T>(result);
            //Use Extension Method to set a statusCode as well as an object
            return controller.StatusCode(apiResponse.StatusCode, apiResponse);
        }
    }
}
