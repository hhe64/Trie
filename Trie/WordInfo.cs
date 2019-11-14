namespace TrieLib
{
    public class WordInfo
    {
        public string Word { get; set; }
        public long StreamPosition { get; set; }
        public long CharPosition { get; set; }
        public override string ToString() { return $"{Word} [{StreamPosition}]"; }
    }
}