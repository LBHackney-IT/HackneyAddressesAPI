using System.Threading;
using System.Threading.Tasks;
using LBHAddressesAPI.Infrastructure.V1.API;

namespace LBHAddressesAPI.Infrastructure.V1.UseCase
{
    public interface IRawUseCaseAsync<TRequest, TResponse> where TRequest : IRequest
    {
        Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
    }
}
