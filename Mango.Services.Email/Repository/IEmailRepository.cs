using Mango.Services.OrderAPI.Messages;

namespace Mango.Services.Email.Repository
{
    public interface IEmailRepository
    {
       
        Task SendAndLogEmail(UpdatePaymentResultMessage message);
    }
}
