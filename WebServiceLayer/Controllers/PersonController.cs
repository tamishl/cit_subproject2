using DataServiceLayer.DTOs;
using DataServiceLayer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebServiceLayer.Controllers
{
    [Route("api/people")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        // Get detailed information about a person
        [HttpGet("{id}", Name = nameof(GetPerson))]
        public IActionResult GetPerson(string id)
        {
            var personDetails = _personService.GetPerson(id);

            if (personDetails == null)
            {
                return NotFound("Person not found.");
            }

            return Ok(personDetails);  // Return the detailed person info
        }

        // Get paginated list of people by name search
        [HttpGet("search")]
        public IActionResult GetPersonsByName(
            [FromQuery] string search,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 10)
        {
            var people = _personService.GetPersonsByName(search, page, pageSize);

            if (people.Items != null && people.Items.Any())  // Check if Items is not null and has any items
            {
                return Ok(people);  // Return the paginated list of people
            }
            else
            {
                return NotFound("No people found with the given search term.");  // Return "Not Found" if no people
            }

        }

        // Get paginated list of credits for a person
        [HttpGet("{id}/credits")]
        public IActionResult GetPersonCredits(string id, [FromQuery] int page = 0, [FromQuery] int pageSize = 10)
        {
            var credits = _personService.GetPersonCredits(id, page, pageSize);

            if (credits.Items != null && credits.Items.Any())  // Check if there are any credits
            {
                return Ok(credits);  // Return the paginated list of credits
            }
            else
            {
                return NotFound("No credits found for the person.");  // Return "Not Found" if no credits
            }
        }
    
    }
}
