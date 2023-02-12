using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class UserQuoteDto
    {
        public UserDto User { get; set; }
        public QuoteRequestDto QuoteRequest { get; set; }
    }
}