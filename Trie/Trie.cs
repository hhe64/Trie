using System;
using System.Collections.Generic;
using System.Text;

namespace Trie
{
    public class Trie
    {
        private TrieNode _root;

        public Trie()
        {
            _root = new TrieNode("");
        }
        public TrieNode Root { get => _root; }

        public void Insert(WordInfo wordInfo)
        {
            Insert(new StringBuilder(wordInfo.Word.ToLower()), wordInfo.StreamPosition, Root);
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

        private void Insert(StringBuilder sbword, long streamPosition, TrieNode parent)
        {
            if (parent.Children == null) parent.Children = new List<TrieNode>();

            var found = parent.Children.Find(f => GetMatch(f.Text, sbword) > 0);
            if (found == null)
            {
                parent.Children.Add(new TrieNode(sbword.ToString()) { Locations = new List<long>() { streamPosition } });
                return;
            }

            var matchCount = GetMatch(found.Text, sbword);
            if (matchCount == found.Text.Length)
            {
                if (matchCount == sbword.Length)
                {
                    if (found.Locations == null) found.Locations = new List<long>();
                    found.Locations.Add(streamPosition);
                }
                else
                {
                    sbword.Remove(0, matchCount);
                    Insert(sbword, streamPosition, found);
                }
            }
            else if (matchCount == sbword.Length)
            {
                parent.Children.Remove(found);

                var n = new TrieNode(sbword.ToString()) { Locations = new List<long>() { streamPosition }, Children = new List<TrieNode>() };
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
                    new TrieNode(sbword.Remove(0, matchCount).ToString()) { Locations = new List<long>() { streamPosition } },
                };
            }
        }

        public Dictionary<string, List<long>> GetAllWords()
        {
            var allWords = new Dictionary<string, List<long>>();

            var traverser = new TrieTraverser(this);

            traverser.Traverse((n, s) =>
            {
                if ((n.Locations?.Count ?? 0) > 0)
                {
                    allWords[s] = n.Locations;
                }
            });
            return allWords;
        }

        private List<long> Find(StringBuilder sbword, TrieNode parent)
        {
            if (parent.Children == null) return null;

            var found = parent.Children.Find(f => GetMatch(f.Text, sbword) > 0);
            if (found == null) return null;

            int match = GetMatch(found.Text, sbword);

            if (match == sbword.Length && match == found.Text.Length) return found.Locations;

            return Find(sbword.Remove(0, match), found);
        }

        public List<long> Find(string word)
        {
            var sbword = new StringBuilder(word.ToLower());

            return Find(sbword, this.Root);
        }
    }
}
