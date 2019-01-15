using System.Threading;
using System.Threading.Tasks;

namespace LBHAddressesAPITest.Helpers.Stub
{
    public interface IRepository<AddressDetails> 
    {
        Task<AddressDetails> GetAddressAsync(string lpi_key);

    }
}
