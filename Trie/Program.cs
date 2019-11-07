using System;
using System.IO;

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
                foreach (var word in wordwise)
                {
                    Console.WriteLine(word.ToString());

                    // Test verschachtelte Enumeratoren
                    //var wordwise2 = new WordWiseReader(stream);
                    //foreach (var word2 in wordwise2)
                    //{
                    //    Console.WriteLine("    "+word2.ToString());
                    //}
                }
            }
        }
    }
}
