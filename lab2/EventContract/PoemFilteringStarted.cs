namespace PoemBeautifierContract
{
    public interface PoemFilteringStarted
    {
        string UserId { get; }
        string CorrId { get; }
        string[] Poem { get; }
    }
}
