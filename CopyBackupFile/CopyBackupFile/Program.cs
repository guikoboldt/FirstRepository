using NLog;
using System;
using System.Collections.Generic;
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
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static string[] credentials = new string[] { ConfigurationManager.AppSettings["serverUser"], ConfigurationManager.AppSettings["serverDomain"], ConfigurationManager.AppSettings["serverPassword"] };

        static void Main(string[] args)
        {
            //Task.Factory.StartNew(async () =>
            //{
            //loking for file
            logger.Info("Starting proccess to copy the file from server.... - {0}", DateTime.Now);
            TryCopyFile(ConfigurationManager.AppSettings["sourcePath"], ConfigurationManager.AppSettings["targetPath"]).Wait();

            //copy it to the end destination
            logger.Info("Starting proccess to copy the file to backup server.... - {0}", DateTime.Now);
            TryCopyFile(ConfigurationManager.AppSettings["targetPath"], ConfigurationManager.AppSettings["sourcePath"] + @"\return").Wait();

            logger.Info("Deleting the zip file create in the source server.... - {0}", DateTime.Now);
            DeleteZipFile(@"C:\TESTE").Wait();
            //}).Wait();

        }

        private static async Task TryCopyFile(string source, string target)
        {
            try
            {
                logger.Info("Tring to access the directory {0} with credentials.... - {1}", source, DateTime.Now);
                using (new Helpers.Impersonator(credentials[0], credentials[1], credentials[2]))
                {
                    var dbFolder = new DirectoryInfo(source);

                    if (!dbFolder.Exists)
                        dbFolder.Create();

                    var lastBackUp = dbFolder.GetFiles("*.bak*").OrderByDescending(_ => _.LastWriteTime).FirstOrDefault();
                    logger.Info("Founded the latest file {0} - {1}", lastBackUp.FullName, DateTime.Now);

                    if (lastBackUp == null)
                        return;

                    var fileName = "";

                    if (lastBackUp.Extension.ToLowerInvariant() != ".zip")
                    {
                        logger.Info("Iniciating compress process on file... - {0}", DateTime.Now);
                        fileName = lastBackUp.FullName + ".zip";
                        using (ZipArchive archive = ZipFile.Open(fileName, ZipArchiveMode.Create))
                        {
                            archive.CreateEntryFromFile(lastBackUp.FullName, lastBackUp.Name);
                        }
                        logger.Info("Compressed sucess - {0}", DateTime.Now);
                    }
                    else
                    {
                        fileName = lastBackUp.FullName;
                    }
                    logger.Info("Tring to open file...- {0}", DateTime.Now);
                    using (FileStream SourceStream = File.Open(fileName, FileMode.Open))
                    {
                        var name = fileName.Substring(fileName.LastIndexOf(@"\"));
                        logger.Info("Creating destination file... - {0}", DateTime.Now);
                        using (FileStream DestinationStream = File.Create(target + name))
                        {
                            await SourceStream.CopyToAsync(DestinationStream);
                        }
                    }
                    logger.Info("File was copy with sucess - {0}", DateTime.Now);
                }
            }
            catch (Exception e)
            {
                logger.Fatal("The file wasn't copy \n error: {0} \n {1}", e, DateTime.Now);
            }

        }

        private static async Task DeleteZipFile(string path)
        {
            try
            {
                logger.Info("Tring to access the directory {0} with credentials.... - {1}", path, DateTime.Now);
                using (new Helpers.Impersonator(credentials[0], credentials[1], credentials[2]))
                {
                    var dbFolder = new DirectoryInfo(path);

                    if (!dbFolder.Exists)
                        dbFolder.Create();

                    var lastBackUp = dbFolder.GetFiles("*.zip").OrderByDescending(_ => _.LastWriteTime).FirstOrDefault();
                    logger.Info("Founded the temp zip file {0} - {1}", lastBackUp.FullName, DateTime.Now);
                    if (lastBackUp == null)
                        return;

                    logger.Info("Trying to delete... - {0}", DateTime.Now);
                    await Task.Run(() => File.Delete(lastBackUp.FullName));
                    logger.Info("Deleted with sucess... - {0}", DateTime.Now);
                }
            }
            catch (Exception e)
            {
                logger.Fatal("Error on delete \n error: {0} \n {1}", e, DateTime.Now);
            }
        }
    }
}
