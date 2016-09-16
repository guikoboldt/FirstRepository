using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MonitoringDirectory.Entities
{
    public class FTPFileManager : Interfaces.IFileProvider
    {
        private FtpWebRequest FtpConnection { get; set; }
        public string FtpURL { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public FTPFileManager(string ftpURL, string user, string password)
        {
            this.FtpURL = ftpURL;
            this.User = user;
            this.Password = password;
            FtpConnection = WebRequest.Create(this.FtpURL) as FtpWebRequest;
            FtpConnection.Credentials = new NetworkCredential(user, password);
            FtpConnection.Method = WebRequestMethods.Ftp.ListDirectory; //loas file's Name
            this.Files = LoadFiles();
        }

        public ICollection<File> Files { get; set; }

        public ICollection<File> LoadFiles()
        {

            ICollection<File> listContent = new List<File>();

            using (var FtpContent = (FtpWebResponse) this.FtpConnection.GetResponse())
            {
                using (var response = FtpContent.GetResponseStream())
                {
                    using (var content = new StreamReader(response))
                    {
                        var line = default(string);
                        while (!string.IsNullOrEmpty(line = content.ReadLine()))
                        {
                            listContent.Add(new File(Regex.Replace(line, @"\w+[/]", ""), this.FtpURL, FtpContent.ContentLength));
                        }
                    }
                }
            }
            return LoadFilesSize(listContent);
        }

        private ICollection<File> LoadFilesSize(ICollection<File> listFiles)
        {
            foreach (var file in listFiles)
            {
                FtpConnection = WebRequest.Create(this.FtpURL + @"/" + file.Name) as FtpWebRequest;
                FtpConnection.Credentials = new NetworkCredential(this.User, this.Password);
                FtpConnection.Method = WebRequestMethods.Ftp.GetFileSize; //loas file's Name
                using (var FtpContent = this.FtpConnection.GetResponse())
                {
                    if (FtpContent.ContentLength > 0)
                    {
                        file.Size = FtpContent.ContentLength;
                    }
                }
            }
            return LoadFileLastModified(listFiles);
        }

        private ICollection<File> LoadFileLastModified(ICollection<File> listFiles)
        {
            foreach (var file in listFiles)
            {
                FtpConnection = WebRequest.Create(this.FtpURL + @"/" + file.Name) as FtpWebRequest;
                FtpConnection.Credentials = new NetworkCredential(this.User, this.Password);
                FtpConnection.Method = WebRequestMethods.Ftp.GetDateTimestamp; //loas file's Name
                using (var FtpContent = (FtpWebResponse)this.FtpConnection.GetResponse())
                {
                    file.LastChange = FtpContent.LastModified;
                }
            }
            return listFiles;
        }

        public void CopyFileTo(File file, string destination)
        {
            FtpConnection = WebRequest.Create(FtpURL + @"/" + file.Name) as FtpWebRequest;
            FtpConnection.Credentials = new NetworkCredential(this.User, this.Password);
            FtpConnection.Method = WebRequestMethods.Ftp.DownloadFile;
            FtpConnection.UseBinary = true;

            using (var FtpFile = FtpConnection.GetResponse())
            {
                using (var response = FtpFile.GetResponseStream())
                {
                    using (var content = new FileStream(System.IO.Path.Combine(destination, file.Name), FileMode.Create))
                    {
                        var buffer = new byte[2048];
                        var bytesRead = default(int);

                        while ((bytesRead = response.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            content.Write(buffer, 0, bytesRead);
                        }
                    }
                }
            }
        }

        public void DeleteFile(File file)
        {
            FtpConnection = WebRequest.Create(FtpURL + @"/" + file.Name) as FtpWebRequest;
            FtpConnection.Credentials = new NetworkCredential(this.User, this.Password);
            FtpConnection.Method = WebRequestMethods.Ftp.DeleteFile;
            FtpConnection.GetResponse();
            this.Files.Remove(file);
        }

        public void OpenFile(File file)
        {
            if (!System.IO.File.Exists(Path.Combine(@"C:\Download\", file.Name)))
            {
                CopyFileTo(file, @"C:\Download\");
            }
            file.Location = Path.Combine(@"C:\Download\");
            Task.Factory.StartNew(() => Process.Start(file.FullPath())).Wait();
        }
    }
}
