using System;
using LBHAddressesAPI.Infrastructure.V1.API;
using LBHAddressesAPI.Infrastructure.V1.Validation;

namespace LBHAddressesAPI.Infrastructure.V1.UseCase.Execution
{
    public class ExecuteWrapper<T>: IExecuteWrapper<T> where T:class
    {
        public bool IsSuccess { get; set; }
        public T Result { get; set; }
        public APIError Error { get; set; }

        public ExecuteWrapper(T response)
        {           
                IsSuccess = true;
                Result = response;
        }

        public ExecuteWrapper(RequestValidationResponse validationResponse)
        {
            Error = new APIError(validationResponse);
        }

        

        public ExecuteWrapper(Exception ex)
        {
            Error = new APIError(ex);
        }

        public ExecuteWrapper(ExecutionError ex)
        {
            Error = new APIError(ex);
        }

        public ExecuteWrapper(APIError apiError)
        {
            Error = apiError;
        }
    }
}
