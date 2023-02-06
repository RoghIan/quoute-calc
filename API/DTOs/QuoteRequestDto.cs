using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class QuoteRequestDto
    {
        public int UserId { get; set; }
        public int LoanAmount { get; set; }
        public int NumberOfRepayments { get; set; }
        public int ProductId { get; set; }
    }
}