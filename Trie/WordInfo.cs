namespace TrieLib
{
    public class WordInfo
    {
        public string Word { get; set; }
        public WordPosition Position { get; set; }
        public override string ToString() { return $"{Word} [{Position.BytePos}/{Position.CharPos}]"; }
    }
}