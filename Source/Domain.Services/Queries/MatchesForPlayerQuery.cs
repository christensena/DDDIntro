namespace DDDIntro.Domain.Services.Queries
{
    public class MatchesForPlayerQuery : IQuery<Match[]>
    {
        public int PlayerID { get; private set; }

        public MatchesForPlayerQuery(int playerID)
        {
            PlayerID = playerID;
        }
    }
}