using System;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace InRamStreamHelper
{
    public static class InRamStreamUtil
    {
        public static IFileProvider StringToFileProvider(string source)
        {
            return new InRamFileProvider(source);
        }

        public static Stream StringToStream(string str, bool withBom = false)
        {
            var memStream = new MemoryStream();
            var textWriter = new StreamWriter(memStream, new System.Text.UTF8Encoding(withBom));
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

        public class InRamFileProvider : IFileProvider
        {
            private readonly string _data;

            public InRamFileProvider(string source)
            {
                _data = source;
            }

            public IFileInfo GetFileInfo(string subpath)
            {
                return new InRamFile(_data);
            }


            public IDirectoryContents GetDirectoryContents(string subpath)
            {
                throw new NotImplementedException();
            }

            public IChangeToken Watch(string filter)
            {
                throw new NotImplementedException();
            }
        }

        public class InRamFile : IFileInfo
        {
            private readonly string _data;

            public InRamFile(string data)
            {
                _data = data;
            }

            public Stream CreateReadStream()
            {
                return StringToStream(_data);
            }

            public bool Exists => true;
            public long Length => _data.Length;
            public string PhysicalPath => throw new NotImplementedException();
            public string Name { get; } = "In Ram File";
            public DateTimeOffset LastModified => throw new NotImplementedException();
            public bool IsDirectory => true;
        }
    }
}
