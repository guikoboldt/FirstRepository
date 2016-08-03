using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyBackupFile
{
    public class CopyFile
    {
        private static NLog.Logger Log { get; set; }
        private static NameValueCollection appSettings = ConfigurationManager.AppSettings;

        static void Main(string[] args)
        {
            Log = NLog.LogManager.GetCurrentClassLogger();
            //var credentials = ConfigurationManager.AppSettings;
            //Task.Factory.StartNew(async () =>
            //{
            //loking for file
            Log.Info("Starting proccess to copy the file from server.... - {0}", DateTime.Now);
            TryCopyFile(appSettings.Get("sourcePath"), appSettings.Get("targetPath")).Wait();

            //copy it to the end destination
            Log.Info("Starting proccess to copy the file to backup server.... - {0}", DateTime.Now);
            TryCopyFile(appSettings.Get("targetPath"), appSettings.Get("sourcePath") + @"\return").Wait();

            Log.Info("Deleting the zip file create in the source server.... - {0}", DateTime.Now);
            DeleteZipFile(appSettings.Get("sourcePath")).Wait();
            //}).Wait();
        }

        private static async Task TryCopyFile(string source, string target)
        {
            try
            {
                Log.Info("Trying to access the directory {0} with credentials.... - {1}", source, DateTime.Now);
                using (new Helpers.Impersonator(appSettings.Get("serverUser"), appSettings.Get("serverDomain"), appSettings.Get("serverPassword")))
                {
                    var dbFolder = new DirectoryInfo(source);

                    if (!dbFolder.Exists)
                        dbFolder.Create();

                    var lastBackUp = dbFolder.GetFiles("*.bak*").OrderByDescending(_ => _.LastWriteTime).FirstOrDefault();
                    Log.Info("Founded the latest file {0} - {1}", lastBackUp.FullName, DateTime.Now);

                    if (lastBackUp == null)
                        return;

                    var fileName = "";

                    if (lastBackUp.Extension.ToLowerInvariant() != ".zip")
                    {
                        Log.Info("Iniciating compress process on file... - {0}", DateTime.Now);
                        fileName = lastBackUp.FullName + ".zip";
                        using (ZipArchive archive = ZipFile.Open(fileName, ZipArchiveMode.Create))
                        {
                            archive.CreateEntryFromFile(lastBackUp.FullName, lastBackUp.Name);
                        }
                        Log.Info("Compressed sucess - {0}", DateTime.Now);
                    }
                    else
                    {
                        fileName = lastBackUp.FullName;
                    }
                    Log.Info("Trying to open file...- {0}", DateTime.Now);
                    using (FileStream SourceStream = File.Open(fileName, FileMode.Open))
                    {
                        var name = fileName.Substring(fileName.LastIndexOf(@"\"));
                        Log.Info("Creating destination file... - {0}", DateTime.Now);
                        using (FileStream DestinationStream = File.Create(target + name))
                        {
                            await SourceStream.CopyToAsync(DestinationStream);
                        }
                    }
                    Log.Info("File was copy with sucess - {0}", DateTime.Now);
                }
            }
            catch (Exception e)
            {
                Log.Fatal("The file wasn't copy \n error: {0} \n {1}", e, DateTime.Now);
            }
        }

        private static async Task DeleteZipFile(string path)
        {
            try
            {
                Log.Info("Trying to access the directory {0} with credentials.... - {1}", path, DateTime.Now);
                using (new Helpers.Impersonator(appSettings.Get("serverUser"), appSettings.Get("serverDomain"), appSettings.Get("serverPassword")))
                {
                    var dbFolder = new DirectoryInfo(path);

                    if (!dbFolder.Exists)
                        dbFolder.Create();

                    var lastBackUp = dbFolder.GetFiles("*.zip").OrderByDescending(_ => _.LastWriteTime).FirstOrDefault();
                    Log.Info("Founded the temp zip file {0} - {1}", lastBackUp.FullName, DateTime.Now);
                    if (lastBackUp == null)
                        return;

                    Log.Info("Trying to delete... - {0}", DateTime.Now);
                    await Task.Run(() => File.Delete(lastBackUp.FullName));
                    Log.Info("Deleted with sucess... - {0}", DateTime.Now);
                }
            }
            catch (Exception e)
            {
                Log.Fatal("Error on delete \n error: {0} \n {1}", e, DateTime.Now);
            }
        }
    }
}
