using System;

namespace LBHAddressesAPI.Exceptions
{
    public class MissingEnvironmentVariableException : Exception
    {
        public MissingEnvironmentVariableException(string message) : base(message)
        {

        }
    }
}
