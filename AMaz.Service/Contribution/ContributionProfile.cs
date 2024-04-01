using AutoMapper;
using AMaz.Common;
using AMaz.Entity;

public class ContributionProfile : Profile
{
    public ContributionProfile()
    {
        // Mapping for CreateContributionViewModel to CreateContributionRequest
        CreateMap<CreateContributionViewModel, CreateContributionRequest>();

        // Mapping for CreateContributionRequest to Contribution
        CreateMap<CreateContributionRequest, Contribution>()
            .ForMember(dest => dest.Files, opt => opt.Ignore());

        // Mapping for Contribution to ContributionViewModel and vice versa
        CreateMap<Contribution, ContributionViewModel>().ForMember(dest => dest.Magazine, opt => opt.MapFrom(src => src.Magazine.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.GetName((ContributionStatus)src.Status)))
            .ReverseMap();

        // Mapping for Contribution to ContributionDetailViewModel
        CreateMap<Contribution, ContributionDetailViewModel>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.GetName((ContributionStatus)src.Status)))
            .ForMember(dest => dest.Magazine, opt => opt.MapFrom(src => src.Magazine))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.Files))
            .ReverseMap();

        // Mapping for ContributionDetailViewModel to UpdateContributionViewModel
        CreateMap<ContributionDetailViewModel, UpdateContributionViewModel>()
            .ForMember(dest => dest.Files, opt => opt.Ignore());

        // Mapping for UpdateContributionViewModel to UpdateContributionRequest
        CreateMap<UpdateContributionViewModel, UpdateContributionRequest>();

        // Mapping for UpdateContributionRequest to Contribution
        CreateMap<UpdateContributionRequest, Contribution>()
            .ForMember(dest => dest.Files, opt => opt.Ignore());

        // Mapping for Contribution to UpdateContributionViewModel
        CreateMap<Contribution, UpdateContributionViewModel>()
            .ForMember(dest => dest.Files, opt => opt.Ignore());
    }
}
