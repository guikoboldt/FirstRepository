using System;
using System.Threading;
using System.Threading.Tasks;

namespace MemoryMapped
{
    public interface IReadOnlyMemoryMappingFile : IMemoryMappingFileBase, IReadable { }
    public interface IMemoryMappingFile : IReadOnlyMemoryMappingFile, IWritable, ICreatable { }
    public interface IPhysicalMemoryMappingFile : IMemoryMappingFile
    {
        static public IPhysicalMemoryMappingFile CreateMapping(string fileName)
        {
            var file = new PhysicalMemoryMappedFile();
            file.Create(fileName);
            return file;
        }
    }
    public interface IInMemoryMappingFile : IMemoryMappingFile
    {
        static public IInMemoryMappingFile CreateMapping(string fileName)
        {
            var file = new InMemoryMappedFile();
            file.Create(fileName);
            return file;
        }
    }

    public interface IMemoryMappingFileBase
    {
        Guid ID { get; }
        string FileName { get; }
    }

    public interface IReadable
    {
        bool Read();
    }

    public interface IWritable
    {
        Task Write(bool content, CancellationToken ct = default);
    }

    public interface ICreatable
    {
        void Create(string fileName);
    }
}
