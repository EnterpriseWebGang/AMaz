using AutoMapper;
using AMaz.Common;
using AMaz.Entity;

public class ContributionProfile : Profile
{
    public ContributionProfile()
    {
        CreateMap<CreateContributionViewModel, CreateContributionRequest>();
        CreateMap<CreateContributionRequest, Contribution>().ForMember(dest => dest.Files, opt => opt.Ignore());

    }
}
