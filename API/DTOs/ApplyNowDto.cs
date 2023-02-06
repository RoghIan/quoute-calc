using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ApplyNowDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int NumberOfRepayments { get; set; }
        public double LoanAmount { get; set; }
        public double MonthlyRepaymentAmount { get; set; }
        public double TotalInterest { get; set; }
        public double TotalCost { get; set; }
    }
}