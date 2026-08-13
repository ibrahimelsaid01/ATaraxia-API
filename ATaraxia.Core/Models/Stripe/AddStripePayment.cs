namespace ATaraxia.Core.Models.Stripe;

public record AddStripePayment(
     string CustomerId,
     string ReceiptEmail,
     string Description,
     string Currency,
     long Amount);
