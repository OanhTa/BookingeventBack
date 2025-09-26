using bookingEvent.DTO;
using bookingEvent.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CheckoutController : ControllerBase
{
    private readonly ICheckoutService _checkoutService;

    public CheckoutController(ICheckoutService checkoutService)
    {
        _checkoutService = checkoutService;
    }

    // POST: api/checkout
    [HttpPost]
    public async Task<IActionResult> CreateCheckout([FromBody] CheckoutDto dto)
    {
        if (!ModelState.IsValid)  
            return BadRequest(ModelState);

        var checkout = await _checkoutService.CreateCheckoutAsync(dto);

        return Ok(new
        {
            Message = "Thanh toán thành công",
            CheckoutId = checkout.Id,
            checkout
        });
    }

    // GET: api/checkout/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCheckout(int id)
    {
        var checkout = await _checkoutService.GetCheckoutByIdAsync(id);
        if (checkout == null) return NotFound();

        return Ok(checkout);
    }
}
