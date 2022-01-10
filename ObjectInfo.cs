namespace LibZipSharpTest
{
    public class ObjectInfo
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public short Version { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<ObjectTag> Tags { get; set; }

        public Guid OwnerId { get; set; }

        public int BlockCount { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastChanged { get; set; }

        public bool IsEditable { get; set; }

        public short MaxPlayerCount { get; set; }

        public bool IsPrivate { get; set; }

        public string OwnerName { get; set; }
    }
}
