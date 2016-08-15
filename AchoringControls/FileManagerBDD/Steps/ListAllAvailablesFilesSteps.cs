using FileManagerApp.Entities;
using FileManagerApp.Exceptions;
using FileManagerBDD.Extensions;
using FluentAssertions;
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
        protected FileInfo[] files { get; set; } = null;

        [Given(@"This is the download path: '(.*)'")]
        public void GivenThisIsTheDownloadPath(string downloadPath)
        {
            downloadManager = new DownloadManager(downloadPath);
        }

        [When(@"I click on the DownloadManager button")]
        public void WhenIClickOnTheFileManagerButton()
        {
            files = downloadManager.GetAllFiles();
        }

        [Then(@"files should be found")]
        public void ThenFileShouldBeFound()
        {
            files.Should()
                 .NotBeEmpty("Files found");
        }

        [Then(@"I should see a message : No files found")]
        public void ThenIShouldSeeAMessageNoFilesFound()
        {
            Task.Run(() =>
            {
                foreach (var file in files)
                {
                    file.Delete();
                };
            }).Wait();

            this.Invoking(_ => _.WhenIClickOnTheFileManagerButton()).ShouldThrow<NoFilesFoundException>();
        }


    }
}
