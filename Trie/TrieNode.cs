using System.Collections.Generic;

namespace Trie
{
    public class TrieNode
    {
        public string Text { get; private set; }
        public List<TrieNode> Children { get; private set; }
        public int Occurence { get; private set; }

        public TrieNode()
        {
            Children = new List<TrieNode>();
        }
    }
}
