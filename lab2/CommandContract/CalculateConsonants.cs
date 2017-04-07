namespace PoemFilterContract
{
    public interface CalculateConsonants
    {
        string UserId { get; }
        string CorrId { get; }
        string[] Text { get; }
        int[] VowelCounts { get; }
    }
}
