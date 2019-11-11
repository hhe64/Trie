namespace Trie
{
    public class WordInfo
    {
        public string Word { get; set; }
        public long StreamPosition { get; set; }
        public override string ToString() { return $"{Word} [{StreamPosition}]"; }
    }
}