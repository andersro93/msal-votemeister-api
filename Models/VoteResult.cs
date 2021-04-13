namespace MsalDemo.Backend.Models
{
    public class VoteResult
    {
        public string CandidateName { get; set; }
        public int Votes { get; set; }

        public VoteResult(string candidateName, int votes)
        {
            CandidateName = candidateName;
            Votes = votes;
        }
    }
}