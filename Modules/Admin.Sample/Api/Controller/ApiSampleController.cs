

using Microsoft.AspNetCore.Mvc;
using NLog;
using Steam.Core.Utilities.STeamHelper;
using X.PagedList;
using Admin.Sample.Api.Models.Request;
using Steam.Core.Base.Models;
namespace Admin.Sample.Api.Controllers
{
    [ApiController, Route("/api/[controller]")]
    public class ApiSampleController : Controller
    {
        private readonly ILogger _logger;
        private readonly IMailHelper _mailHelper;

        public ApiSampleController()
        {

        }
        [HttpGet]
        public IActionResult Get()
        {
            // Simulate retrieving data
            var data = new { Id = 1, Name = "John Doe", Age = 30 };

            // Return 200 OK with data
            return Ok(data);
        }

        // POST api/sample
        [HttpPost]
        public IActionResult Post([FromBody] SampleModel model)
        {
            if (!ModelState.IsValid)
            {
                // Return 400 Bad Request with validation errors
                return BadRequest(ModelState);
            }

            // Process the model (e.g., save to database)

            // Return 201 Created with the created resource
            return CreatedAtAction(nameof(Get), new { id = model.Id }, model);
        }

        // GET api/sample/1
        [HttpGet("{id}")]
        public async Task<JsonResult> GetById(int id)
        {
            Response<int> rs = new Response<int>();
            var data = GetDataById(id);

            if (data == null)
            {
                return rs.NotFound();
            }

            // Return 200 OK with data
            return rs.Ok();
        }

        // Simulated data retrieval method
        private object GetDataById(int id)
        {
            // Implement data retrieval logic here
            return null; // Return null to simulate data not found
        }

        // POST api/sample/authenticate
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] LoginModel model)
        {
            // Simulate authentication logic
            if (model.Username == "admin" && model.Password == "password")
            {
                // Return 200 OK with a success message or token
                return Ok(new { Message = "Authenticated successfully" });
            }

            // Return 401 Unauthorized if authentication fails
            return Unauthorized();
        }

    }

}
