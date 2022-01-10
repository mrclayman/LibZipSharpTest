namespace LibZipSharpTest
{
    public class ObjectTag
    {
        public string Value { get; set; }

        public string Origin { get; set; }

        public ObjectTag(string value, string origin)
        {
            Value = value;
            Origin = origin;
        }
    }
}
