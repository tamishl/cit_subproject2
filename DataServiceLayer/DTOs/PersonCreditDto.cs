using DataServiceLayer.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.DTOs;

//This is an array containing the list of credits for the person, 
//with each credit representing a movie or TV show they worked on.
public class PersonCreditDto
{
    public string? TitleId { get; set; }
    public string? Title { get; set; }
    public string? Year { get; set; }
    public string? Department { get; set; }   // Actor, Director, Writer
    public string? Role { get; set; }        // Character name or job title

}
