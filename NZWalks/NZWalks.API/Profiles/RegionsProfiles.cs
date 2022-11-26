using AutoMapper;

namespace NZWalks.API.Profiles
{
    public class RegionsProfiles: Profile
    {
        public RegionsProfiles()
        {
            CreateMap<Models.Domain.Region, Models.DTO.Region>().ReverseMap();
        }
    }
}
