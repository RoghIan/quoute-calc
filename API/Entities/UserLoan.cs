using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class UserLoan
    {
        public int Id { get; set; }
        public int NumberOfRepayments { get; set; }
        public double LoanAmount { get; set; }
        public double MonthlyRepaymentAmount { get; set; }
        public double TotalInterest { get; set; }
        public double TotalCost { get; set; }
        public AppUser AppUser { get; set; }
        public Product Product { get; set; }
    }
}