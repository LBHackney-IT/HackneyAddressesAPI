using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using HackneyAddressesAPI.Interfaces;

namespace HackneyAddressesAPI.Tests.Actions
{
    public class AddressActionsTests
    {
        [Fact]
        public async Task Get_addresses_by_postcode_default_no_filters_return_simple_addresses()
        {

            /*Search for address via postcode. Default search.
            Should return local and national (and out of borough - which overrides equivalent NLPG approved preferred address)
            Simple format
            addressStatus Approved Preferred
            Filter out property shells (Should be no Usage classification = P)
            Should be limited to 50 results - default
                */
        }

        [Fact]
        public async Task Get_addresses_by_postcode_default_no_filters_return_no_addresses()
        {
            /*Search for address via invalid postcode. Default search.
            Should return no addresses
                */
        }

        [Fact]
        public async Task Get_address_by_lpikey_return_address()
        {
             
            /*Retrieve an address via lpi_key.
                Return a single address detailed format.
                */
        }

        [Fact]
        public async Task get_addres_by_invalid_lpikey_return_no_address()
        {
            /*Retrieve an address via invalid lpi_key.
                Return error or blank???
                */
        }

        [Fact]
        public async Task get_addresses_by_postcode_default_with_paging_return_simple_addresses()
        {
            /*Return paged addresses
             Search for address via postcode. Limiting results to X at a time. (Limit and offset)
            Should perform paging
            Simple format
            addressStatus Approved Preferred
            Filter out property shells
            */
        }

        [Fact]
        public async Task get_addresses_by_postcode_local_gazeteer_return_simple_addresses()
        {
            /*Should return only local (NOT OOB) addresses
            Simple format
            addressStatus Approved Preferred
            Filter out property shells
            */
        }

        [Fact]
        public async Task get_addresses_by_postcode_local_gazeteer_include_oob_return_simple_addresses()
        {
            /*Should return only local (AND OOB) addresses
            Simple format
            addressStatus Approved Preferred
            Filter out property shells
            */
        }

        [Fact]
        public async Task get_addresses_by_postcode_detailed_format_return_detailed_addresses()
        {
            /*Search for address via postcode asking for detailed addresses
            Should return local and national (and out of borough - which overrides equivalent NLPG approved preferred address)
            Detailed format
            addressStatus Approved Preferred
            Filter out property shells
            */
        }

        [Fact]
        public async Task get_addresses_by_postcode_with_status_return_simple_addresses()
        {
            /*Search for an address via postcode passing in an address status of “X”
            Should return local and national addresses
            Address status should be “X”
            */
        }

        public async Task get_addresses_by_postcode_with_multiple_statuses_return_simple_address()
        {
            /*Search for an address via postcode passing in an address status of “X” and address status of ‘Y’
            Should return local and national addresses
            Address status should be “X” or ‘Y’
            */
        }

        public async Task get_addresses_by_postcode_with_usage_classification_return_simple_address()
        {
            /*Search for an address via post code and passing in usage classification code of ‘X’
            Should return local and national (and out of borough - which overrides equivalent NLPG approved preferred address)
            Simple format
            addressStatus Approved Preferred
            Filter out property shells (Should be no Usage classification = P)
            Usage classification should be ‘X’
            */
        }

        public async Task get_addresses_by_postcode_with_multiple_usage_classifications_return_simple_address()
        {
            /*Search for an address via post code and passing in usage classification codes of ‘X’ and usage classification codes of ‘Y’
            Should return local and national (and out of borough - which overrides equivalent NLPG approved preferred address)
            Simple format
            addressStatus Approved Preferred
            Filter out property shells (Should be no Usage classification = P)
            Usage classification should be ‘X’ or ‘Y’
            */
        }
    }
}
