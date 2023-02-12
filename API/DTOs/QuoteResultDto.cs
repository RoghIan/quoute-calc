using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class QuoteResultDto
    {
        public int NumberOfRepayments { get; set; }
        public double MonthlyRepaymentNoInterest { get; set; }
        public double MonthlyRepaymentWithInterest { get; set; }
        public double InterestValuePerMonth { get; set; }
        public double TotalCost { get; set; }
        public double TotalInterest { get; set; }
        public UserDto User { get; set; }
        public ProductDto Product { get; set; }
    }
}