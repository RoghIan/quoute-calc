using System.Net.Mail;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            return users;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Post(UserDto user)
        {
            var existingUser = await UserExists(user);

            if (existingUser != null)
            {
                existingUser.LoanAmount = user.LoanAmount;
                _context.Entry(existingUser).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return $"http://localhost:4200/{existingUser.Id}";
            }

            var newUSer = new AppUser
            {
                Title = user.Title,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                LoanAmount = user.LoanAmount
            };

            await _context.Users.AddAsync(newUSer);
            var result = await _context.SaveChangesAsync();

            if (result != 0)
                return $"http://localhost:4200/{newUSer.Id}";

            return BadRequest();
        }

        [HttpPost("calculate-quote")]
        public async Task<ActionResult> GetQuote(QuoteRequestDto quoteRequest)
        {
            var selectedProduct = await _context.Products.FindAsync(quoteRequest.ProductId);

            if (selectedProduct == null) return BadRequest("Selected Product Not Found!");

            var monthlyInterestRate = selectedProduct.AnnualInterestRate / 12;

            var monthlyNoInterest = quoteRequest.LoanAmount / quoteRequest.NumberOfRepayments;
            var monthlyWithInterest = Financial.Pmt(monthlyInterestRate, quoteRequest.NumberOfRepayments, quoteRequest.LoanAmount) * -1;
            var interestValuePerMonth = monthlyWithInterest - monthlyNoInterest;
            var totalCost = monthlyWithInterest * quoteRequest.NumberOfRepayments;
            var totalInterestValue = totalCost - quoteRequest.LoanAmount;

            var user = await _context.Users.FindAsync(quoteRequest.UserId);
            user.LoanAmount = quoteRequest.LoanAmount;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new QuoteResultDto()
            {
                MonthlyRepaymentNoInterest = monthlyNoInterest,
                MonthlyRepaymentWithInterest = monthlyWithInterest,
                InterestValuePerMonth = interestValuePerMonth,
                TotalCost = totalCost,
                TotalInterest = totalInterestValue,
                User = new UserDto()
                {
                    Id = user.Id,
                    Title = user.Title,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateOfBirth = user.DateOfBirth,
                    Email = user.Email,
                    LoanAmount = user.LoanAmount
                }
            });
        }

        [HttpPost("apply-now")]
        public async Task<ActionResult> ApplyNow(ApplyNowDto userLoan)
        {
            var user = await _context.Users.FindAsync(userLoan.UserId);

            if (user.GetAge() < 18) return BadRequest("Sorry, you should be at least 18 years old to apply for a loan");

            var isPhoneBlocked = await _context.BlacklistedMobileNumbers.AnyAsync(x => x.MobileNumber == user.MobileNumber);
            var isEmailBlocked = await _context.BlacklistedEmailDomains.AnyAsync(x => x.EmailDomainName == new MailAddress(user.Email).Host);

            if (isEmailBlocked || isPhoneBlocked) return BadRequest("Sorry, your application has be Denied. User is Blocked");

            var product = await _context.Products.FindAsync(userLoan.ProductId);

            var newUserLoan = new UserLoan()
            {
                NumberOfRepayments = userLoan.NumberOfRepayments,
                LoanAmount = userLoan.LoanAmount,
                MonthlyRepaymentAmount = userLoan.MonthlyRepaymentAmount,
                TotalInterest = userLoan.TotalInterest,
                TotalCost = userLoan.TotalCost,
                AppUser = user,
                Product = product
            };

            await _context.UserLoans.AddAsync(newUserLoan);
            return Ok(newUserLoan);
        }

        private async Task<AppUser> UserExists(UserDto user)
        {
            return await _context.Users
            .FirstOrDefaultAsync(x =>
            x.FirstName.ToLower() == user.FirstName.ToLower() &&
            x.LastName.ToLower() == user.LastName.ToLower() &&
            x.DateOfBirth == user.DateOfBirth);
        }
    }
}