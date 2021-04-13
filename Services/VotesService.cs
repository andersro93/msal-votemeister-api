using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MsalDemo.Backend.Models;

namespace MsalDemo.Backend.Services
{
    public class VotesService
    {
        private readonly ConcurrentBag<Vote> _votes = new();

        public IEnumerable<Vote> GetVotes() => _votes.ToArray();
        public void AddVote(Vote vote) => _votes.Add(vote);

        public bool HasVoted(string tenantId, string objectId) => _votes
            .Where(vote => vote.ObjectId == objectId)
            .Where(vote => vote.TenantId == tenantId)
            .FirstOrDefault() != null;

        public IEnumerable<VoteResult> GetResults() => _votes
                .GroupBy(v => v.Candidate)
                .Select(vg => new VoteResult(vg.Key, vg.Count()))
                .OrderByDescending(vr => vr.Votes);
    }
}