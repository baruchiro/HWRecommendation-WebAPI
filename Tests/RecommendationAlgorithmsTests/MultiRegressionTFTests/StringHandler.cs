using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.ML.Data;

namespace MultiRegressionTFTests
{
    class StringHandler : IFileHandle
    {
        private readonly Stream _streamReader;
        private readonly Stream _streamWriter;

        public StringHandler(string data)
        {
            _streamReader = data.ToStream();
            _streamWriter = data.ToStream();
        }

        public void Dispose()
        {
            _streamWriter.Close();
            _streamReader.Close();
        }

        public Stream CreateWriteStream()
        {
            return _streamWriter;
        }

        public Stream OpenReadStream()
        {
            return _streamReader;
        }

        public bool CanWrite => _streamWriter.CanWrite;
        public bool CanRead => _streamReader.CanRead;


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
