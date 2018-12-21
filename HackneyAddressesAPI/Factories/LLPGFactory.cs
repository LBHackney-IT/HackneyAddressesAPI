using LBHAddressesAPI.Interfaces;
using LBHAddressesAPI.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LBHAddressesAPI.DB;

namespace LBHAddressesAPI.Factories
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
