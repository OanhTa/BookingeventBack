using bookingEvent.Infrastructure.Middlewares;
using bookingEvent.Model;
using bookingEvent.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bookingEvent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private CategoryServices _categoryServices;

        public CategoryController(CategoryServices categoryService)
        {
            _categoryServices = categoryService;
        }

        [HttpGet]
        [Authorize]
        //[Permission("Identity.Category.Read")]
        public async Task<ActionResult<IEnumerable<Category>>> GetAll()
        {
            var categories = await _categoryServices.GetAllAsync();
            return Ok(categories);
        }
    }
}
