using System.Collections.Generic;

namespace TrieLib
{
    public class TrieNode
    {
        private string _text;

        public bool IsRoot => _text == "";

        public bool IsLeaf => WordPositions != null && WordPositions.Count > 0;

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

        private List<WordPosition> _wordPosition;

        public List<WordPosition> WordPositions
        {
            get { return _wordPosition; }
            set { _wordPosition = value; }
        }

        public TrieNode(string text)
        {
            Text = text;
        }
    }
}
