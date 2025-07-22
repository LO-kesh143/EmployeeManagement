using EmployeeManagement.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TitleApiController : ControllerBase
    {
        private readonly ITitleService _titleService;
        public TitleApiController(ITitleService titleService) => _titleService = titleService;

        [HttpGet("list")]
        public async Task<IActionResult> GetTitles()
        {
            var data = await _titleService.GetTitleSalaryStatsAsync();
            return Ok(data);
        }
    }

}
