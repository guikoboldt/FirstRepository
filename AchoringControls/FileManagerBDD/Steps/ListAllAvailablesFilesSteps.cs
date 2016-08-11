using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using TechTalk.SpecFlow;

namespace FileManagerBDD.Steps
{
    [Binding]
    public class ListAllAvailablesFilesSteps
    {
        private string defaultDownloadPath;
        private int numberAvailableFiles;

        [Given(@"This is the download path: '(.*)'")]
        public void GivenThisIsTheDownloadPath(string downloadPath)
        {
            defaultDownloadPath = downloadPath;
        }
        
        [Given(@"there is '(.*)' or more files availables")]
        public void GivenThereIsFilesAvailablesIn(int numberAvailablesFiles)
        {
            this.numberAvailableFiles = numberAvailablesFiles;
        }

        [When(@"I click on the FileManager button")]
        public void WhenIClickOnTheFileManagerButton()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"'(.*)' files availables in the '(.*)' should appear in the screen")]
        public void ThenAllFilesAvailablesShouldAppearInTheScreen(int availablesFilesFound, string downloadPath)
        {
            //get files of the original download path of application
            var defaultDownloadPath = Path.Combine(FileManagerApp.Globals.GlobalInformations.baseDirectory , FileManagerApp.Globals.GlobalInformations.defultDownloadPath);
            var filesDefaultPath = Directory.GetFiles(defaultDownloadPath);

            //get the files of the download path parameter
            var filesParameterPath = Directory.GetFiles(downloadPath);

            Assert.AreEqual(filesDefaultPath.Length, filesParameterPath.Length, availablesFilesFound);

        }
    }
}
