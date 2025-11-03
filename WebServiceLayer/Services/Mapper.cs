using DataServiceLayer.Domains;
using MapsterMapper;
using WebServiceLayer.Controllers;
using WebServiceLayer.DTOs;

namespace WebServiceLayer.Services
{
    public class Mapper
    {
        public  IMapper _mapper;
        private LinkGenerator _linkGenerator;
        private HttpContextAccessor _httpContextAccessor;

        public Mapper(IMapper mapper, LinkGenerator linkGenerator, HttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
        }

        //Mapping 

        private CreateUser CreateUserDto(User user)
        {
            var dto = _mapper.Map<CreateUser>(user);
            dto.UrlNewUser = GetUrl(nameof(UserController.GetUser), new {username = user.Username });

            return dto;
        }


        //Helper method to generate URLs

        protected string? GetUrl(string endpointName, object values)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            return _linkGenerator.GetUriByName(httpContext, endpointName, values);
        }
    }
}
