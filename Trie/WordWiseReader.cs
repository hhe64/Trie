using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TrieLib
{

    internal class Enumerator : IEnumerator<WordInfo>
    {
        private class CharInfo
        {
            public char c;
            public int bytesRead;
            public CharInfo()
            {
                c = '\0';
                bytesRead = 0;
            }
        }

        private long _streamPosition;
        private Stream _stream;
        private Encoding _encoding;
        private bool _endOfFile;
        private long _charCount;

        public Enumerator(Stream stream, Encoding encoding)
        {
            _stream = stream;
            _encoding = encoding;
            _charCount = 0;
            _stream.Seek(0, SeekOrigin.Begin);

            // wegen verschachtelten for-Schleifen muss jeder Enumerator seine eigene stream-Position verwalten
            _streamPosition = _stream.Position;
            _endOfFile = false;
        }

        public WordInfo Current { get; set; }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (_endOfFile) return false;

            var sbword = new StringBuilder();

            if (_stream.Position != _streamPosition)
                _stream.Seek(_streamPosition, SeekOrigin.Begin);

            long wordBytePosition = 0;
            // Füllzeichen überspringen:
            CharInfo ci;
            while ((ci = ReadNextChar(_stream)).bytesRead > 0)
            {
                _charCount++;
                if (!IsWordChar(ci.c)) continue;

                wordBytePosition = _stream.Position - ci.bytesRead;
                sbword.Append(ci.c);
                break;
            }
            if (ci.bytesRead == 0)
            {
                _endOfFile = true;
                return false;
            }

            // Wort bilden:
            while ((ci = ReadNextChar(_stream)).bytesRead > 0)
            {
                _charCount++;
                if (IsWordChar(ci.c))
                {
                    sbword.Append(ci.c);
                    continue;
                }
                break;
            }
            if (ci.bytesRead < 1)
            {
                _endOfFile = true;
            }

            _streamPosition = _stream.Position;

            Current = new WordInfo() { 
                Word = sbword.ToString(), 
                Position = new WordPosition() { 
                    BytePos = wordBytePosition, 
                    CharPos = _charCount - 1 
                } 
            };
            return true;
        }

        private CharInfo ReadNextChar(Stream stream)
        {
            // erstmal nur für UTF8-encoding 
            if (_encoding != Encoding.UTF8)
            {
                throw new Exception("Only UTF8-encoding implemented.");
            }
            byte[] buffer = new byte[4];

            int bytescount;
            bytescount = stream.Read(buffer, 0, 1);
            if (bytescount < 1) return new CharInfo() { c = '\0', bytesRead = 0 };
            if ((buffer[0] & (1 << 7)) != 0)
            {
                int followBytes = 1;
                // if ((buffer[0] & (1 << 6)) != 0) followBytes++;
                if ((buffer[0] & (1 << 5)) != 0) followBytes++;
                if ((buffer[0] & (1 << 4)) != 0) followBytes++;

                for (var i = 0; i < followBytes; i++)
                {
                    bytescount = stream.Read(buffer, i + 1, 1);
                    if (bytescount < 1) return new CharInfo() { c = '\0', bytesRead = 0 };
                }
                return new CharInfo() { c = _encoding.GetString(buffer, 0, followBytes + 1)[0], bytesRead = followBytes + 1 };
            }
            else
            {
                return new CharInfo() { c = _encoding.GetString(buffer, 0, 1)[0], bytesRead = 1 };
            }
        }

        private bool IsWordChar(char c)
        {
            return char.IsLetterOrDigit(c) || c == '_';
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }

    public class WordWiseReader : IEnumerable<WordInfo>
    {
        private Stream _stream;
        private Encoding _encoding;

        public WordWiseReader(Stream stream)
        {
            this._stream = stream;
            this._encoding = DetectEncoding(stream);
        }

        private Encoding DetectEncoding(Stream stream)
        {
            // vorerst:
            return Encoding.UTF8;
        }

        public IEnumerator<WordInfo> GetEnumerator()
        {
            return new Enumerator(_stream, _encoding);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}