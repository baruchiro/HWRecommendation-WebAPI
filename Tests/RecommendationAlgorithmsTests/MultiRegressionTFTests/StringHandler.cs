using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.ML.Data;

namespace MultiRegressionTFTests
{
    class StringHandler : IFileHandle
    {
        private readonly List<Stream> _streamReaders;
        private readonly Stream _streamWriter;
        private readonly string _data;

        public StringHandler(string data)
        {
            _data = data;
            _streamReaders = new List<Stream>();
            _streamWriter = data.ToStream();
        }

        public void Dispose()
        {
            _streamWriter.Close();
            _streamReaders.ForEach(s=>s.Close());
        }

        public Stream CreateWriteStream()
        {
            return _streamWriter;
        }

        public Stream OpenReadStream()
        {
            var reader = _data.ToStream();
            _streamReaders.Add(reader);
            return reader;
        }

        public bool CanWrite => _streamWriter.CanWrite;
        public bool CanRead => true;


    }

    internal static class extension
    {
        public static Stream ToStream(this string str)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(str);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
