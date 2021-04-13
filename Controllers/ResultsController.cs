using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MsalDemo.Backend.Authorization;
using MsalDemo.Backend.Services;

namespace MsalDemo.Backend.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/results")]
    public class ResultsController : ControllerBase
    {
        private readonly VotesService _votesService;

        public ResultsController(VotesService votesService)
        {
            _votesService = votesService;
        }

        [HttpGet]
        [RequiredScope(Scopes.ResultsRead)]
        public IActionResult GetResults()
        {
            return Ok(_votesService.GetResults());
        }
    }
}