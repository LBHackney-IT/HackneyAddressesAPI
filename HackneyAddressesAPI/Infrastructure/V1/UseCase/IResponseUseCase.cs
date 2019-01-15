using System.Threading;
using System.Threading.Tasks;

namespace LBHAddressesAPI.Infrastructure.V1.UseCase
{
    public interface IResponseUseCase<TResponse>
    {
        Task<TResponse> ExecuteAsync(CancellationToken cancellationToken);
    }
}
