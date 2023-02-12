using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateOnly DateOfBirth { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        [Required]
        public double LoanAmount { get; set; }
        public int ProductId { get; set; }
        public int NumberOfRepayments { get; set; }
    }
}