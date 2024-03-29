﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using TrieLib;

namespace TrieWpf
{
    public class TrieTreeItem
    {
        public string FullText { get; set; }
        public string Text { get => IsRoot?"@ROOT":_trieNode.Text; }
        public List<WordPosition> WordPositions { get => _trieNode.WordPositions; }

        public bool IsRoot => _trieNode.IsRoot;
        public bool IsLeaf => _trieNode.IsLeaf;

        private ObservableCollection<TrieTreeItem> _children;

        public ObservableCollection<TrieTreeItem> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        private TrieNode _trieNode;

        public TrieTreeItem(TrieNode trieNode, string fullText)
        {
            _trieNode = trieNode;
            FullText = fullText;
        }

        public string NameToShow {
            get
            {
                string nameToShow = $"{Text}";
                if (IsLeaf)
                {
                    nameToShow = nameToShow + $" [{FullText}]";
                }
                return nameToShow;
            }
        }
        public Brush TreeItemForeground
        {
            get
            {
                return new SolidColorBrush()
                {
                    Color = IsLeaf ? Color.FromRgb(255, 0, 0) : Color.FromRgb(0, 0, 0),
                };
            }
        }
    }
}
