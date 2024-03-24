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
        CreateMap<Contribution, ContributionViewModel>().ReverseMap();

        // Mapping for UpdateContributionViewModel to UpdateContributionRequest
        CreateMap<UpdateContributionViewModel, UpdateContributionRequest>();

        // Mapping for UpdateContributionRequest to Contribution
        CreateMap<UpdateContributionRequest, Contribution>()
            .ForMember(dest => dest.Files, opt => opt.Ignore());
    }
}
