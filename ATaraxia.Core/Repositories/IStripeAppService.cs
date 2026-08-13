namespace ATaraxia.Core.Repositories;

public interface IStripeAppService
{
    Task<StripeCustomer> AddStripeCustomerAsync(AddStripeCustomer customer, CancellationToken ct);
    Task<StripePayment> AddStripePaymentAsync(AddStripePayment payment, CancellationToken ct);
}
