using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyBackupFile
{
    public class CopyFile
    {
        static void Main(string[] args)
        {
            //Task.Factory.StartNew(async () =>
            //{
            //loking for file
            TryCopyFile(@"C:\TESTE", @"\\poasms002\Segep - Correios\teste", "poasup02", "andritz.com", "MeMpPo734DP").Wait();

            //copy it to the end destination
            TryCopyFile(@"\\poasms002\Segep - Correios\teste", @"C:\TESTE\return", "poasup02", "andritz.com", "MeMpPo734DP").Wait();

            DeleteZipFile(@"C:\TESTE", "poasup02", "andritz.com", "MeMpPo734DP").Wait();
            //}).Wait();

        }

        private static async Task TryCopyFile(string source, string target, string user, string domain, string password)
        {
            try
            {
                using (new Helpers.Impersonator(user, domain, password))
                {
                    var dbFolder = new DirectoryInfo(source);

                    if (!dbFolder.Exists)
                        dbFolder.Create();

                    var lastBackUp = dbFolder.GetFiles("*.bak*").OrderByDescending(_ => _.LastWriteTime).FirstOrDefault();

                    if (lastBackUp == null)
                        return;

                    var fileName = "";

                    if (lastBackUp.Extension.ToLowerInvariant() != ".zip")
                    {
                        fileName = lastBackUp.FullName + ".zip";
                        using (ZipArchive archive = ZipFile.Open(fileName, ZipArchiveMode.Create))
                        {
                            archive.CreateEntryFromFile(lastBackUp.FullName, lastBackUp.Name);
                        }
                    }
                    else
                    {
                        fileName = lastBackUp.FullName;
                    }

                    using (FileStream SourceStream = File.Open(fileName, FileMode.Open))
                    {
                        var name = fileName.Substring(fileName.LastIndexOf(@"\"));
                        using (FileStream DestinationStream = File.Create(target + name))
                        {
                            await SourceStream.CopyToAsync(DestinationStream);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error on copy");
                using (var sw = new StreamWriter("log.txt", true))
                {
                    sw.WriteLine(e);
                }
            }

        }

        private static async Task DeleteZipFile(string path, string user, string domain, string password)
        {
            try
            {
                using (new Helpers.Impersonator(user, domain, password))
                {
                    var dbFolder = new DirectoryInfo(path);

                    if (!dbFolder.Exists)
                        dbFolder.Create();

                    var lastBackUp = dbFolder.GetFiles("*.zip").OrderByDescending(_ => _.LastWriteTime).FirstOrDefault();

                    if (lastBackUp == null)
                        return;

                    await Task.Run(() => File.Delete(lastBackUp.FullName));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error on copy");
                using (var sw = new StreamWriter("log.txt", true))
                {
                    sw.WriteLine(e);
                }
            }
        }
    }
}
