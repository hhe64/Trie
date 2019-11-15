using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace TrieLib
{
    class Program
    {
        static void Main(string[] args)
        {
            // string textFile = @"Dateien\LoreIpsum.txt";
            // string textFile = @"Dateien\LoreIpsum.de.txt";
            // string textFile = @"Dateien\KurzerText.txt";
            string textFile = @"Dateien\VSCodeAufraeumarbeiten.txt";

            if (!File.Exists(textFile))
            {
                Console.WriteLine($"Datei {textFile} wurde nicht gefunden.");
                return;
            }
            using (var stream = File.Open(textFile, FileMode.Open))
            {
                var wordwiseReader = new WordWiseReader(stream);
                // Print(wordwise);
                // ReadWordsFromStream(stream, wordwise);

                Trie trie = TrieFromWords(wordwiseReader);
                var allWords = trie.GetAllWords();

                var wordPositions = trie.FindWord("Aktualisierungen");
                wordPositions = trie.FindWord("die");

                List<WordInfo> sortedByPosition = new List<WordInfo>();
                foreach(var w in allWords)
                {
                    w.Value.ForEach(x => sortedByPosition.Add(new WordInfo() { Word = w.Key, Position = x }));
                }
                sortedByPosition.Sort((a, b) => a.Position.CharPos.CompareTo(b.Position.CharPos));

                sortedByPosition.ForEach(x => Console.Write($"{x.Word} "));
            }
        }

        private static Trie TrieFromWords(IEnumerable<WordInfo> words)
        {
            var trie = new Trie();
            foreach(var wordInfo in words)
            {
                trie.InsertWord(wordInfo);
            }
            return trie;
        }

        private static void ShowWordSurroundings(FileStream stream, WordWiseReader wordwise)
        {
            foreach(var word in wordwise)
            {
                stream.Seek(word.Position.BytePos, SeekOrigin.Begin);
                byte[] buffer = new byte[100];
                stream.Read(buffer, 0, 20);
                var s = Encoding.UTF8.GetString(buffer,0,20);
                foreach (char nl in new char[]{'\r', '\n' })   {
                    if (s.LastIndexOf(nl) < 0) continue;
                    s = s.Replace(nl, ' ');
                }
                Console.WriteLine(s);
            }
        }

        private static void PrintAllWords(WordWiseReader wordwise)
        {
            foreach(var word in wordwise)
            {
                Console.WriteLine(word.ToString());
            }
        }
    }
}
