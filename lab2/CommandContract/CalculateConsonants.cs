namespace CommandContract
{
    public interface CalculateConsonants
    {
        string CorrId { get; }
        string[] Text { get; }
        int[] VowelCounts { get; }
    }
}
