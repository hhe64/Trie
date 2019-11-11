using System.Collections.Generic;

namespace Trie
{
    public class TrieNode
    {
        private string _text;

        bool IsRoot => _text == "";

        public string Text
        {
            get { return _text; }
            set { 
                _text = value; 
            }
        }
        private List<TrieNode> _children;

        public List<TrieNode> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        private List<long> _locations;

        public List<long> Locations
        {
            get { return _locations; }
            set { _locations = value; }
        }

        public TrieNode(string text)
        {
            Text = text;
        }
    }
}
