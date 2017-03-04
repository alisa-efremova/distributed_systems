namespace PoemMessage
{
    public interface ConsonantsCalculated
    {
        string CorrId { get; }
        string Text { get; }
        int[] VowelCounts { get; }
        int[] ConsonantCount { get; }
    }
}
