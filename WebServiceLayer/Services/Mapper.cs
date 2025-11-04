using DataServiceLayer.Domains;
using DataServiceLayer.DTOs;
using MapsterMapper;
using WebServiceLayer.Controllers;
using WebServiceLayer.DTOs;

namespace WebServiceLayer.Services
{
    public class Mapper
    {
        private  IMapper _mapper;
        private LinkGenerator _linkGenerator;
        private IHttpContextAccessor _httpContextAccessor;

        public Mapper(IMapper mapper, LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
        }

        //Mapping 

        public UserInfo? CreateUserDto(User user)
        {
            if (user == null)
            {
                return null;
            }

            var result = _mapper.Map<UserInfo>(user);
            result.UrlUser = GetUrl(nameof(UserController.GetUser), new {username = user.Username });

            return result;
        }

        public UserMinimumInfo? UserMinimumInfoDto(UserMinimumDetailsDto userDto)
        {
            if (userDto == null)
            {
                return null;
            }

            var result = _mapper.Map<UserMinimumInfo>(userDto);
            result.UrlUser = GetUrl(nameof(UserController.GetUser), new { username = userDto.Username });
            return result;
        }

        public UserMinimumInfo UserMinimumInfoDto(User user)
        {
            if (user == null)
            {
                return null;
            }

            var result = _mapper.Map<UserMinimumInfo>(user);
            result.UrlUser = GetUrl(nameof(UserController.GetUser), new { username = user.Username });
            return result;
        }

        public UserMinimumInfo? AllUsers(UserMinimumDetailsDto userDto)
        {
            if (userDto == null)
            {
                return null;
            }

            var result = _mapper.Map<UserMinimumInfo>(userDto);
            result.UrlUser = GetUrl(nameof(UserController.GetUser), new { username = userDto.Username });
            return result;
        }

        //Rating mapping
        public UserRatingDto? RatingDto(RatingDto ratingDto)
        {
            if (ratingDto == null)
            {
                return null;
            }
            var result = _mapper.Map<UserRatingDto>(ratingDto);
            result.TitleUrl = GetUrl(nameof(TitleController.GetTitle), new { title = ratingDto.TitleId });

            return result;
        }




        //Helper method to generate URLs

        private string? GetUrl(string endpointName, object values)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return null;
            return _linkGenerator.GetUriByName(httpContext, endpointName, values);
        }
    }
}
