namespace EventContract
{
    public interface PoemFilteringCompleted
    {
        string UserId { get; }
        string CorrId { get; }
        string[] Poem { get; }
    }
}
