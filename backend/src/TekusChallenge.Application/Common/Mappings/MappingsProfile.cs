using AutoMapper;
using TekusChallenge.Domain.Entities;
using TekusChallenge.Application.DTO;
using TekusChallenge.Application.UseCases.ProviderCustomFields.Commands.CreateProviderCustomField;
using TekusChallenge.Application.UseCases.ProviderCustomFields.Commands.UpdateProviderCustomField;
using TekusChallenge.Application.UseCases.Providers.Commands.CreateProvider;
using TekusChallenge.Application.UseCases.Services.Commands.CreateService;
using TekusChallenge.Application.UseCases.Services.Commands.UpdateService;

namespace TekusChallenge.Application.Common.Mappings;

public class MappingsProfile: Profile
{
    public MappingsProfile()
    {
        CreateMap<Provider, ProviderDto>().ReverseMap();
        
        CreateMap<Service, ServiceDto>()
            .ForMember(dest => dest.Countries, 
                opt => opt.MapFrom(src => src.ServiceCountries.Select(sc => sc.Country)))
            .ForMember(dest => dest.ProviderName,
                opt => opt.MapFrom(src => src.Provider != null ? src.Provider.Name : null));
        
        CreateMap<Country, CountryDto>().ReverseMap();
        CreateMap<ServiceCountry, ServiceCountryDto>().ReverseMap();
        CreateMap<ProviderCustomField, ProviderCustomFieldDto>().ReverseMap();

        // Command mappings
        CreateMap<CreateProviderCommand, Provider>()
            .ForMember(dest => dest.Services, opt => opt.Ignore())
            .ForMember(dest => dest.CustomFields, opt => opt.Ignore());
            
        CreateMap<CreateServiceCommand, Service>();
        CreateMap<UpdateServiceCommand, Service>();
        CreateMap<CreateProviderCustomFieldCommand, ProviderCustomField>();
        CreateMap<UpdateProviderCustomFieldCommand, ProviderCustomField>();
    }
}
