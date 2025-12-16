using DataAccesLayer.DTOs;
using DataAccesLayer.ReadDTOs;
using DataServiceLayer.Domains;
using DataServiceLayer.DTOs;
using MapsterMapper;
using System.Globalization;
using WebServiceLayer.Controllers;
using WebServiceLayer.DTOs;

namespace WebServiceLayer.Services
{
    public class Mapper
    {
        private IMapper _mapper;
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
            result.TitleUrl = GetUrl(nameof(TitleController.GetTitle), new { Id = ratingDto.TitleId});

            return result;
        }

        public CreateRating? CreateRatingDto(RatingValueDto dto, string titleId)
        {
            if (dto == null)
            {
                return null;
            }
            var result = _mapper.Map<CreateRating>(dto);

            result.TitleUrl = GetUrl(nameof(TitleController.GetTitle), new { Id = titleId });

            return result;
        }


        public BookmarkTitleDtoWsl? BookmarkTitleDto(BookmarkTitleDto dto)
        {
            if (dto == null)
            {
                return null;
            }
            var result = _mapper.Map<BookmarkTitleDtoWsl>(dto);

            result.TitleUrl = GetUrl(nameof(TitleController.GetTitle), new { Id = dto.TitleId});
            result.BookmarkUrl = GetUrl(nameof(BookmarkController.CreateBookmarkTitle), new {titleId = dto.TitleId});

            return result;
        }

        public CreateBookmarkTitle CreateBookmarkTitleDto(BookmarkTitle bookmark)
        {
            if (bookmark == null)
            {
                return null;
            }

            var result = _mapper.Map<CreateBookmarkTitle>(bookmark);

            result.BookmarkUrl = GetUrl(nameof(BookmarkController.CreateBookmarkTitle), new {titleId = bookmark.TitleId});

            return result;
        }


        public BookmarkPersonDtoWsl? BookmarkPersonDto(BookmarkPersonDto dto)
        {
            if (dto == null)
            {
                return null;
            }
            var result = _mapper.Map<BookmarkPersonDtoWsl>(dto);

            result.PersonUrl = GetUrl(nameof(PersonController.GetPerson), new { Id = dto.PersonId });
            result.BookmarkUrl = GetUrl(nameof(BookmarkController.CreateBookmarkPerson), new {personId = dto.PersonId});

            return result;
        }

        public CreateBookmarkPerson CreateBookmarkPersonDto(BookmarkPerson bookmark)
        {
            if (bookmark == null)
            {
                return null;
            }

            var result = _mapper.Map<CreateBookmarkPerson>(bookmark);

            result.BookmarkUrl = GetUrl(nameof(BookmarkController.CreateBookmarkPerson), new {personId = bookmark.PersonId});

            return result;
        }


        public GetTitleRatingByGroupDto TitleRatingByGroupDto(List<RatingByGroupDto> groups, string titleId)
        {
            return new GetTitleRatingByGroupDto
            {
                TitleId = titleId,
                Url = GetUrl(nameof(TitleRatingController.GetTitleRatingsByGroup), new {titleId}),
                RatingGroups = groups
            };
        }

        public GetTitleRatingDto TitleRatingDto(TitleRatingDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            var result = _mapper.Map<GetTitleRatingDto>(dto);

            result.Url = GetUrl(nameof(TitleRatingController.GetTitleRating), new { titleId = dto.TitleId });

            return result;
        }

        public GetPersonRatingDto PersonRatingDto(PersonRatingDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            var result = _mapper.Map<GetPersonRatingDto>(dto);

            result.Url = GetUrl(nameof(PersonRatingController.GetPersonRating), new { personId = dto.PersonId });

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
