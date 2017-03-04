namespace PoemMessage
{
    public interface VowelsCalculated
    {
        string CorrId { get; }
        string Text { get; }
        int[] VowelCounts { get; }
    }
}