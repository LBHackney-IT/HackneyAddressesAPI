using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackneyAddressesAPI.Models
{
    //Didnt realise HackneyAPI actually does return a "ValidationResult" and is similar to this, may implement same way they have done.
    public class ValidationResult
    {
        public ValidationResult()
        {
            this.ErrorMessages = new List<ApiErrorMessage>();
            this.ErrorOccurred = false;
        }

        public List<ApiErrorMessage> ErrorMessages { get; set; }
        public bool ErrorOccurred { get; set; }
    }
}