using HackneyAddressesAPI.Interfaces;
using HackneyAddressesAPI.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackneyAddressesAPI.DB;

namespace HackneyAddressesAPI.Factories
{
    internal class LLPGFactory
    {
        //Ask about if this should be called services or DB etc
        public IDB_Helper build()
        {
            if (TestStatus.IsRunningInTests == false)
            {
                //return new DB_LLPG();
                return null;
            }
            else
            {
                //return new FakeDB_LLPG();
                return null;
            }
        }


    }
}
