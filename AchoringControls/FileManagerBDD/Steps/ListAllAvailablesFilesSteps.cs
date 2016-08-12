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
        protected FileInfo[] files { get; set; }

        [Given(@"This is the download path: '(.*)'")]
        public void GivenThisIsTheDownloadPath(string downloadPath)
        {
            downloadManager = new DownloadManager(downloadPath);

            for (int i = 0; i < 3; i++) //path files initializer
            {
                File.Create(Path.Combine(downloadPath, @"text" + i + ".txt"));
            }
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
            foreach (var file in files)
            {
                file.Delete();
            }
            this.Invoking(_ => _.WhenIClickOnTheFileManagerButton()).ShouldThrow<NoFilesFoundException>();
        }


    }
}
