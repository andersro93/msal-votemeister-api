using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MsalDemo.Backend.Models;

namespace MsalDemo.Backend.Services
{
    public class CandidateService
    {
        private readonly ConcurrentBag<Candidate> _candidates = new()
        {
            new Candidate() { Name = "Windows", Description = "An operating system created by Microsoft" },
            new Candidate() { Name = "Mac", Description = "An operating system created by Apple" },
            new Candidate() { Name = "Linux", Description = "A set of multiple distros based on the Linux kernel" },
        };

        public IEnumerable<Candidate> GetCandidates() => _candidates.ToArray();

        public bool IsACandidate(string candidateName) => _candidates
            .Count(c => c.Name == candidateName) == 1;
    }
}