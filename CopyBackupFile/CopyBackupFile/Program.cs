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
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private static NameValueCollection appSettings = ConfigurationManager.AppSettings;

        static void Main(string[] args)
        {
            WriteLog("Starting proccess to copy the file from the original server...");
            TryCopyFile(appSettings.Get("serverSourcePath"), appSettings.Get("serverTargetPath"), appSettings.Get("serverUser"), appSettings.Get("serverDomain"), appSettings.Get("serverPassword")).Wait();

            WriteLog("Starting proccess to copy the file to backup server...");
            TryCopyFile(appSettings.Get("backupServerSourcePath"), appSettings.Get("backupServerTargetPath"), appSettings.Get("backupServerUser"), appSettings.Get("backupServerDomain"), appSettings.Get("backupServerPassword")).Wait();

            WriteLog("Deleting the zip file created in the source server...");
            DeleteZipFile(appSettings.Get("serverSourcePath"), appSettings.Get("serverUser"), appSettings.Get("serverDomain"), appSettings.Get("serverPassword")).Wait();
        }

        private static async Task TryCopyFile(string source, string target, string user, string domain, string password)
        {
            try
            {
                WriteLog("Trying to access the directory " + source + " with credentials...");
                using (new Helpers.Impersonator(user, domain, password))
                {
                    var dbFolder = new DirectoryInfo(source);

                    if (!dbFolder.Exists)
                        dbFolder.Create();

                    var lastBackUp = dbFolder.GetFiles("*.bak*").OrderByDescending(_ => _.LastWriteTime).FirstOrDefault();
                    WriteLog("Founded the new file " + lastBackUp.FullName);

                    if (lastBackUp == null)
                        return;

                    var fileName = "";

                    if (lastBackUp.Extension.ToLowerInvariant() != ".zip")
                    {
                        WriteLog("Iniciating compress process on file...");
                        fileName = lastBackUp.FullName + ".zip";
                        using (ZipArchive archive = ZipFile.Open(fileName, ZipArchiveMode.Create))
                        {
                            WriteLog("Compressing file...");
                            archive.CreateEntryFromFile(lastBackUp.FullName, lastBackUp.Name, CompressionLevel.Optimal);
                        }
                        WriteLog("Compressed with sucess");
                    }
                    else
                    {
                        fileName = lastBackUp.FullName;
                    }
                    WriteLog("Trying to open file...");
                    using (FileStream SourceStream = File.Open(fileName, FileMode.Open))
                    {
                        var name = fileName.Substring(fileName.LastIndexOf(@"\"));
                        WriteLog("Creating destination file..");
                        using (FileStream DestinationStream = File.Create(target + name))
                        {
                            WriteLog("Copyng file...");
                            await SourceStream.CopyToAsync(DestinationStream);
                        }
                    }
                    
                }
                WriteLog("Copyied with sucess!");
            }
            catch (Exception e)
            {
                WriteLog(e,"The file wasn't copy");
            }
        }
        private static async Task DeleteZipFile(string path, string user, string domain, string password)
        {
            try
            {
                WriteLog("Trying to access the directory " + path + " with credentials...");
                using (new Helpers.Impersonator(user, domain, password))
                {
                    var dbFolder = new DirectoryInfo(path);

                    if (!dbFolder.Exists)
                        dbFolder.Create();

                    var lastBackUp = dbFolder.GetFiles("*.zip").OrderByDescending(_ => _.LastWriteTime).FirstOrDefault();
                    WriteLog("Founded the temp zip file " + lastBackUp.FullName);
                    if (lastBackUp == null)
                        return;

                    WriteLog("Deleting file...");
                    await Task.Run(() => File.Delete(lastBackUp.FullName));
                }
                WriteLog("Deleted with sucess...");
            }
            catch (Exception e)
            {
                WriteLog(e, "Error on delete");
            }
        }
        private static void WriteLog (string message)
        {
            Log.Info(message);
        }
        private static void WriteLog(Exception e, string message)
        {
            Log.Fatal(e, message);
        }
    }
}
