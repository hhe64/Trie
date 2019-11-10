using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace Trie
{
    class Program
    {
        static void Main(string[] args)
        {
            // string textFile = @"Dateien\LoreIpsum.txt";
            string textFile = @"Dateien\LoreIpsum.de.txt";


            if (!File.Exists(textFile))
            {
                Console.WriteLine($"Datei {textFile} wurde nicht gefunden.");
                return;
            }
            using (var stream = File.Open(textFile, FileMode.Open))
            {
                var wordwise = new WordWiseReader(stream);
                // Print(wordwise);
                // ReadWordsFromStream(stream, wordwise);

                Trie trie = TrieFromWords(wordwise);
            }
        }

        private static Trie TrieFromWords(IEnumerable<WordInfo> wordwise)
        {
            var trie = new Trie();
            foreach(var word in wordwise)
            {
                trie.Insert(word);
            }
        }

        private static void ReadWordsFromStream(FileStream stream, WordWiseReader wordwise)
        {
            foreach(var word in wordwise)
            {
                stream.Seek(word.StreamPosition, SeekOrigin.Begin);
                byte[] buffer = new byte[100];
                stream.Read(buffer, 0, 20);
                var s = Encoding.UTF8.GetString(buffer,0,20);
                foreach (char nl in new char[]{'\r', '\n' })   {
                    if (s.LastIndexOf(nl) < 0) break;
                    s = s.Replace(nl, ' ');
                }
                Console.WriteLine(s);
            }
        }

        private static void Print(WordWiseReader wordwise)
        {
            foreach(var word in wordwise)
            {
                Console.WriteLine(word.ToString());
            }
        }
    }
}
