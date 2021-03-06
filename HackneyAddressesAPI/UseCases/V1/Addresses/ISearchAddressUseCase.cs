﻿using LBHAddressesAPI.Infrastructure.V1.UseCase;
using LBHAddressesAPI.UseCases.V1.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHAddressesAPI.UseCases.V1.Addresses
{
    public interface ISearchAddressUseCase : IRawUseCaseAsync<SearchAddressRequest, SearchAddressResponse>
    {

    }
}
