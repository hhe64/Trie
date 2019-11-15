using System;
using System.Collections.Generic;
using System.Text;

namespace TrieLib
{
    public class Trie
    {
        private TrieNode _root;

        public Trie()
        {
            _root = new TrieNode("");
        }
        public TrieNode Root { get => _root; }

        public void InsertWord(WordInfo wordInfo)
        {
            InsertWord(new StringBuilder(wordInfo.Word.ToLower()), wordInfo.Position, Root);
        }

        private int GetMatch(string word, StringBuilder sbword)
        {
            int maxCount = Math.Min(word.Length, sbword.Length);
            for (int i = 0; i < maxCount; i++)
            {
                if (word[i] != sbword[i]) return i;
            }
            return maxCount;
        }

        private void InsertWord(StringBuilder sbword, WordPosition position,  TrieNode parent)
        {
            if (parent.Children == null) parent.Children = new List<TrieNode>();

            var found = parent.Children.Find(f => GetMatch(f.Text, sbword) > 0);
            if (found == null)
            {
                parent.Children.Add(new TrieNode(sbword.ToString()) { WordPositions = new List<WordPosition>() { position } });
                return;
            }

            var matchCount = GetMatch(found.Text, sbword);
            if (matchCount == found.Text.Length)
            {
                if (matchCount == sbword.Length)
                {
                    if (found.WordPositions == null) found.WordPositions = new List<WordPosition>();
                    found.WordPositions.Add(position);
                }
                else
                {
                    sbword.Remove(0, matchCount);
                    InsertWord(sbword, position, found);
                }
            }
            else if (matchCount == sbword.Length)
            {
                parent.Children.Remove(found);

                var n = new TrieNode(sbword.ToString()) { WordPositions = new List<WordPosition>() { position }, Children = new List<TrieNode>() };
                parent.Children.Add(n);

                parent.Children.Remove(found);
                found.Text = found.Text.Substring(matchCount);
                n.Children.Add(found);
            }
            else
            {
                parent.Children.Remove(found);

                var newparent = new TrieNode(found.Text.Substring(0, matchCount)) { Children = new List<TrieNode>() };
                parent.Children.Add(newparent);

                found.Text = found.Text.Substring(matchCount);
                newparent.Children = new List<TrieNode>()
                {
                    found,
                    new TrieNode(sbword.Remove(0, matchCount).ToString()) { WordPositions = new List<WordPosition>() { position } },
                };
            }
        }

        public Dictionary<string, List<WordPosition>> GetAllWords()
        {
            var allWords = new Dictionary<string, List<WordPosition>>();

            var traverser = new TrieTraverser(this);

            traverser.Traverse((n, s) =>
            {
                if (n.IsLeaf)
                {
                    allWords[s] = n.WordPositions;
                }
            });
            return allWords;
        }

        private List<WordPosition> FindWord(StringBuilder sbword, TrieNode parent)
        {
            if (parent.Children == null) return null;

            var found = parent.Children.Find(f => GetMatch(f.Text, sbword) > 0);
            if (found == null) return null;

            int match = GetMatch(found.Text, sbword);

            if (match == sbword.Length && match == found.Text.Length) return found.WordPositions;

            return FindWord(sbword.Remove(0, match), found);
        }

        public List<WordPosition> FindWord(string word)
        {
            var sbword = new StringBuilder(word.ToLower());

            return FindWord(sbword, this.Root);
        }
    }
}
