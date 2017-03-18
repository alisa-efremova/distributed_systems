namespace EventContract
{
    public interface PoemFilteringStarted
    {
        string CorrId { get; }
        string[] Poem { get; }
    }
}
