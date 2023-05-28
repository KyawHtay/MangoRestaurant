using Mango.Services.Email.DbContexts;
using Mango.Services.Email.Models;
using Mango.Services.OrderAPI.Messages;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.Email.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContext;
        public EmailRepository(DbContextOptions<ApplicationDbContext>  dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SendAndLogEmail(UpdatePaymentResultMessage message)
        {
            //implement an email sender or call some other class lib
            EmailLog emailLog = new EmailLog()
            {
                Email = message.Email,
                EmailSent = DateTime.Now,
                Log = $"Order - {message.OrderId} has been created successfully."
            };
            await using var _db = new ApplicationDbContext(_dbContext);
            _db.Add(emailLog);
            await _db.SaveChangesAsync();
        }
    }
}

