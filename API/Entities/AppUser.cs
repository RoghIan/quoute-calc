using API.Extensions;

namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public double LoanAmount { get; set; }
        public int NumberOfRepayments { get; set; }
        public int ProductId { get; set; }
        public int GetAge()
        {
            return DateOfBirth.CalculateAge();
        }
    }
}