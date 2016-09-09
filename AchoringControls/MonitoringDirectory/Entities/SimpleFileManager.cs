using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MonitoringDirectory.Entities
{
    public class SimpleFileManager : FileManager
    {
        public DirectoryInfo TargetDirectory { get; set; }

        public SimpleFileManager (string sourcePath, string targetPath)
            : base(sourcePath)
        {
            if (!targetPath.EndsWith(@"\"))
                targetPath = targetPath + @"\";
            this.TargetDirectory = new DirectoryInfo(targetPath);
            if (!TargetDirectory.Exists)
            {
                TargetDirectory.Create();
            }

            base.Files = base.GetFiles(TargetDirectory.FullName);
        }


        public override void OnDeleted(File file)
        {
            var deletedFile = this.Files.FirstOrDefault(_ => _.Name.Equals(file.Name));
            if (deletedFile != null)
            {
                base.DeleteFile(deletedFile);
                this.Files.Remove(deletedFile);
            }
        }

        async public override void OnChanged(File file)
        {
            var changedTargetFile = this.Files.FirstOrDefault(_ => _.Name.Equals(file.Name));
            if (changedTargetFile != null)
            {
                base.DeleteFile(changedTargetFile);
                this.Files.Remove(changedTargetFile);
            }

            var newFile = new File(file.Name, Regex.Replace(file.Location, file.Name, ""));
            await base.CopyFileTo(newFile, TargetDirectory.FullName);
            newFile.Location = TargetDirectory.FullName;
            Files.Add(newFile);
        }

        async public override void OnRenamed(File oldFile, File file)
        {
            var updatedFile = this.Files.FirstOrDefault(_ => _.Name.Equals(oldFile.Name));
            if (updatedFile != null)
            {
                base.DeleteFile(updatedFile);
                this.Files.Remove(updatedFile);
            }
            var newFile = new File(file.Name, Regex.Replace(file.Location, file.Name, ""));
            await base.CopyFileTo(newFile, TargetDirectory.FullName);
            newFile.Location = TargetDirectory.FullName;
            this.Files.Add(newFile);
        }
    }
}
