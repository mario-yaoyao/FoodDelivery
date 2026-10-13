namespace FoodDelivery.BLL.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toName, string toEmail, string subject, string textContent);
    }
}
