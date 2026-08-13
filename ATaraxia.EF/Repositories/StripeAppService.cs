namespace ATaraxia.EF.Repositories;

public class StripeAppService : IStripeAppService
{
    private readonly ChargeService _chargeService;
    private readonly CustomerService _customerService;
    private readonly TokenService _tokenService;

    public StripeAppService(
        ChargeService chargeService,
        CustomerService customerService,
        TokenService tokenService)
    {
        _chargeService = chargeService;
        _customerService = customerService;
        _tokenService = tokenService;
    }
    // Create a new customer at Stripe through API using customer and card details from records.
    public async Task<StripeCustomer> AddStripeCustomerAsync(AddStripeCustomer customer, CancellationToken cancellationToken)
    {
        // Set Stripe Token options based on customer data
        TokenCreateOptions tokenOptions = new TokenCreateOptions
        {
            Card = new TokenCardOptions
            {
                Name = customer.Name,
                Number = customer.CreditCard.CardNumber,
                ExpYear = customer.CreditCard.ExpirationYear,
                ExpMonth = customer.CreditCard.ExpirationMonth,
                Cvc = customer.CreditCard.Cvc
            }
        };
        // Create new Stripe Token
        Token stripeToken = await _tokenService.CreateAsync(tokenOptions, null, cancellationToken);
        // Set Customer options using
        CustomerCreateOptions customerOptions = new CustomerCreateOptions
        {
            Name = customer.Name,
            Email = customer.Email,
            Source = stripeToken.Id
        };
        // Create customer at Stripe
        Customer createdCustomer = await _customerService.CreateAsync(customerOptions, null, cancellationToken);
        // Return the created customer at stripe
        return new StripeCustomer(createdCustomer.Name, createdCustomer.Email, createdCustomer.Id);
    }
    // Add a new payment at Stripe using Customer and Payment details.
    public async Task<StripePayment> AddStripePaymentAsync(AddStripePayment payment, CancellationToken cancellationToken)
    {
        // Set the options for the payment we would like to create at Stripe
        ChargeCreateOptions paymentOptions = new ChargeCreateOptions
        {
            Customer = payment.CustomerId,
            ReceiptEmail = payment.ReceiptEmail,
            Description = payment.Description,
            Currency = payment.Currency,
            Amount = payment.Amount
        };

        // Create the payment
        var createdPayment = await _chargeService.CreateAsync(paymentOptions, null, cancellationToken);

        // Return the payment to requesting method
        return new StripePayment(
          createdPayment.CustomerId,
          createdPayment.ReceiptEmail,
          createdPayment.Description,
          createdPayment.Currency,
          createdPayment.Amount,
          createdPayment.Id);
    }
}
