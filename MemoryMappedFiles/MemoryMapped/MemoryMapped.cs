using MemoryMapped.Helpers;
using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MemoryMapped
{
    public abstract class MemoryMappedFile : IMemoryMappingFile
    {
        protected string _mutexSufix = "_mutex";
        public Guid ID { get; private set; } = Guid.NewGuid();
        public string FileName { get; protected set; }

        abstract public void Create(string fileName);

        abstract public bool Read();

        abstract public Task Write(bool content, CancellationToken ct = default);
    }

    public class PhysicalMemoryMappedFile : MemoryMappedFile, IPhysicalMemoryMappingFile
    {
        public string FilePath { get; private set;  }

        private string _fileFullPath => Path.Combine(Directory.GetCurrentDirectory(), FileHandler.FilesDirectory, this.FileName);

        override public void Create(string fileName)
        {
            FileHandler.ValidateAndCreate(fileName);
            this.FileName = fileName;
        }

        override public bool Read()
        {
            var mutex = MutexHandler.Retrive(this.FileName + this._mutexSufix);
            try
            {
                using var existentMapped = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(this.FilePath, FileMode.Open, this.FileName, 100);
                using var viewStream = existentMapped.CreateViewStream();
                var binaryReader = new BinaryReader(viewStream);

                using var memoryStream = new MemoryStream();
                var buffer = new byte[100];
                int count;

                while ((count = binaryReader.Read(buffer, 0, buffer.Length)) != 0)
                    memoryStream.Write(buffer, 0, count);

                var value = Serializer.Deserialize(memoryStream.ToArray());
                return value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[READING ERROR] - {ex.Message}");
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return default;
        }

        override async public Task Write(bool content, CancellationToken ct = default)
        {
            var mutex = MutexHandler.Retrive(this.FileName + this._mutexSufix);

            var buffer = Serializer.Serialize(content);

            try
            {
                using var memoryMapped = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(_fileFullPath, FileMode.OpenOrCreate, this.FileName, 100);
                using var viewStream = memoryMapped.CreateViewStream();
                await viewStream.WriteAsync(buffer, 0, buffer.Length, ct).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WRITING ERROR] - {ex.Message}");
            }

            mutex.ReleaseMutex();
        }

        static public IReadOnlyMemoryMappingFile FromExistent(string fileName, string fileFullPath)
        {
            return new PhysicalMemoryMappedFile
            {
                FileName = fileName,
                FilePath = fileFullPath
            };
        }
    }

    public class InMemoryMappedFile : MemoryMappedFile, IInMemoryMappingFile
    {
        private System.IO.MemoryMappedFiles.MemoryMappedFile _memoryMappedFile;

        public override void Create(string fileName)
        {
            this._memoryMappedFile = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateNew(fileName, 1000);
            this.FileName = fileName;
        }

        public override bool Read()
        {
            var receiverMutex = MutexHandler.Retrive(this.FileName + this._mutexSufix);
            try
            {
                //using var mmf = System.IO.MemoryMappedFiles.MemoryMappedFile.OpenExisting(this.FileName);
                using var stream = this._memoryMappedFile.CreateViewStream();
                var reader = new BinaryReader(stream);
                var newFlag = reader.ReadBoolean();

                Console.WriteLine($"New flag from {this.FileName}: {newFlag} {DateTime.Now.TimeOfDay}");

                return newFlag;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR READING: {ex.Message}");
                return false;
            }
            finally
            {
                receiverMutex.ReleaseMutex();
            }
        }

        async public override Task Write(bool content, CancellationToken ct = default)
        {
            var mutex = MutexHandler.Retrive(this.FileName + this._mutexSufix);
            try
            {
                //using var mmf = MemoryMappedFile.OpenExisting(fileToNotifyAlives);
                using var stream = this._memoryMappedFile.CreateViewStream();
                var writer = new BinaryWriter(stream);
                writer.Write(content);

                await Task.CompletedTask.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR WRITING: {ex.Message}");
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        static public IInMemoryMappingFile CreateFromExistent(string fileName)
        {
            var inMemoryFile = new InMemoryMappedFile
            {
                FileName = fileName,
                _memoryMappedFile = System.IO.MemoryMappedFiles.MemoryMappedFile.OpenExisting(fileName)
            };

            return inMemoryFile;
        }
    }
}
