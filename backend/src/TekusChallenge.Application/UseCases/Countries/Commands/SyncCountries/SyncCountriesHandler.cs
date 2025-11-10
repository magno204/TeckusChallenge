using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekus.Transversal;
using TekusChallenge.Domain.Entities;
using TekusChallenge.Domain.Interfaces;

namespace TekusChallenge.Application.UseCases.Countries.Commands.SyncCountries;

public class SyncCountriesHandler : IRequestHandler<SyncCountriesCommand, Response<int>>
{
    private readonly IUnitOfWork _unitOfWork;

    public SyncCountriesHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<int>> Handle(SyncCountriesCommand request, CancellationToken cancellationToken)
    {
        var response = new Response<int>();
        int createdCount = 0;

        try
        {
            foreach (var countryCode in request.CountryCodes)
            {
                var existsInDb = await _unitOfWork.Countries.ExistsAsync(countryCode, cancellationToken);

                if (!existsInDb)
                {
                    if (request.CountriesFromApi.TryGetValue(countryCode, out var countryFromApi))
                    {
                        var newCountry = new Country
                        {
                            Code = countryFromApi.Code,
                            CodeAlpha3 = countryFromApi.CodeAlpha3,
                            Name = countryFromApi.Name,
                            Flag = countryFromApi.Flag
                        };

                        await _unitOfWork.Countries.AddAsync(newCountry, cancellationToken);
                        createdCount++;
                    }
                }
            }

            if (createdCount > 0)
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            response.IsSuccess = true;
            response.Data = createdCount;
            response.Message = createdCount > 0
                ? $"{createdCount} country(ies) synchronized successfully."
                : "All countries already exist in the database.";

            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = $"Error synchronizing countries: {ex.Message}";
            response.Data = 0;
            return response;
        }
    }
}
