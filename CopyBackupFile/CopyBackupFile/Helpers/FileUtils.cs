using Polly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CopyBackupFile.Helpers
{
    public class FileUtils
    {
        public static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        public static Policy Policy = Policy.Handle<Exception>()
                                        .WaitAndRetryAsync(5, RetryAttemp =>
                                        (TimeSpan.FromMinutes(5 * RetryAttemp)),
                                        onRetry: (e, retryCount) =>
                                        {
                                            WriteLog(e, string.Format("Exception {0} - " +
                                                "Attemp: {1}", e.Message, retryCount));
                                        });
        public static async Task TryCopyFile(Entities.File file, string target, string user,
            string domain, string password)
        {
            await Policy.ExecuteAsync(async () =>
                {
                    var testToOpen = File.OpenRead(file.FullPath());
                    testToOpen.Close();
                    WriteLog(string.Format("File name: {0} Location: {1} Destination: {2}", file.Name,
                        file.Location, target));

                    var fileName = default(string);
                    WriteLog("Iniciating compress process on file...");
                    fileName = Regex.Replace((file.FullPath() + ".zip"), ".bak", "");

                    using (var archive = new Ionic.Zip.ZipFile(fileName))
                    {
                        WriteLog("Compressing file...");
                        archive.UseZip64WhenSaving = Ionic.Zip.Zip64Option.AsNecessary;
                        archive.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                        archive.ParallelDeflateThreshold = -1;
                        archive.AddFile(file.FullPath());
                        archive.Save();
                        WriteLog("Compressed with sucess");
                    }

                    WriteLog("Trying to open compressed file...");

                    var name = fileName.Substring(fileName.LastIndexOf(@"\"));
                    using (FileStream SourceStream = File.Open(fileName, FileMode.Open))
                    {
                        WriteLog(string.Format("Creating destination file...with credentials {0} ",
                            user));
                        using (new Helpers.Impersonator(user, domain, password))
                        {
                            using (FileStream DestinationStream = File.Create(target + name))
                            {
                                WriteLog("Copyng file...");
                                await SourceStream.CopyToAsync(DestinationStream);
                                WriteLog("Copyied with sucess!");
                            }
                        }
                    }
                    await DeleteZipFile(new Entities.File(name, file.Location));
                    Helpers.FileUtils.WriteLog(string.Format("Waiting for new backup file on {0}", file.Location));
                });

        }
        public static async Task DeleteZipFile(Entities.File file)
        {
            await Policy.ExecuteAsync(async () =>
            {
                await Task.Run(() =>
                {
                    WriteLog(string.Format("Trying to access the directory {0} ...", file.Location));
                    WriteLog("Founded the temp zip file " + file.FullPath());
                    WriteLog("Deleting file...");
                    File.Delete(file.FullPath());
                    WriteLog("Deleted with sucess...");
                });
            });
        }

        async static public Task CheckFolderDestination(string destinationServerTargetPath,
            string user, string domain, string password)
        {
            WriteLog("Cleaning target folder " + destinationServerTargetPath);
            using (new Impersonator(user, domain, password))
            {
                var directory = new DirectoryInfo(destinationServerTargetPath);
                if (!directory.Exists)
                {
                    WriteLog("Creating directory folder...");
                    directory.Create();
                    WriteLog("directory created!");
                }
                else
                {
                    await Task.Run(() =>
                    {
                        WriteLog("Checking target old files...");
                        var files = directory.GetFiles();
                        if (files != null)
                        {
                            foreach (var file in files)
                            {
                                if (file.Extension.Equals(".zip"))
                                {
                                    if ((DateTime.Now - file.CreationTime) > TimeSpan.FromDays(5))
                                    {
                                        file.Delete();
                                        WriteLog("Deleting file " + file.FullName);
                                    }
                                }
                            }
                        }
                        WriteLog("Target path ready!");
                    });
                }

            }
        }

        public static void WriteLog(string message)
        {
            Log.Info(message);
        }
        public static void WriteLog(Exception e, string message)
        {
            Log.Error(e, message);
        }
    }
}
