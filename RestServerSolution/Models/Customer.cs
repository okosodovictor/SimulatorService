using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Hosting;
using System.Xml.Linq;

namespace RestServerSolution.Models
{
	public class Customer : Model
	{
        [Required(ErrorMessage = "Id is required.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Age is required.")]
        [Range(19, int.MaxValue, ErrorMessage = "Age must be above 18.")]
        public int Age { get; set; }

        public override string ToString()
        {
            return $"{Id},{FirstName},{LastName},{Age}";
        }
    }
}

