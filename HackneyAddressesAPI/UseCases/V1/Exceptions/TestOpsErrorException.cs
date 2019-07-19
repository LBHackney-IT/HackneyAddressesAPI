using System;

namespace LBHAddressesAPI.UseCases.V1.Exceptions
{
    public class TestOpsErrorException : Exception
    {
        public TestOpsErrorException() : base("This is a test exception to test our integrations") { }
    }
}
