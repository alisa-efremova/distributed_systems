namespace EventContract
{
    public interface PoemFilteringCompleted
    {
        string CorrId { get; }
        string[] Poem { get; }
    }
}
