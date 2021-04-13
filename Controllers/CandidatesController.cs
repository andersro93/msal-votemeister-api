using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MsalDemo.Backend.Authorization;
using MsalDemo.Backend.Services;

namespace MsalDemo.Backend.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/candidates")]
    public class CandidatesController : ControllerBase
    {
        private readonly CandidateService _candidateService;

        public CandidatesController(CandidateService candidateService)
        {
            _candidateService = candidateService;
        }

        [HttpGet]
        [RequiredScope(Scopes.CandidatesRead)]
        public IActionResult GetCandidates()
        {
            return Ok(_candidateService.GetCandidates());
        }
    }
}