namespace PoemBeautifierContract
{
    public interface ProceedBeautifiedPoem
    {
        string UserId { get; }
        string CorrId { get; }
        string[] Poem { get; }
    }
}
