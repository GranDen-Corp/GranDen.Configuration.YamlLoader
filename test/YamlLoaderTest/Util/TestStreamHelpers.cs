﻿using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace YamlLoaderTest.Util
{
    public static class TestStreamHelpers
    {
        public static readonly string ArbitraryFilePath = "Unit tests do not touch file system";

        public static IFileProvider StringToFileProvider(string str)
        {
            return new TestFileProvider(str);
        }

        private class TestFile : IFileInfo
        {
            private readonly string _data;

            public TestFile(string str)
            {
                _data = str;
            }

            public bool Exists => true;

            public bool IsDirectory => false;

            public DateTimeOffset LastModified => throw new NotImplementedException();

            public long Length => throw new NotImplementedException();

            public string Name => throw new NotImplementedException();

            public string PhysicalPath => throw new NotImplementedException();

            public Stream CreateReadStream()
            {
                return StringToStream(_data);
            }
        }

        private class TestFileProvider : IFileProvider
        {
            private readonly string _data;
            public TestFileProvider(string str)
            {
                _data = str;
            }

            public IDirectoryContents GetDirectoryContents(string subpath)
            {
                throw new NotImplementedException();
            }

            public IFileInfo GetFileInfo(string subpath)
            {
                return new TestFile(_data);
            }

            public IChangeToken Watch(string filter)
            {
                throw new NotImplementedException();
            }
        }

        public static Stream StringToStream(string str, bool withBom = false)
        {
            var memStream = new MemoryStream();
            var textWriter = new StreamWriter(memStream, new UTF8Encoding(withBom));
            textWriter.Write(str);
            textWriter.Flush();
            memStream.Seek(0, SeekOrigin.Begin);

            return memStream;
        }

        public static string StreamToString(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }
    }
}