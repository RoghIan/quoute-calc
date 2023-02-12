using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ProductDto
    {
        public string ProductName { get; set; }
        public double AnnualInterestRate { get; set; }
        public bool Is2MonthsInterestFree { get; set; }
    }
}