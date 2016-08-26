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
        protected DownloadManagerTestContext context { get; set; }

        public ListAllAvailablesFilesSteps(DownloadManagerTestContext context)
        {
            this.context = context;
        }

        [Given(@"The following path '(.*)'")]
        public void GivenTheFollowingPath(string downloadPath)
        {
           context.DownloadManager = new DownloadManager(downloadPath);
        }

        [When(@"I open the DownloadManager")]
        public void WhenIOpenDownloadManager()
        {
           context.Files = context.DownloadManager.Files;
        }

        [Then(@"files should be displayed")]
        public void ThenFilesShouldBeDisplayed()
        {
            context.Files.Should()
                         .NotBeEmpty("The initializer put files into this directory, so if the problem is working we should found some files");
        }

        [Then(@"No files should be displayed")]
        public void ThenNoFilesShoudBeDisplayed()
        {
            context.Files.Should()
                         .BeEmpty("I removed all files before this scenario runs, so no files should be found");
        }                


    }
}
