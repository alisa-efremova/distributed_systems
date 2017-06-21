namespace PoemBeautifierContract
{
    public interface ProceedOriginalPoem
    {
        string UserId { get; }
        string CorrId { get; }
        string[] Poem { get; }
    }
}
