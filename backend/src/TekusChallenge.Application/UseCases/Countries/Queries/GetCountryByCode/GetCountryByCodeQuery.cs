using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Tekus.Transversal;
using TekusChallenge.Application.DTO;

namespace TekusChallenge.Application.UseCases.Countries.Queries.GetCountryByCode;

public sealed record GetCountryByCodeQuery : IRequest<Response<CountryDto>>
{
    public string Code { get; init; } = string.Empty;
}

