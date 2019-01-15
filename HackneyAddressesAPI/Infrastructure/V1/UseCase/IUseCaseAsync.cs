using System.Threading;
using System.Threading.Tasks;
using LBHAddressesAPI.Infrastructure.V1.API;
using LBHAddressesAPI.Infrastructure.V1.UseCase.Execution;

namespace LBHAddressesAPI.Infrastructure.V1.UseCase
{
    public interface IUseCaseAsync<TRequest, TResponse> where TRequest: IRequest 
    {
        Task<IExecuteWrapper<TResponse>> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
    }
}
