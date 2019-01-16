using LBHAddressesAPI.Infrastructure.V1.API;

namespace LBHAddressesAPI.Infrastructure.V1.UseCase.Execution
{
    public interface IExecuteWrapper<T>
    {
        bool IsSuccess { get; set; }
        T Result { get; set; }
        APIError Error { get; set; }
    }
}
