using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class BlacklistedEmailDomain
    {
        public int Id { get; set; }
        public string EmailDomainName { get; set; }
    }
}