namespace ATaraxiaApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StripeController : ControllerBase
{
    private readonly IStripeAppService _stripeService;

    public StripeController(IStripeAppService stripeService)
    {
        _stripeService = stripeService;
    }

    [HttpPost("customer/add")]
    public async Task<ActionResult<StripeCustomer>> AddStripeCustomer(
        [FromBody] AddStripeCustomer customer,
        CancellationToken cancellationToken)
    {
        StripeCustomer createdCustomer = await _stripeService.AddStripeCustomerAsync(
            customer,cancellationToken);
       
        return Ok(createdCustomer);
    }

    [HttpPost("payment/add")]
    public async Task<ActionResult<StripePayment>> AddStripePayment(
        [FromBody] AddStripePayment payment,
        CancellationToken cancellationToken)
    {
        StripePayment createdPayment = await _stripeService.AddStripePaymentAsync(
            payment,
            cancellationToken);

        return Ok( createdPayment);
    }
}

