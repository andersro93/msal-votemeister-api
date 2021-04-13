namespace MsalDemo.Backend.Authorization
{
    public static class Scopes
    {
        public const string VotesRead = "votes.read";
        public const string VotesCast = "votes.cast";
        public const string CandidatesRead = "candidates.read";
        public const string ResultsRead = "results.read";
        
        internal static string FullScopeName(string scope) => $"api://votemeister-2021-api/{scope}";
    }
}