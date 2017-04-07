namespace PoemFilterContract
{
    public interface ExtractBestLines
    {
        string UserId { get; }
        string CorrId { get; }
        string[] Text { get; }
        int[] VowelCounts { get; }
        int[] ConsonantCount { get; }
    }
}
