using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringDirectory.Entities
{
    public class FTPFileManager : Interfaces.IFileProvider
    {
        private FtpWebRequest FtpConnection { get; set; }
        public string FtpURL { get; set; }
        public FTPFileManager(string ftpURL, string user, string password, int port = 21)
        {
            //if (!ftpURL.EndsWith("/"))
            //    ftpURL = ftpURL + "/";
            FtpConnection = WebRequest.Create(ftpURL + @"test\") as FtpWebRequest;
            FtpConnection.Credentials = new NetworkCredential(user, password);
            FtpConnection.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            ICollection<File> listContent = new List<File>();

            using (var FtpContent = FtpConnection.GetResponse())
            {
                var response = FtpContent.GetResponseStream();
                using (var content = new StreamReader(response))
                {
                    listContent.Add(new File(content.ReadLine(), ftpURL));
                }
            }
            Files = listContent;
        }

        public ICollection<File> Files { get; set; }

        //public ICollection<string> ListContent(string folder = "")
        //{
        //    FtpConnection = WebRequest.Create(this.FtpURL + folder + @"\") as FtpWebRequest;
        //    FtpConnection.Credentials = new NetworkCredential(this.User, this.Password);
        //    ICollection<string> listContent = new List<string>();
        //    if (string.IsNullOrEmpty(folder))
        //    {
        //        FtpConnection.Method = WebRequestMethods.Ftp.ListDirectory;
        //    }
        //    else
        //    {
        //        FtpConnection.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
        //    }

        //    using (var FtpContent = FtpConnection.GetResponse())
        //    {
        //        var response = FtpContent.GetResponseStream();
        //        using (var content = new StreamReader(response))
        //        {
        //            listContent.Add(content.ReadToEnd());
        //        }
        //    }


        //    return listContent;
        //}

        public void CopyFileTo(File file, string destination)
        {
            FtpConnection = WebRequest.Create(FtpURL + file.Name) as FtpWebRequest;
            FtpConnection.Method = WebRequestMethods.Ftp.DownloadFile;
            var newFile = System.IO.File.Create(destination + file.Name);
            using (var FtpFile = FtpConnection.GetResponse())
            {
                var response = FtpFile.GetResponseStream();
                using (var content = new StreamReader(response))
                {
                    using (var destinationFile = new StreamWriter(destination + file.Name))
                    {
                        destinationFile.WriteLine(content.ReadLine());
                    }
                }
            }
        }

        public void DeleteFile(File file)
        {
            throw new NotImplementedException();
        }

        public void OpenFile(File file)
        {
            throw new NotImplementedException();
        }
    }
}
