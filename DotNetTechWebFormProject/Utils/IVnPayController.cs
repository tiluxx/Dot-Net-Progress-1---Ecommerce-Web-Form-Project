using DotNetTechWebFormProject.Model;

namespace DotNetTechWebFormProject.Utils
{
    public interface IVnPayController
    {
        string CreatePaymentUrl(Payment model, HttpContext context);
        PaymentResponse PaymentExecute(IQueryCollection collections);
    }
}
