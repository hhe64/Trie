using System.Collections.Generic;

namespace Trie
{
    public class TrieNode
    {
        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
        private List<TrieNode> _children;

        public List<TrieNode> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        private List<long> _occurences;

        public List<long> Occurences
        {
            get { return _occurences; }
            set { _occurences = value; }
        }

        public TrieNode(string text)
        {

            
        }
    }
}
