
namespace LBHAddressesAPI.UseCases.V1.Exceptions
{
    public class ThrowOpsErrorUsecase
    {
        public static void Execute()
        {
            throw new TestOpsErrorException();
        }
    }
}
