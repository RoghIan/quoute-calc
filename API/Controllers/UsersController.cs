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

                return $"http://localhost:4200/create-request/{existingUser.Id}";
            }

            var newUSer = new AppUser
            {
                Title = user.Title,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                LoanAmount = user.LoanAmount
            };

            await _context.Users.AddAsync(newUSer);
            var result = await _context.SaveChangesAsync();

            if (result != 0)
                return $"http://localhost:4200/create-request/{newUSer.Id}";

            return BadRequest();
        }

        [HttpPost("calculate-quote")]
        public async Task<ActionResult> GetQuote(UserQuoteDto userQuote)
        {
            var product = await _context.Products.FindAsync(userQuote.QuoteRequest.ProductId);

            if (product == null) return BadRequest("Selected Product Not Found!");

            var user = await _context.Users.FindAsync(userQuote.QuoteRequest.UserId);

            if (user == null) return NotFound("User Not Found!");


            if (
                user.FirstName != userQuote.User.FirstName ||
                user.LastName != userQuote.User.LastName ||
                user.DateOfBirth != userQuote.User.DateOfBirth ||
                user.Email != userQuote.User.Email ||
                user.MobileNumber != userQuote.User.MobileNumber)
            {
                user.FirstName = userQuote.User.FirstName;
                user.LastName = userQuote.User.LastName;
                user.DateOfBirth = userQuote.User.DateOfBirth;
                user.Email = userQuote.User.Email;
                user.MobileNumber = userQuote.User.MobileNumber;
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            var quote = await CalculateQuoteAsync(product, userQuote.QuoteRequest, user);

            return Ok(quote);
        }

        [HttpPost("apply-now")]
        public async Task<ActionResult> ApplyNow(QuoteRequestDto userLoan)
        {
            var user = await _context.Users.FindAsync(userLoan.UserId);

            if (user == null) return NotFound("User Not Found!");

            var age = user.GetAge();

            if (user.GetAge() < 18) return BadRequest("Sorry, you should be at least 18 years old to apply for a loan");

            var isPhoneBlocked = await _context.BlacklistedMobileNumbers.AnyAsync(x => x.MobileNumber == user.MobileNumber);
            var isEmailBlocked = await _context.BlacklistedEmailDomains
                .AnyAsync(x => x.EmailDomainName.Trim().ToLower() == new MailAddress(user.Email).Host.Trim().ToLower());

            if (isEmailBlocked || isPhoneBlocked) return BadRequest("Sorry, your application has be Denied. User is Blocked");

            var product = await _context.Products.FindAsync(userLoan.ProductId);

            if (product == null) return NotFound("Selected Product Not Found!");

            var quote = await CalculateQuoteAsync(product, userLoan, user);

            var newUserLoan = new UserLoan()
            {
                NumberOfRepayments = userLoan.NumberOfRepayments,
                LoanAmount = userLoan.LoanAmount,
                MonthlyRepaymentAmount = quote.MonthlyRepaymentWithInterest,
                TotalInterest = quote.TotalInterest,
                TotalCost = quote.TotalCost,
                AppUser = user,
                Product = product
            };

            await _context.UserLoans.AddAsync(newUserLoan);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private async Task<AppUser> UserExists(UserDto user)
        {
            return await _context.Users
            .FirstOrDefaultAsync(x =>
            x.FirstName.ToLower() == user.FirstName.ToLower() &&
            x.LastName.ToLower() == user.LastName.ToLower() &&
            x.DateOfBirth == user.DateOfBirth);
        }

        private async Task<QuoteResultDto> CalculateQuoteAsync(Product product, QuoteRequestDto quoteRequest, AppUser user)
        {
            var monthlyInterestRate = product.AnnualInterestRate / 12;

            var monthlyNoInterest = quoteRequest.LoanAmount / quoteRequest.NumberOfRepayments;
            var monthlyWithInterest = Financial.Pmt(monthlyInterestRate, quoteRequest.NumberOfRepayments, quoteRequest.LoanAmount) * -1;
            var interestValuePerMonth = monthlyWithInterest - monthlyNoInterest;
            var totalCost = monthlyWithInterest * quoteRequest.NumberOfRepayments;
            if (product.Is2MonthsInterestFree)
            {
                totalCost -= (interestValuePerMonth * 2);
            }
            var totalInterestValue = totalCost - quoteRequest.LoanAmount;

            if (user.LoanAmount != quoteRequest.LoanAmount ||
            user.NumberOfRepayments != quoteRequest.NumberOfRepayments ||
             user.ProductId != quoteRequest.ProductId)
            {
                user.LoanAmount = quoteRequest.LoanAmount;
                user.NumberOfRepayments = quoteRequest.NumberOfRepayments;
                user.ProductId = quoteRequest.ProductId;
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            return new QuoteResultDto()
            {
                NumberOfRepayments = quoteRequest.NumberOfRepayments,
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
                    MobileNumber = user.MobileNumber,
                    Email = user.Email,
                    LoanAmount = user.LoanAmount,
                    ProductId = user.ProductId,
                    NumberOfRepayments = user.NumberOfRepayments
                },
                Product = new ProductDto()
                {
                    ProductName = product.ProductName,
                    AnnualInterestRate = product.AnnualInterestRate,
                    Is2MonthsInterestFree = product.Is2MonthsInterestFree
                }
            };
        }
    }
}