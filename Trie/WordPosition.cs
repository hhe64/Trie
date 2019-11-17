namespace TrieLib
{
    public class WordPosition
    {
        public long BytePos { get; set; }
        public long CharPos { get; set; }
        public override string ToString()
        {
            return $"{CharPos}/{BytePos}";
        }
    }
}
