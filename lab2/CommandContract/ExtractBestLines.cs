namespace CommandContract
{
    public interface ExtractBestLines
    {
        string CorrId { get; }
        string[] Text { get; }
        int[] VowelCounts { get; }
        int[] ConsonantCount { get; }
    }
}
