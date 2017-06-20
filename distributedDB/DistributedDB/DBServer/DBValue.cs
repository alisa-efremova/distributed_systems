namespace DBServer
{
    public class DBValue
    {
        public string Value { get; set; }
        public int Version { get; set; }
        public int ServerId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
