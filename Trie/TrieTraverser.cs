using System;

namespace Trie
{
    public class TrieTraverser
    {
        private Trie _trie;

        public Trie Trie
        {
            get { return _trie; }
            // set { _trie = value; }
        }
        public TrieTraverser(Trie trie)
        {
            _trie = trie;
        }

        public void Traverse(Action<TrieNode, string> traverseAction )
        {
            Traverse(Trie.Root, "", traverseAction);
        }
        private void Traverse(TrieNode node, string concatenated, Action<TrieNode, string> traverseAction)
        {
            string currentText = concatenated + node.Text;
            traverseAction(node, currentText);
            if (node.Children == null) return;
            foreach(var n in node.Children)
            {
                Traverse(n, currentText,  traverseAction);
            }
        }
    }
}
