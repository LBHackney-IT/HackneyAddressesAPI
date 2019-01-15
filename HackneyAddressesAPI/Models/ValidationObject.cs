using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHAddressesAPI.Models
{
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