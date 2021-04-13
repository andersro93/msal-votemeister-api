using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using MsalDemo.Backend.Authorization;
using MsalDemo.Backend.Models;
using MsalDemo.Backend.Services;

namespace MsalDemo.Backend.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/votes")]
    public class VotesController : ControllerBase
    {
        private readonly VotesService _votesService;
        private readonly CandidateService _candidateService;

        public VotesController(VotesService votesService, CandidateService candidateService)
        {
            _votesService = votesService;
            _candidateService = candidateService;
        }

        [HttpGet]
        [RequiredScope(Scopes.VotesRead)]
        public IActionResult GetVotes()
        {
            var objectId = User.Claims.First(claim => claim.Type == ClaimConstants.ObjectId).Value;
            var tenantId = User.Claims.First(claim => claim.Type == ClaimConstants.TenantId).Value;

            if (_votesService.HasVoted(tenantId, objectId) is false)
            {
                return BadRequest("You must vote before you can see the votes");
            }
            
            return Ok(_votesService.GetVotes().Select(vote => new {vote.Candidate, vote.CreatedAt}));
        }

        [HttpPost]
        [RequiredScope(Scopes.VotesCast)]
        public IActionResult CastVote(Vote vote)
        {
            vote.ObjectId = User.Claims.First(claim => claim.Type == ClaimConstants.ObjectId).Value;
            vote.TenantId = User.Claims.First(claim => claim.Type == ClaimConstants.TenantId).Value;
            vote.CreatedAt = DateTimeOffset.Now;

            if (_votesService.HasVoted(vote.TenantId, vote.ObjectId))
            {
                return BadRequest("You have already casted a vote");
            }

            if (_candidateService.IsACandidate(vote.Candidate) is false)
            {
                return BadRequest("Not a valid candidate");
            }

            _votesService.AddVote(vote);

            return Ok();
        }
    }
}