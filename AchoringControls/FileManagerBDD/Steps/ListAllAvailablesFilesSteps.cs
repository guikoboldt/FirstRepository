using FileManagerApp.Entities;
using FileManagerBDD.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace FileManagerBDD.Steps
{
    [Binding]
    public class ListAllAvailablesFilesSteps
    {
        protected DownloadManager downloadManager { get; set; }
        protected StringCollection files { get; set; }

        [Given(@"This is the download path: '(.*)'")]
        public void GivenThisIsTheDownloadPath(string downloadPath)
        {
            downloadManager = new DownloadManager(downloadPath);
        }
        
        [Given(@"there is '(\d)' or more files availables")]
        public void GivenThereIsFilesAvailablesIn(int numberAvailablesFiles)
        {
            downloadManager.numberOfFiles = numberAvailablesFiles;
        }

        [When(@"I click on the FileManager button")]
        public void WhenIClickOnTheFileManagerButton()
        {
            files = new StringCollection();
            files.AddRange(downloadManager.GetAllFilesAsync().Result);
        }

        [Then(@"The following files should appear")]
        public void ThenTheFollowingFilesShouldAppear(Table table)
        {
            var values = table.ToStringCollection();
            Assert.AreEqual(values.Count, files.Count);
        }

    }
}
