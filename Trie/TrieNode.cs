using System.Collections.Generic;

namespace TrieLib
{
    public class TrieNode
    {
        private string _text;

        public bool IsRoot => _text == "";

        public bool IsLeaf => BytePositions != null && BytePositions.Count > 0;

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

        private List<long> _bytePositions;

        public List<long> BytePositions
        {
            get { return _bytePositions; }
            set { _bytePositions = value; }
        }

        public TrieNode(string text)
        {
            Text = text;
        }
    }
}
